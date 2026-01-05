# Subtask 06: Configurar appsettings.json

## Descrição
Adicionar as configurações de autenticação Cognito no `appsettings.json` e `appsettings.Development.json`, seguindo o padrão do OrderHub.

## Passos de implementação
- Abrir arquivo `src/InterfacesExternas/FastFood.KitchenFlow.Api/appsettings.json`
- Adicionar seção `Authentication` após a seção `AllowedHosts`:
  ```json
  "Authentication": {
    "Cognito": {
      "Region": "us-east-1",
      "UserPoolId": "",
      "ClientId": "",
      "ClockSkewMinutes": 5
    }
  }
  ```
- Se Customer JWT for necessário, adicionar também:
  ```json
  "JwtCustomer": {
    "Issuer": "FastFood.Auth",
    "Audience": "FastFood.API",
    "SecretKey": ""
  }
  ```
- Abrir arquivo `src/InterfacesExternas/FastFood.KitchenFlow.Api/appsettings.Development.json`
- Adicionar as mesmas seções (pode deixar vazias ou preencher com valores de desenvolvimento se disponíveis)
- Verificar que o JSON está válido (sem vírgulas extras, chaves fechadas, etc.)

## Referências
- **OrderHub appsettings.json**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\appsettings.json`
  - Linhas 16-22: Seção `Authentication:Cognito`
  - Linhas 24-27: Seção `JwtCustomer`

## Observações importantes
- Os valores vazios (`UserPoolId`, `ClientId`, `SecretKey`) serão preenchidos via variáveis de ambiente no cluster Kubernetes
- `Region` e `ClockSkewMinutes` podem ficar no appsettings.json (valores não sensíveis)
- Valores sensíveis devem vir de variáveis de ambiente ou secrets do Kubernetes

## Como testar
- Verificar que o JSON está válido (usar validador JSON online se necessário)
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Api`
- Verificar que a aplicação consegue ler as configurações (mesmo vazias)

## Critérios de aceite
- Seção `Authentication:Cognito` adicionada em `appsettings.json`
- Propriedades configuradas:
  - `Region`: "us-east-1"
  - `UserPoolId`: "" (vazio, será preenchido via variável de ambiente)
  - `ClientId`: "" (vazio, será preenchido via variável de ambiente)
  - `ClockSkewMinutes`: 5
- Seção `JwtCustomer` adicionada (se necessário)
- `appsettings.Development.json` atualizado com as mesmas seções
- JSON válido (sem erros de sintaxe)
- Projeto compila sem erros
- Aplicação consegue ler as configurações
