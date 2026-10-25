# VehicleManagement

API REST para gerenciamento de veículos, desenvolvida como teste técnico em .NET 8.

O projeto utiliza arquitetura em camadas com ASP.NET Core Web API (Controllers), MediatR, FluentValidation, Entity Framework Core InMemory e autenticação JWT.

## Arquitetura

| Camada | Responsabilidade |
|--------|------------------|
| `WebApi` | Controllers, HTTP, Swagger, autenticação/autorização e composição de DI |
| `Application` | Commands/Queries, Handlers MediatR, Services, FluentValidation e casos de uso |
| `Domain` | Entidades, enumeradores, interfaces de repositório e regras de domínio |
| `Infra` | DbContext, EF Core InMemory e implementações de repositório |

Fluxo: `Controller → MediatR → Handler → Service → Repository → DbContext`

## Estrutura

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
└── VehicleManagement.sln
```

## Como executar

```powershell
dotnet restore
dotnet build --configuration Release
dotnet run --project src/VehicleManagement.WebApi
```

Swagger (Development): `/swagger`

## Status

Fase P-0 concluída: solução, referências, pacotes de fundação, WebApi com Controllers e Swagger.
