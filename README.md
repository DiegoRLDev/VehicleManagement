# VehicleManagement

API REST para gerenciamento de veículos, desenvolvida em **.NET 8**, utilizando arquitetura em camadas, autenticação JWT, FluentValidation e MediatR.

A aplicação oferece gerenciamento de usuários, autenticação e operações de cadastro, consulta, atualização e exclusão de veículos, com foco na organização do código, segurança e separação de responsabilidades.

## Tecnologias

- .NET 8 / ASP.NET Core Web API (Controllers)
- Entity Framework Core 8 (provider InMemory)
- MediatR
- FluentValidation
- JWT Bearer (HMAC-SHA256)
- BCrypt.Net-Next
- Swagger / OpenAPI (Swashbuckle)
- xUnit

## Arquitetura

| Camada | Responsabilidade |
|--------|------------------|
| **WebApi** | Controllers, HTTP, Swagger, autenticação/autorização JWT e composição de DI |
| **Application** | Commands/Queries, Handlers MediatR, Services, FluentValidation, DTOs e Inputs |
| **Domain** | Entidades, enumeradores e interfaces de repositório |
| **Infra** | DbContext, EF Core InMemory e implementações dos repositórios |

Fluxo: `Controller → MediatR → Handler → Service → Repository → DbContext`

### Representações

| Tipo | Onde | Função |
|------|------|--------|
| Request | WebApi | Contrato HTTP de entrada |
| Command/Query | Application | Mensagem MediatR |
| Input | Application | Contrato interno dos Services |
| DTO | Application | Resposta segura (sem dados sensíveis) |

## Decisões técnicas

**Tratamento de exceções:** a API utiliza o middleware nativo do ASP.NET Core 8 com `IExceptionHandler` e `ProblemDetails`, centralizando o tratamento de erros e padronizando as respostas HTTP.

**MediatR e Services:** os handlers coordenam os casos de uso, enquanto os services concentram as regras de aplicação, mantendo os controllers enxutos.

**Autenticação e autorização:** JWT com claims e autorização declarativa por meio de `[Authorize]` e `[Authorize(Roles = "Admin")]`.

**Persistência:** Entity Framework Core InMemory, permitindo executar a aplicação sem configurar um banco de dados externo.

## Pré-requisitos

- .NET 8 SDK (ou SDK mais recente compatível com `net8.0`)
- Runtime ASP.NET Core 8
- PowerShell, bash ou terminal equivalente

Verifique a instalação:

```powershell
dotnet --list-sdks
dotnet --list-runtimes
```

## Como executar

Na raiz da solução:

```powershell
dotnet restore
dotnet build --configuration Release
dotnet run --project src/VehicleManagement.WebApi --launch-profile http
```

A API sobe em Development por padrão via `launchSettings.json`.

- Swagger UI: `http://localhost:5095/swagger`
- OpenAPI JSON: `http://localhost:5095/swagger/v1/swagger.json`
- Health: `http://localhost:5095/api/health`

Perfil HTTPS (opcional):

```powershell
dotnet run --project src/VehicleManagement.WebApi --launch-profile https
```

### Configuração JWT

A seção `Jwt` exige `Issuer`, `Audience`, `SigningKey` e `ExpirationMinutes`.

- `appsettings.json`: `SigningKey` vazia (não use chave de produção no código).
- `appsettings.Development.json`: chave **somente para desenvolvimento local**.
- Outros ambientes: User Secrets ou variável de ambiente `Jwt__SigningKey`.

Exemplo com User Secrets:

```powershell
dotnet user-secrets set "Jwt:SigningKey" "sua-chave-segura-com-pelo-menos-32-caracteres" --project src/VehicleManagement.WebApi
```

### Configuração do administrador (Development)

Em Development, a aplicação pode criar um usuário administrador inicial de forma idempotente (não duplica se o login já existir).

A seção `SeedAdmin` aceita `Login`, `Nome` e `Senha`. Login e Nome podem estar em `appsettings.Development.json`; a **senha não deve ser versionada**.

Configure a senha via User Secrets ou variável de ambiente:

```powershell
dotnet user-secrets set "SeedAdmin:Senha" "sua-senha-segura" --project src/VehicleManagement.WebApi
```

```powershell
$env:SeedAdmin__Senha = "sua-senha-segura"
```

Se `SeedAdmin:Senha` estiver ausente ou vazia, a API sobe normalmente e registra um log informativo — o administrador não é criado.

## Autenticação e autorização

