# Subtask 07: Testar Autenticação Localmente

## Descrição
Testar a autenticação Cognito localmente, verificando que os endpoints estão protegidos corretamente e que o endpoint anônimo funciona.

## Passos de implementação
- Configurar variáveis de ambiente locais (se necessário):
  - `Authentication__Cognito__Region` ou `COGNITO__REGION`
  - `Authentication__Cognito__UserPoolId` ou `COGNITO__USERPOOLID`
  - `Authentication__Cognito__ClientId` ou `COGNITO__CLIENTID`
- Executar a aplicação: `dotnet run` no projeto `FastFood.KitchenFlow.Api`
- Abrir Swagger UI: `https://localhost:5001/swagger` (ou porta configurada)
- Testar endpoints **SEM token**:
  - `POST /api/preparations` → Deve retornar **201 Created** (anônimo permitido)
  - `GET /api/preparations` → Deve retornar **401 Unauthorized**
  - `POST /api/preparations/take-next` → Deve retornar **401 Unauthorized**
  - `POST /api/preparations/{id}/finish` → Deve retornar **401 Unauthorized**
  - `GET /api/deliveries/ready` → Deve retornar **401 Unauthorized`
  - `POST /api/deliveries/{id}/finalize` → Deve retornar **401 Unauthorized`
- Obter token Cognito (se disponível):
  - Usar endpoint de login do Auth service
  - Ou usar AWS CLI: `aws cognito-idp admin-initiate-auth ...`
- Testar endpoints **COM token Cognito válido**:
  - Adicionar token no header: `Authorization: Bearer <token>`
  - Todos os endpoints (exceto CreatePreparation) devem retornar **200 OK** ou **201 Created**
- Testar token Cognito **SEM política Admin**:
  - Deve retornar **403 Forbidden** nos endpoints protegidos

## Referências
- **OrderHub**: Testar seguindo o mesmo padrão
- Documentação AWS Cognito: Como obter tokens para testes

## Pré-requisitos
- Aplicação rodando localmente
- Configurações de Cognito válidas (UserPoolId, ClientId)
- Token Cognito válido para testes (ou mock se necessário)

## Como testar
- Usar Postman, Insomnia ou Swagger UI
- Verificar status codes HTTP corretos
- Verificar mensagens de erro quando aplicável

## Critérios de aceite
- Endpoint `POST /api/preparations` funciona **SEM token** (retorna 201)
- Demais endpoints retornam **401 Unauthorized** **SEM token**
- Endpoints retornam **200/201** **COM token Cognito válido com política Admin**
- Endpoints retornam **403 Forbidden** **COM token Cognito válido SEM política Admin**
- Mensagens de erro são claras e informativas
- Swagger UI mostra corretamente quais endpoints requerem autenticação

## Observações
- Se não houver acesso ao Cognito localmente, pode ser necessário mockar a validação de token
- Em ambiente de desenvolvimento, pode ser necessário desabilitar validação de HTTPS para Cognito (`RequireHttpsMetadata = false`)
