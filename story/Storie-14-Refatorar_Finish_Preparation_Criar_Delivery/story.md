# Storie-14: Refatorar Finish Preparation e Criação Automática de Delivery

## Descrição
Como desenvolvedor, quero refatorar o finish de preparação para que o delivery seja criado automaticamente quando uma preparação é finalizada, seguindo o padrão do projeto antigo. O finish deve retornar o ID do delivery criado para que possamos finalizá-lo posteriormente. O endpoint de criação manual de delivery deve ser removido, pois o delivery só será criado automaticamente no finish da preparação.

## Objetivo
Refatorar o fluxo de finalização de preparação para:
- Criar automaticamente o delivery quando a preparação é finalizada
- Retornar o DeliveryId no response do finish
- Remover o endpoint `POST /api/deliveries` (criação manual não é mais necessária)
- Manter os endpoints `GET /api/deliveries/ready` e `POST /api/deliveries/{id}/finalize`

## Escopo Técnico
- **Tecnologias**: .NET 8, ASP.NET Core, Entity Framework Core
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Application` (UseCase, OutputModel, Response, Presenter)
  - `FastFood.KitchenFlow.Api` (Controller - remover endpoint)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\01-InterfacesExternas\FastFood.Api\Controllers\PreparationController.cs`
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\UseCases\PreparationManagement\FinishPreparationUseCase.cs`

## Mudanças Necessárias

### 1. Refatorar FinishPreparationUseCase
- Adicionar `IDeliveryRepository` como dependência
- Após finalizar a preparação, criar automaticamente o delivery
- Verificar idempotência (não criar delivery duplicado se já existir)
- Retornar o DeliveryId no OutputModel

### 2. Atualizar FinishPreparationOutputModel
- Adicionar propriedade `DeliveryId` (Guid?)

### 3. Atualizar FinishPreparationResponse
- Adicionar propriedade `DeliveryId` (Guid?)

### 4. Atualizar FinishPreparationPresenter
- Mapear `DeliveryId` do OutputModel para o Response

### 5. Remover endpoint POST /api/deliveries
- Remover método `CreateDelivery` do `DeliveryController`
- Remover `CreateDeliveryUseCase` do construtor do `DeliveryController`
- Manter apenas `GetReadyDeliveriesUseCase` e `FinalizeDeliveryUseCase`

### 6. Atualizar Dependency Injection
- Remover registro de `CreateDeliveryUseCase` do `Program.cs` (se não for usado em outros lugares)
- Manter `CreateDeliveryUseCase` registrado caso seja usado internamente pelo `FinishPreparationUseCase`

## Endpoints Afetados

### POST /api/preparations/{id}/finish
**Mudança**: Agora retorna o DeliveryId no response.

**Response 200 OK (ANTES)**:
```json
{
  "success": true,
  "message": "Preparação finalizada com sucesso.",
  "content": {
    "id": "guid",
    "orderId": "guid",
    "status": 2,
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

**Response 200 OK (DEPOIS)**:
```json
{
  "success": true,
  "message": "Preparação finalizada com sucesso.",
  "content": {
    "id": "guid",
    "orderId": "guid",
    "status": 2,
    "createdAt": "2024-01-01T00:00:00Z",
    "deliveryId": "guid"
  }
}
```

### POST /api/deliveries
**Mudança**: Endpoint removido. O delivery agora é criado automaticamente no finish da preparação.

### GET /api/deliveries/ready
**Mudança**: Nenhuma. Continua retornando deliveries com `ReadyForPickup = 1`.

### POST /api/deliveries/{id}/finalize
**Mudança**: Nenhuma. Continua funcionando normalmente.

## Fluxo de Negócio Atualizado

### Finalização de Preparação
1. Preparação está com status `InProgress`
2. Sistema finaliza a preparação (status → `Finished`)
3. **NOVO**: Sistema cria automaticamente um Delivery associado à Preparation
4. **NOVO**: Sistema retorna o DeliveryId no response
5. Delivery é criado com status `ReadyForPickup`
6. Delivery pode ser listado via `GET /api/deliveries/ready`
7. Delivery pode ser finalizado via `POST /api/deliveries/{id}/finalize`

### Idempotência
- Se já existir um Delivery para a Preparation, não criar duplicado
- Retornar o DeliveryId existente no response

## Subtasks

- [ ] [Subtask 01: Refatorar FinishPreparationUseCase](./subtask/Subtask-01-Refatorar_FinishPreparationUseCase.md)
- [ ] [Subtask 02: Atualizar FinishPreparationOutputModel](./subtask/Subtask-02-Atualizar_OutputModel.md)
- [ ] [Subtask 03: Atualizar FinishPreparationResponse](./subtask/Subtask-03-Atualizar_Response.md)
- [ ] [Subtask 04: Atualizar FinishPreparationPresenter](./subtask/Subtask-04-Atualizar_Presenter.md)
- [ ] [Subtask 05: Remover endpoint CreateDelivery do DeliveryController](./subtask/Subtask-05-Remover_Endpoint_CreateDelivery.md)
- [ ] [Subtask 06: Atualizar testes do FinishPreparationUseCase](./subtask/Subtask-06-Atualizar_Testes.md)
- [ ] [Subtask 07: Validar fluxo completo](./subtask/Subtask-07-Validar_Fluxo.md)

## Critérios de Aceite da História

- [ ] `FinishPreparationUseCase` refatorado:
  - Recebe `IDeliveryRepository` como dependência
  - Cria delivery automaticamente após finalizar preparação
  - Verifica idempotência (não cria duplicado)
  - Retorna `DeliveryId` no OutputModel
- [ ] `FinishPreparationOutputModel` atualizado:
  - Propriedade `DeliveryId` (Guid?) adicionada
- [ ] `FinishPreparationResponse` atualizado:
  - Propriedade `DeliveryId` (Guid?) adicionada
- [ ] `FinishPreparationPresenter` atualizado:
  - Mapeia `DeliveryId` do OutputModel para Response
- [ ] `DeliveryController` atualizado:
  - Endpoint `POST /api/deliveries` removido
  - `CreateDeliveryUseCase` removido do construtor
  - Endpoints `GET /api/deliveries/ready` e `POST /api/deliveries/{id}/finalize` mantidos
- [ ] Testes atualizados:
  - Testes do `FinishPreparationUseCase` validam criação automática de delivery
  - Testes validam retorno do `DeliveryId`
  - Testes validam idempotência
- [ ] Fluxo completo validado:
  - Finish de preparação cria delivery automaticamente
  - Response contém DeliveryId
  - Delivery aparece em `GET /api/deliveries/ready`
  - Delivery pode ser finalizado via `POST /api/deliveries/{id}/finalize`

## Observações Arquiteturais

### Clean Architecture
- **UseCase**: Contém lógica de negócio completa (finalizar preparação + criar delivery)
- **Repository**: Acesso a dados de Preparation e Delivery
- **Presenter**: Transforma OutputModel em Response

### Regras de Negócio

#### Criação Automática de Delivery
- **Trigger**: Finalização de preparação (status → Finished)
- **Idempotência**: Verificar se já existe Delivery antes de criar
- **Status inicial**: Delivery criado com status `ReadyForPickup`
- **Relacionamento**: Delivery sempre associado a uma Preparation

#### Response do Finish
- **DeliveryId**: Sempre retornado quando delivery é criado ou já existe
- **Valor nulo**: Não deve ocorrer, pois delivery é criado automaticamente

### Análise do Projeto Antigo

**Monolito (FinishPreparationUseCase):**
- Linha 55-56: Cria delivery automaticamente após finalizar preparação
- Usa `DeliveryGateway.AddAsync(delivery)`
- Delivery criado com `order.Id`

**Aplicação no KitchenFlow:**
- Adaptar para usar `IDeliveryRepository`
- Usar `PreparationId` em vez de `OrderId` (seguindo novo modelo)
- Manter idempotência
- Retornar `DeliveryId` no response

### Integração com Delivery
- Delivery é criado automaticamente no finish da preparação
- Não há mais necessidade de endpoint manual de criação
- Endpoints de listagem e finalização permanecem inalterados

---

## ✅ Story Concluída

**Data de Conclusão**: [A preencher após implementação]

### Resumo da Implementação

[Será preenchido após conclusão]
