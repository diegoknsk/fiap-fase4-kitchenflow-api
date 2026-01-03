# Storie-06: Criar PreparationController (Endpoint para Iniciar Preparo via Payment)

## Descrição
Como desenvolvedor, quero criar o `PreparationController` com o endpoint principal que será chamado pelo microserviço Payment quando um pagamento for confirmado, para iniciar o fluxo de preparação na cozinha. O endpoint deve receber os dados do pedido (OrderId e OrderSnapshot) e criar uma Preparation no banco de dados.

## Objetivo
Criar o `PreparationController` e toda a camada Application necessária (UseCase, InputModel, OutputModel, Presenter, Port) para suportar o endpoint principal:
- `POST /api/preparations` - Criar nova preparação quando pagamento for confirmado

Este endpoint será chamado pelo microserviço Payment após confirmação de pagamento.

## Escopo Técnico
- **Tecnologias**: .NET 8, ASP.NET Core, Entity Framework Core
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Api` (Controller)
  - `FastFood.KitchenFlow.Application` (UseCase, InputModel, OutputModel, Presenter, Port)
  - `FastFood.KitchenFlow.Infra.Persistence` (Repository/DataSource implementando Port)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de controllers e use cases)
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda` (padrão de Application layer)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\01-InterfacesExternas\FastFood.Api\Controllers\PreparationController.cs`
  - Adaptar para o novo modelo com OrderSnapshot

## Endpoint Principal

### POST /api/preparations
**Descrição**: Criar nova preparação quando pagamento for confirmado pelo Payment.

**Request Body**:
```json
{
  "orderId": "guid",
  "orderSnapshot": "{ ... json do pedido ... }"
}
```

**Response 201 Created**:
```json
{
  "id": "guid",
  "orderId": "guid",
  "status": 0,
  "createdAt": "2024-01-01T00:00:00Z",
  "message": "Preparação criada com sucesso"
}
```

**Response 400 Bad Request**: Dados inválidos
**Response 409 Conflict**: Preparação já existe para este pedido (idempotência)

## Fluxo de Integração Payment → KitchenFlow

1. Payment confirma pagamento
2. Payment chama `POST /api/preparations` do KitchenFlow
3. KitchenFlow recebe `OrderId` e `OrderSnapshot`
4. KitchenFlow valida dados
5. KitchenFlow verifica idempotência (não criar duplicado)
6. KitchenFlow cria `Preparation` com status `Received`
7. KitchenFlow retorna resposta de sucesso

## Subtasks

- [x] [Subtask 01: Criar Port IPreparationRepository](./subtask/Subtask-01-Criar_Port_IPreparationRepository.md)
- [x] [Subtask 02: Criar Repository/DataSource implementando Port](./subtask/Subtask-02-Criar_Repository_DataSource.md)
- [x] [Subtask 03: Criar InputModel, OutputModel e Response](./subtask/Subtask-03-Criar_Models.md)
- [x] [Subtask 04: Criar UseCase CreatePreparationUseCase](./subtask/Subtask-04-Criar_UseCase.md)
- [x] [Subtask 05: Criar Presenter](./subtask/Subtask-05-Criar_Presenter.md)
- [x] [Subtask 06: Criar PreparationController](./subtask/Subtask-06-Criar_Controller.md)
- [x] [Subtask 07: Configurar Dependency Injection](./subtask/Subtask-07-Configurar_DI.md)
- [x] [Subtask 08: Validar endpoint completo](./subtask/Subtask-08-Validar_endpoint.md)

## Critérios de Aceite da História

- [x] Port `IPreparationRepository` criado em `Application/Ports/`:
  - Método `CreateAsync(Preparation)` para criar preparação
  - Método `GetByOrderIdAsync(Guid orderId)` para verificar idempotência
- [x] Repository/DataSource criado em `Infra.Persistence/Repositories/`:
  - Implementa `IPreparationRepository`
  - Usa `KitchenFlowDbContext` para persistir
  - Mapeia entre entidade de domínio e entidade de persistência
- [x] InputModel `CreatePreparationInputModel` criado:
  - `OrderId` (Guid)
  - `OrderSnapshot` (string)
- [x] OutputModel `CreatePreparationOutputModel` criado:
  - `Id` (Guid)
  - `OrderId` (Guid)
  - `Status` (int)
  - `CreatedAt` (DateTime)
- [x] Response `CreatePreparationResponse` criado:
  - Propriedades do OutputModel
  - `Message` (string)
- [x] UseCase `CreatePreparationUseCase` criado:
  - Recebe `CreatePreparationInputModel`
  - Valida dados
  - Verifica idempotência (não criar duplicado)
  - Cria entidade de domínio `Preparation`
  - Chama repository para persistir
  - Chama presenter para transformar
  - Retorna `CreatePreparationResponse`
- [x] Presenter `CreatePreparationPresenter` criado:
  - Transforma `CreatePreparationOutputModel` em `CreatePreparationResponse`
- [x] Controller `PreparationController` criado:
  - Endpoint `POST /api/preparations`
  - Valida ModelState
  - Mapeia Request para InputModel
  - Chama UseCase
  - Retorna HTTP Response apropriado
- [x] Dependency Injection configurado no `Program.cs`:
  - `IPreparationRepository` → `PreparationRepository`
  - `CreatePreparationUseCase` registrado
- [x] Endpoint testado e funcionando:
  - Cria preparação com sucesso (201)
  - Retorna erro para dados inválidos (400)
  - Retorna dados existentes para preparação duplicada (idempotência)
- [x] Swagger documenta o endpoint corretamente
- [x] Código segue padrão do OrderHub/Auth

## Observações Arquiteturais

### Clean Architecture
- **Controller**: Apenas recebe request, valida, mapeia e chama UseCase
- **UseCase**: Contém lógica de negócio, validações, chamadas a Ports
- **Port**: Interface na Application, define contrato
- **Repository**: Implementação na Infra.Persistence, acessa DbContext
- **Presenter**: Transforma OutputModel em Response (chamado pelo UseCase)

### Idempotência
- **Importante**: Verificar se já existe Preparation para o OrderId antes de criar
- **Motivo**: Payment pode chamar o endpoint múltiplas vezes (retry, etc.)
- **Solução**: Usar `GetByOrderIdAsync` para verificar existência
- **Retorno**: Se já existe, retornar 409 Conflict ou 200 OK com dados existentes

### OrderSnapshot
- **Formato**: String JSON (será armazenado como jsonb no banco)
- **Conteúdo**: Snapshot completo do pedido no momento do pagamento
- **Uso**: Exibição no display da cozinha, referência para entrega, auditoria
- **Imutável**: Não deve ser alterado após criação

### Análise dos Projetos de Referência

**OrderHub (Controllers e UseCases):**
- Controllers simples, apenas chamam UseCases
- UseCases pequenos e focados
- InputModels e OutputModels na Application
- Presenters transformam OutputModels em Responses
- Ports definem contratos, implementações na Infra

**Monolito (PreparationController):**
- Usa `PreparationControllerOrchestrator` (padrão antigo)
- Acesso direto a DataSources (não segue Clean Arch)
- Adaptar para novo padrão com UseCases

**Aplicação no KitchenFlow:**
- Seguir padrão do OrderHub (Clean Architecture)
- Criar UseCase focado em criar preparação
- Implementar idempotência
- Suportar OrderSnapshot

### Endpoint para Payment
Este é o endpoint principal que o Payment chamará. Outros endpoints (listar, iniciar, finalizar preparação) serão criados em stories futuras.