| Role | Origem | Permissões |
|------|--------|------------|
| **User** | Cadastro público (`POST /api/usuarios`) | CRUD de veículos |
| **Admin** | Seed em Development (quando `SeedAdmin:Senha` estiver configurada) | CRUD de veículos + `GET /api/usuarios/{id}` |

O cadastro público cria exclusivamente usuários com perfil `User` e **não aceita** role no body. Qualquer usuário autenticado (`User` ou `Admin`) pode executar o CRUD de veículos; a consulta de usuários por ID é exclusiva do perfil `Admin`.

### Fluxo no Swagger

1. Inicie a API (`dotnet run --project src/VehicleManagement.WebApi --launch-profile http`).
2. Abra o Swagger em `http://localhost:5095/swagger`.
3. Execute **POST `/api/usuarios`** para cadastrar um usuário comum (perfil `User`).
4. Execute **POST `/api/auth/login`** com o mesmo login/senha e copie o campo `token` da resposta.
5. Clique em **Authorize**, cole **apenas o token** (sem o prefixo `Bearer` — o Swagger aplica automaticamente) e confirme.
6. Execute o CRUD de **`/api/veiculos`** (qualquer autenticado).
7. Com o token de `User`, tente **GET `/api/usuarios/{id}`** e confira o retorno **403**.
8. Configure `SeedAdmin:Senha`, reinicie a API, faça login com o administrador, autorize com o novo token e execute **GET `/api/usuarios/{id}`** (esperado **200**).

Endpoints públicos: cadastro de usuário, login e health.  
Endpoints protegidos: CRUD de veículos (qualquer autenticado) e consulta de usuário (somente Admin).

## Exemplos JSON

### POST `/api/usuarios`

```json
{
  "nome": "Diego Lima",
  "login": "diego.lima",
  "senha": "senha123"
}
```

### POST `/api/auth/login`

```json
{
  "login": "diego.lima",
  "senha": "senha123"
}
```

Resposta (exemplo):

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresAt": "2026-10-26T03:00:00Z"
}
```

### POST `/api/veiculos` (requer JWT)

```json
{
  "descricao": "Volkswagen Golf GTI",
  "marca": 7,
  "modelo": "Golf",
  "opcionais": "Teto solar, bancos de couro, central multimidia",
  "valor": 185000
}
```

Valores de `marca` (enum):

| Valor | Marca |
|------:|-------|
| 1 | Chevrolet |
| 2 | Fiat |
| 3 | Ford |
| 4 | Honda |
| 5 | Hyundai |
| 6 | Toyota |
| 7 | Volkswagen |

### PUT `/api/veiculos/{id}` (requer JWT)

```json
{
  "descricao": "Volkswagen Golf GTI - Atualizado",
  "marca": 7,
  "modelo": "Golf",
  "opcionais": "Teto solar, bancos de couro, central multimidia, sensores de estacionamento",
  "valor": 190000
}
```

## Endpoints

| Método | Rota | Auth | Sucesso |
|--------|------|------|---------|
| POST | `/api/usuarios` | Não | 201 |
| GET | `/api/usuarios/{id}` | Sim (Admin) | 200 |
| POST | `/api/auth/login` | Não | 200 |
| POST | `/api/veiculos` | Sim | 201 |
| GET | `/api/veiculos` | Sim | 200 |
| GET | `/api/veiculos/{id}` | Sim | 200 |
| PUT | `/api/veiculos/{id}` | Sim | 204 |
| DELETE | `/api/veiculos/{id}` | Sim | 204 |
| GET | `/api/health` | Não | 200 |

Erros comuns: **400** validação, **401** não autenticado/credenciais inválidas, **403** autenticado sem permissão, **404** recurso inexistente, **409** login duplicado.

## Testes

```powershell
dotnet test --configuration Release
```

Os testes cobrem Domain, repositórios InMemory, Services, FluentValidation, seed Admin, Roles/Claims JWT e integração HTTP (usuários, autorização Admin/User e CRUD de veículos).

## Persistência

Os dados são mantidos em memória durante a execução da API (EF Core InMemory). Não há banco externo a configurar; o estado é reiniciado a cada novo processo.

## Estrutura do repositório

```text
VehicleManagement/
├── src/
│   ├── VehicleManagement.WebApi/
│   ├── VehicleManagement.Application/
│   ├── VehicleManagement.Domain/
│   └── VehicleManagement.Infra/
├── tests/
│   ├── VehicleManagement.Application.Tests/
│   └── VehicleManagement.WebApi.Tests/
├── VehicleManagement.sln
├── README.md
├── .gitignore
└── .editorconfig
```
