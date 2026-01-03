# Storie-07: Criar DeliveryController (Endpoints de Entrega)

## Descrição
Como desenvolvedor, quero criar o `DeliveryController` com endpoints para gerenciar entregas, incluindo criação de delivery quando uma preparação é finalizada, listagem de deliveries prontas para retirada e finalização de entregas. O controller deve seguir o padrão Clean Architecture e integrar com a entidade Preparation.

## Objetivo
Criar o `DeliveryController` e toda a camada Application necessária (UseCases, InputModels, OutputModels, Presenters, Port) para suportar os seguintes endpoints:
- `POST /api/deliveries` - Criar nova entrega quando preparação for finalizada
- `GET /api/deliveries/ready` - Listar entregas prontas para retirada (paginação)
- `POST /api/deliveries/{id}/finalize` - Finalizar uma entrega

## Escopo Técnico
- **Tecnologias**: .NET 8, ASP.NET Core, Entity Framework Core
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Api` (Controller)
  - `FastFood.KitchenFlow.Application` (UseCases, InputModels, OutputModels, Presenters, Port)
  - `FastFood.KitchenFlow.Infra.Persistence` (Repository implementando Port)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de controllers e use cases)
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda` (padrão de Application layer)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\01-InterfacesExternas\FastFood.Api\Controllers\DeliveryController.cs`
  - Adaptar para o novo modelo com PreparationId

## Endpoints

### POST /api/deliveries
**Descrição**: Criar nova entrega quando uma preparação for finalizada.

**Request Body**:
```json
{
  "preparationId": "guid",
  "orderId": "guid" // opcional
}
```

**Response 201 Created**:
```json
{
  "id": "guid",
  "preparationId": "guid",
  "orderId": "guid",
  "status": 1,
  "createdAt": "2024-01-01T00:00:00Z",
  "message": "Entrega criada com sucesso"
}
```

**Response 400 Bad Request**: Dados inválidos ou Preparation não encontrada
**Response 409 Conflict**: Delivery já existe para esta Preparation

### GET /api/deliveries/ready
**Descrição**: Listar entregas prontas para retirada (status ReadyForPickup) com paginação.

**Query Parameters**:
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)

**Response 200 OK**:
```json
{
  "items": [
    {
      "id": "guid",
      "preparationId": "guid",
      "orderId": "guid",
      "status": 1,
      "createdAt": "2024-01-01T00:00:00Z"
    }
  ],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### POST /api/deliveries/{id}/finalize
**Descrição**: Finalizar uma entrega (ReadyForPickup → Finalized).

**Response 200 OK**:
```json
{
  "id": "guid",
  "preparationId": "guid",
  "orderId": "guid",
  "status": 2,
  "createdAt": "2024-01-01T00:00:00Z",
  "finalizedAt": "2024-01-01T01:00:00Z",
  "message": "Entrega finalizada com sucesso"
}
```

**Response 400 Bad Request**: Delivery não encontrada ou status inválido
**Response 404 Not Found**: Delivery não encontrada

## Fluxo de Negócio

### Criação de Delivery
1. Preparation é finalizada (status Finished)
2. Sistema (ou endpoint) cria Delivery associada à Preparation
3. Delivery é criada com status `ReadyForPickup`
4. Delivery pode ser listada e finalizada

### Finalização de Delivery
1. Delivery está com status `ReadyForPickup`
2. Cliente retira o pedido
3. Sistema finaliza a Delivery (status → `Finalized`)
4. `FinalizedAt` é definido

## Subtasks

- [ ] [Subtask 01: Criar Port IDeliveryRepository](./subtask/Subtask-01-Criar_Port_IDeliveryRepository.md)
- [ ] [Subtask 02: Criar Repository implementando Port](./subtask/Subtask-02-Criar_Repository.md)
- [ ] [Subtask 03: Criar InputModels, OutputModels e Responses](./subtask/Subtask-03-Criar_Models.md)
- [ ] [Subtask 04: Criar UseCase CreateDeliveryUseCase](./subtask/Subtask-04-Criar_UseCase_Create.md)
- [ ] [Subtask 05: Criar UseCase GetReadyDeliveriesUseCase](./subtask/Subtask-05-Criar_UseCase_GetReady.md)
- [ ] [Subtask 06: Criar UseCase FinalizeDeliveryUseCase](./subtask/Subtask-06-Criar_UseCase_Finalize.md)
- [ ] [Subtask 07: Criar Presenters](./subtask/Subtask-07-Criar_Presenters.md)
- [ ] [Subtask 08: Criar DeliveryController](./subtask/Subtask-08-Criar_Controller.md)
- [ ] [Subtask 09: Configurar Dependency Injection](./subtask/Subtask-09-Configurar_DI.md)
- [ ] [Subtask 10: Validar endpoints completos](./subtask/Subtask-10-Validar_endpoints.md)

## Critérios de Aceite da História

- [ ] Port `IDeliveryRepository` criado:
  - `CreateAsync(Delivery)` - criar delivery
  - `GetByPreparationIdAsync(Guid preparationId)` - buscar por PreparationId
  - `GetReadyDeliveriesAsync(int pageNumber, int pageSize)` - listar prontas
  - `GetByIdAsync(Guid id)` - buscar por Id
  - `UpdateAsync(Delivery)` - atualizar delivery
- [ ] Repository `DeliveryRepository` criado:
  - Implementa `IDeliveryRepository`
  - Usa `KitchenFlowDbContext`
  - Mapeia entre domínio e persistência
- [ ] UseCases criados:
  - `CreateDeliveryUseCase` - cria delivery
  - `GetReadyDeliveriesUseCase` - lista deliveries prontas
  - `FinalizeDeliveryUseCase` - finaliza delivery
- [ ] Controller `DeliveryController` criado:
  - `POST /api/deliveries` - criar
  - `GET /api/deliveries/ready` - listar prontas
  - `POST /api/deliveries/{id}/finalize` - finalizar
- [ ] Validações implementadas:
  - PreparationId obrigatório e deve existir
  - Delivery não pode ser criada se Preparation não está Finished
  - Delivery só pode ser finalizada se status é ReadyForPickup
- [ ] Paginação implementada para listagem
- [ ] Dependency Injection configurado
- [ ] Endpoints testados e funcionando
- [ ] Swagger documenta os endpoints
- [ ] Código segue padrão do OrderHub/Auth

## Observações Arquiteturais

### Clean Architecture
- **Controller**: Apenas recebe request, valida, mapeia e chama UseCase
- **UseCase**: Contém lógica de negócio, validações, chamadas a Ports
- **Port**: Interface na Application, define contrato
- **Repository**: Implementação na Infra.Persistence, acessa DbContext
- **Presenter**: Transforma OutputModel em Response (chamado pelo UseCase)

### Regras de Negócio

#### Criação de Delivery
- **PreparationId obrigatório**: Delivery sempre depende de uma Preparation
- **Preparation deve estar Finished**: Só criar Delivery se Preparation está com status Finished
- **Idempotência**: Verificar se já existe Delivery para a Preparation antes de criar
- **OrderId opcional**: Apenas para facilitar consultas

#### Finalização de Delivery
- **Status válido**: Só pode finalizar se status é `ReadyForPickup`
- **FinalizedAt**: Deve ser definido ao finalizar
- **Transição**: `ReadyForPickup` → `Finalized`

### Relacionamento Preparation → Delivery
- Uma Preparation pode ter zero ou uma Delivery
- Delivery NÃO existe sem Preparation
- O vínculo principal é `Delivery.PreparationId`
- Delivery é criada quando Preparation é finalizada

### Análise dos Projetos de Referência

**Monolito (DeliveryController):**
- Endpoints: `GET /api/delivery` (listar prontas) e `POST /api/delivery/finalize` (finalizar)
- Usa `DeliveryControllerOrchestrator` (padrão antigo)
- Adaptar para novo padrão com UseCases e PreparationId

**Aplicação no KitchenFlow:**
- Seguir padrão do OrderHub (Clean Architecture)
- Criar UseCases focados (um por operação)
- Implementar validações de negócio
- Suportar relacionamento Preparation → Delivery

### Integração com Preparation
- Delivery é criada quando Preparation é finalizada
- Pode ser criada automaticamente (quando Preparation é finalizada) ou via endpoint
- Endpoint de criação permite controle manual se necessário
