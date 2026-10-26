using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using VehicleManagement.Domain.Enums;

namespace VehicleManagement.WebApi.Tests;

public class VeiculosApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public VeiculosApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_Sem_Token_Retorna_401()
    {
        var response = await _client.PostAsJsonAsync("/api/veiculos", CreatePayload());
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_Com_Token_Invalido_Retorna_401()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/veiculos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "token-invalido");

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_Com_Dados_Invalidos_Retorna_400()
    {
        var token = await CreateAuthenticatedUserAsync();

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/veiculos")
        {
            Content = JsonContent.Create(new
            {
                descricao = "",
                marca = 999,
                modelo = "",
                opcionais = (string?)null,
                valor = -10
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_Por_Id_Inexistente_Retorna_404()
    {
        var token = await CreateAuthenticatedUserAsync();

        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/veiculos/{Guid.NewGuid()}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Crud_Completo_Com_Autenticacao()
    {
        var token = await CreateAuthenticatedUserAsync();

        // POST → 201
        using var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/veiculos")
        {
            Content = JsonContent.Create(CreatePayload())
        };
        createRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.SendAsync(createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        var id = created.GetProperty("id").GetGuid();
        Assert.Equal("Corolla XEi", created.GetProperty("descricao").GetString());

        // GET lista → 200
        using var listRequest = new HttpRequestMessage(HttpMethod.Get, "/api/veiculos");
        listRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var listResponse = await _client.SendAsync(listRequest);
        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);
        var lista = await listResponse.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.True(lista.GetArrayLength() >= 1);

        // GET por Id → 200
        using var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/veiculos/{id}");
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var getResponse = await _client.SendAsync(getRequest);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        // PUT → 204
        using var putRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/veiculos/{id}")
        {
            Content = JsonContent.Create(new
            {
                descricao = "Corolla XEi Atualizado",
                marca = (int)Marca.Toyota,
                modelo = "Corolla",
                opcionais = "Bancos de couro",
                valor = 150000m
            })
        };
        putRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var putResponse = await _client.SendAsync(putRequest);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        using var getAfterPut = new HttpRequestMessage(HttpMethod.Get, $"/api/veiculos/{id}");
        getAfterPut.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var afterPut = await (await _client.SendAsync(getAfterPut)).Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal("Corolla XEi Atualizado", afterPut.GetProperty("descricao").GetString());
        Assert.Equal("Bancos de couro", afterPut.GetProperty("opcionais").GetString());

        // DELETE → 204
        using var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/veiculos/{id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var deleteResponse = await _client.SendAsync(deleteRequest);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        using var getAfterDelete = new HttpRequestMessage(HttpMethod.Get, $"/api/veiculos/{id}");
        getAfterDelete.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var afterDelete = await _client.SendAsync(getAfterDelete);
        Assert.Equal(HttpStatusCode.NotFound, afterDelete.StatusCode);
    }

    [Fact]
    public async Task Post_Valido_Retorna_201()
    {
        var token = await CreateAuthenticatedUserAsync();

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/veiculos")
        {
            Content = JsonContent.Create(CreatePayload())
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Get_Lista_Retorna_200()
    {
        var token = await CreateAuthenticatedUserAsync();

        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/veiculos");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Crud_Acessivel_Com_Token_Admin()
    {
        var adminToken = await LoginAsync(
            CustomWebApplicationFactory.SeedAdminLogin,
            CustomWebApplicationFactory.SeedAdminSenha);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/veiculos")
        {
            Content = JsonContent.Create(CreatePayload())
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private static object CreatePayload() => new
    {
        descricao = "Corolla XEi",
        marca = (int)Marca.Toyota,
        modelo = "Corolla",
        opcionais = "Ar-condicionado",
        valor = 145000m
    };

    private async Task<string> CreateAuthenticatedUserAsync()
    {
        var login = $"veh_{Guid.NewGuid():N}"[..20];
        var createUser = await _client.PostAsJsonAsync("/api/usuarios", new
        {
            nome = "Usuario Veiculos",
            login,
            senha = "senha123"
        });
        createUser.EnsureSuccessStatusCode();

        return await LoginAsync(login, "senha123");
    }

    private async Task<string> LoginAsync(string login, string senha)
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { login, senha });
        loginResponse.EnsureSuccessStatusCode();

        var body = await loginResponse.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return body.GetProperty("token").GetString()!;
    }
}
