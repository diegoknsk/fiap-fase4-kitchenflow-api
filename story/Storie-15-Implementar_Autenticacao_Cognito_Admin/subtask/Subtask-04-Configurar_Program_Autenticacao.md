# Subtask 04: Configurar Program.cs com Autenticação

## Descrição
Atualizar o `Program.cs` da API para configurar autenticação Cognito e políticas de autorização, seguindo exatamente o padrão do OrderHub.

## Passos de implementação
- Abrir arquivo `src/InterfacesExternas/FastFood.KitchenFlow.Api/Program.cs`
- Adicionar `using` no início do arquivo:
  - `using FastFood.KitchenFlow.Infra.Auth;`
  - `using Microsoft.AspNetCore.Authentication.JwtBearer;`
- Adicionar **ANTES** de `var builder = WebApplication.CreateBuilder(args);`:
  ```csharp
  // Configurar JWT Security Token Handler
  JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler();
  ```
- Após `builder.Services.AddSwaggerGen();`, adicionar configuração de autenticação:
  ```csharp
  // Configure authentication
  builder.Services
      .AddAuthentication(options =>
      {
          options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddCognitoJwtBearer(builder.Configuration);

  // Configure authorization policies
  builder.Services.AddAuthorizationPolicies();
  ```
- No pipeline HTTP (após `var app = builder.Build();`), adicionar **ANTES** de `app.UseAuthorization();`:
  ```csharp
  app.UseAuthentication();
  ```
- Verificar que `app.UseAuthentication()` está antes de `app.UseAuthorization()`
- Verificar que a ordem está correta:
  1. `app.UseSwagger()` / `app.UseSwaggerUI()` (se Development)
  2. `app.UseHttpsRedirection()` (se Development)
  3. `app.UseCors()`
  4. `app.UseAuthentication()` ← **NOVO**
  5. `app.UseAuthorization()`
  6. `app.MapControllers()`

## Referências
- **OrderHub Program.cs**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\Program.cs`
- Linhas 13-14: `JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler();`
- Linhas 33-43: Configuração de autenticação e autorização
- Linha 122: `app.UseAuthentication();`

## Dependências NuGet necessárias
- Já devem estar no projeto `FastFood.KitchenFlow.Infra` (referenciado pela API)

## Como testar
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Api`
- Verificar que não há erros de compilação
- Verificar que a aplicação inicia sem erros (não precisa ter token ainda)

## Critérios de aceite
- `JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler()` chamado no início
- Autenticação configurada via `.AddCognitoJwtBearer(builder.Configuration)`
- Políticas de autorização configuradas via `.AddAuthorizationPolicies()`
- `app.UseAuthentication()` adicionado antes de `app.UseAuthorization()`
- Ordem correta do pipeline HTTP
- Projeto compila sem erros
- Aplicação inicia sem erros (mesmo sem configurações de Cognito ainda)
