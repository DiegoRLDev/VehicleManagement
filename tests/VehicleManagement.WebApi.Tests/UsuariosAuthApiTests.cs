using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace VehicleManagement.WebApi.Tests;

public class UsuariosAuthApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public UsuariosAuthApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Usuarios_Cadastra_Com_Sucesso_Como_User()
    {
        var login = $"user_{Guid.NewGuid():N}"[..20];
        var response = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Ana Silva",
            login,
            senha = "senha123"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal("Ana Silva", body.GetProperty("nome").GetString());
        Assert.Equal(login, body.GetProperty("login").GetString());
        Assert.Equal("User", body.GetProperty("role").GetString());
        Assert.False(body.TryGetProperty("senha", out _));
        Assert.False(body.TryGetProperty("senhaHash", out _));
    }

    [Fact]
    public async Task Post_Usuarios_Com_Role_No_Body_Ignora_E_Mantem_User()
    {
        var login = $"role_{Guid.NewGuid():N}"[..20];
        var response = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Tentativa Admin",
            login,
            senha = "senha123",
            role = "Admin"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal("User", body.GetProperty("role").GetString());
    }

    [Fact]
    public async Task Post_Usuarios_Com_Dados_Invalidos_Retorna_400()
    {
        var response = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Ab",
            login = "a",
            senha = "123"
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_Usuarios_Com_Login_Duplicado_Retorna_409()
    {
        var login = $"dup_{Guid.NewGuid():N}"[..20];
        var payload = new { nome = "Ana Silva", login, senha = "senha123" };

        var first = await _client.PostAsJsonAsync("/api/usuarios", payload);
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);

        var second = await _client.PostAsJsonAsync("/api/usuarios", payload);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task Get_Usuarios_Sem_Token_Retorna_401()
    {
        var response = await _client.GetAsync($"/api/usuarios/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_Usuarios_Com_Token_Invalido_Retorna_401()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/usuarios/{Guid.NewGuid()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token-invalido");

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_Usuarios_Com_Token_User_Retorna_403()
    {
        var login = $"usr_{Guid.NewGuid():N}"[..20];
        var createResponse = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Bruno Costa",
            login,
            senha = "senha123"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        var id = created.GetProperty("id").GetGuid();

        var token = await LoginAsync(login, "senha123");

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/usuarios/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_Usuarios_Existente_Com_Token_Admin_Retorna_200()
    {
        var login = $"ok_{Guid.NewGuid():N}"[..20];
        var createResponse = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Bruno Costa",
            login,
            senha = "senha123"
        });
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        var id = created.GetProperty("id").GetGuid();

        var adminToken = await LoginAsync(
            CustomWebApplicationFactory.SeedAdminLogin,
            CustomWebApplicationFactory.SeedAdminSenha);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/usuarios/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal(id, body.GetProperty("id").GetGuid());
        Assert.Equal("User", body.GetProperty("role").GetString());
        Assert.False(body.TryGetProperty("senha", out _));
        Assert.False(body.TryGetProperty("senhaHash", out _));
    }

    [Fact]
    public async Task Get_Usuarios_Inexistente_Com_Token_Admin_Retorna_404()
    {
        var adminToken = await LoginAsync(
            CustomWebApplicationFactory.SeedAdminLogin,
            CustomWebApplicationFactory.SeedAdminSenha);

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/usuarios/{Guid.NewGuid()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_Auth_Login_Valido_Retorna_Token()
    {
        var login = $"auth_{Guid.NewGuid():N}"[..20];
        await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Diego Lima",
            login,
            senha = "senha123"
        });

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            login,
            senha = "senha123"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal("Bearer", body.GetProperty("tokenType").GetString());
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("token").GetString()));
        Assert.True(body.GetProperty("expiresAt").GetDateTime() > DateTime.UtcNow);
    }

    [Fact]
    public async Task Post_Auth_Login_Admin_Seed_Retorna_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            login = CustomWebApplicationFactory.SeedAdminLogin,
            senha = CustomWebApplicationFactory.SeedAdminSenha
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("token").GetString()));
    }

    [Fact]
    public async Task Post_Auth_Login_Com_Senha_Incorreta_Retorna_401()
    {
        var login = $"bad_{Guid.NewGuid():N}"[..20];
        await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Eva Souza",
            login,
            senha = "senha123"
        });

        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            login,
            senha = "errada"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_Auth_Login_Com_Usuario_Inexistente_Retorna_401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            login = "nao.existe",
            senha = "senha123"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_Auth_Login_Invalido_Retorna_400()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            login = "",
            senha = ""
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<string> LoginAsync(string login, string senha)
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { login, senha });
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return body.GetProperty("token").GetString()!;
    }
}
