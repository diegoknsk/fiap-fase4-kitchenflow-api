# Subtask 05: Configurar DbContext no Program.cs da API

## Descrição
Configurar o `KitchenFlowDbContext` no `Program.cs` da API para que possa ser injetado via Dependency Injection e usado pelos repositórios e serviços.

## Passos de implementação
- Abrir `Program.cs` do projeto `FastFood.KitchenFlow.Api`
- Adicionar referência ao namespace do DbContext: `using FastFood.KitchenFlow.Infra.Persistence;`
- Configurar connection string no `appsettings.json`:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
    }
  }
  ```
- Configurar connection string no `appsettings.Development.json` (mesmo valor, não commitado)
- No `Program.cs`, adicionar configuração do DbContext:
  ```csharp
  builder.Services.AddDbContext<KitchenFlowDbContext>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
  ```
- Verificar que o pacote `Npgsql.EntityFrameworkCore.PostgreSQL` está referenciado (via Infra.Persistence)
- Adicionar `using Microsoft.EntityFrameworkCore;` se necessário

## Referências
- **Auth**: Verificar como o Auth configura o DbContext no Program.cs (se disponível)
- Seguir padrão padrão do ASP.NET Core para configuração de DbContext

## Como testar
- Executar `dotnet build` na API para verificar compilação
- Executar `dotnet run` na API para verificar que inicia sem erros
- Verificar que connection string está sendo lida corretamente (logs ou breakpoint)

## Critérios de aceite
- Connection string configurada em `appsettings.json`:
  - `Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres`
- Connection string configurada em `appsettings.Development.json` (mesmo valor)
- `AddDbContext<KitchenFlowDbContext>` configurado no `Program.cs`
- `UseNpgsql` configurado com connection string
- API compila sem erros
- API inicia sem erros (`dotnet run`)
- DbContext pode ser injetado via DI (quando necessário)
