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

Exemplo com User Secrets (na pasta da WebApi):

```powershell
dotnet user-secrets init --project src/VehicleManagement.WebApi
dotnet user-secrets set "Jwt:SigningKey" "sua-chave-segura-com-pelo-menos-32-caracteres" --project src/VehicleManagement.WebApi
```

## Autenticação (fluxo no Swagger)

1. Abra `http://localhost:5095/swagger`.
2. Execute **POST `/api/usuarios`** para cadastrar o primeiro usuário (público).
3. Execute **POST `/api/auth/login`** com o mesmo login/senha.
4. Copie o valor do campo `token` na resposta.
5. Clique em **Authorize**, cole **apenas o token** (sem o prefixo `Bearer`) e confirme.
6. Execute os endpoints de **GET `/api/usuarios/{id}`** e o CRUD de **`/api/veiculos`**.

Endpoints públicos: cadastro de usuário e login.  
Endpoints protegidos: consulta de usuário e todo o CRUD de veículos.

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
| GET | `/api/usuarios/{id}` | Sim | 200 |
| POST | `/api/auth/login` | Não | 200 |
| POST | `/api/veiculos` | Sim | 201 |
| GET | `/api/veiculos` | Sim | 200 |
| GET | `/api/veiculos/{id}` | Sim | 200 |
| PUT | `/api/veiculos/{id}` | Sim | 204 |
| DELETE | `/api/veiculos/{id}` | Sim | 204 |
| GET | `/api/health` | Não | 200 |

Erros comuns: **400** validação, **401** não autenticado/credenciais inválidas, **404** recurso inexistente, **409** login duplicado.

## Testes

```powershell
dotnet test --configuration Release
```

Os testes cobrem Domain, repositórios InMemory, Services, FluentValidation e integração HTTP (usuários, JWT e CRUD de veículos).

## Persistência

A aplicação utiliza **Entity Framework Core InMemory** para armazenamento dos dados durante a execução, sem necessidade de configuração de um banco de dados externo.

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
