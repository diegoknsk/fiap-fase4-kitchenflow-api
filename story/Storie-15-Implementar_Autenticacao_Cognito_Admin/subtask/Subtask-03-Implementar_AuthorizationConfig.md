# Subtask 03: Implementar AuthorizationConfig

## Descrição
Criar o arquivo `AuthorizationConfig.cs` copiando exatamente do projeto OrderHub e adaptando apenas o namespace. Este arquivo define as políticas de autorização (Admin, Customer, etc.).

## Passos de implementação
- Copiar arquivo `AuthorizationConfig.cs` de:
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\AuthorizationConfig.cs`
- Colar em `FastFood.KitchenFlow.Infra/Auth/AuthorizationConfig.cs`
- Alterar namespace de `FastFood.OrderHub.Infra.Auth` para `FastFood.KitchenFlow.Infra.Auth`
- Verificar que a política `Admin` está configurada corretamente:
  - Deve exigir `RequireAuthenticatedUser()`
  - Deve exigir claim `scope` com valor `aws.cognito.signin.user.admin`
- Se Customer JWT não for necessário, pode remover a política `Customer` e `CustomerWithScope` (ou manter para compatibilidade futura)
- Verificar que todas as referências estão corretas

## Referências
- **OrderHub AuthorizationConfig**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\AuthorizationConfig.cs`
- Política Admin deve validar claim `scope` com valor `aws.cognito.signin.user.admin`

## Dependências NuGet necessárias
- `Microsoft.AspNetCore.Authorization`
- `Microsoft.Extensions.DependencyInjection`

## Como testar
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Infra`
- Verificar que não há erros de compilação
- Verificar que a constante `AdminPolicy` está definida como `"Admin"`

## Critérios de aceite
- Arquivo `AuthorizationConfig.cs` criado em `FastFood.KitchenFlow.Infra/Auth/AuthorizationConfig.cs`
- Namespace alterado para `FastFood.KitchenFlow.Infra.Auth`
- Política `Admin` configurada corretamente:
  - `RequireAuthenticatedUser()`
  - `RequireClaim("scope", "aws.cognito.signin.user.admin")`
- Constante `AdminPolicy = "Admin"` definida
- Método `AddAuthorizationPolicies()` implementado
- Projeto compila sem erros
- Código idêntico ao OrderHub (apenas namespace diferente)
