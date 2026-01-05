# Subtask 01: Criar Estrutura de Autenticação na Infra

## Descrição
Criar a estrutura de pastas e arquivos base para autenticação na camada Infra, seguindo exatamente o padrão do projeto OrderHub.

## Passos de implementação
- Abrir projeto `FastFood.KitchenFlow.Infra` no Solution Explorer
- Criar pasta `Auth/` no projeto `FastFood.KitchenFlow.Infra`
- Verificar que a pasta foi criada corretamente na estrutura do projeto
- Preparar para criar os arquivos:
  - `CognitoOptions.cs`
  - `CognitoAuthenticationConfig.cs`
  - `AuthorizationConfig.cs`
  - `JwtAuthenticationConfig.cs` (opcional - apenas se Customer JWT for necessário)

## Referências
- **OrderHub**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\`
- Estrutura deve ser idêntica ao OrderHub

## Como testar
- Verificar que a pasta `Auth/` existe no projeto `FastFood.KitchenFlow.Infra`
- Verificar que o projeto compila sem erros após criar a pasta

## Critérios de aceite
- Pasta `Auth/` criada em `FastFood.KitchenFlow.Infra/Auth/`
- Pasta visível no Solution Explorer
- Projeto compila sem erros
- Estrutura pronta para receber os arquivos de configuração
