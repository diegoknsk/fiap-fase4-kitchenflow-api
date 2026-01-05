# Subtask 02: Implementar CognitoAuthenticationConfig

## Descrição
Criar os arquivos `CognitoOptions.cs` e `CognitoAuthenticationConfig.cs` copiando exatamente do projeto OrderHub e adaptando apenas o namespace.

## Passos de implementação
- Copiar arquivo `CognitoOptions.cs` de:
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\CognitoOptions.cs`
- Colar em `FastFood.KitchenFlow.Infra/Auth/CognitoOptions.cs`
- Alterar namespace de `FastFood.OrderHub.Infra.Auth` para `FastFood.KitchenFlow.Infra.Auth`
- Copiar arquivo `CognitoAuthenticationConfig.cs` de:
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\CognitoAuthenticationConfig.cs`
- Colar em `FastFood.KitchenFlow.Infra/Auth/CognitoAuthenticationConfig.cs`
- Alterar namespace de `FastFood.OrderHub.Infra.Auth` para `FastFood.KitchenFlow.Infra.Auth`
- Verificar que todas as referências estão corretas
- Verificar que o código está idêntico ao OrderHub (apenas namespace diferente)

## Referências
- **OrderHub CognitoOptions**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\CognitoOptions.cs`
- **OrderHub CognitoAuthenticationConfig**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\CognitoAuthenticationConfig.cs`

## Dependências NuGet necessárias
- `Microsoft.AspNetCore.Authentication.JwtBearer` (versão compatível com .NET 8)
- `Microsoft.Extensions.Configuration`
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Extensions.Options`
- `Microsoft.IdentityModel.Tokens`

## Como testar
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Infra`
- Verificar que não há erros de compilação
- Verificar que os namespaces estão corretos

## Critérios de aceite
- Arquivo `CognitoOptions.cs` criado em `FastFood.KitchenFlow.Infra/Auth/CognitoOptions.cs`
- Arquivo `CognitoAuthenticationConfig.cs` criado em `FastFood.KitchenFlow.Infra/Auth/CognitoAuthenticationConfig.cs`
- Namespace alterado para `FastFood.KitchenFlow.Infra.Auth`
- Código idêntico ao OrderHub (apenas namespace diferente)
- Projeto compila sem erros
- Dependências NuGet adicionadas corretamente
