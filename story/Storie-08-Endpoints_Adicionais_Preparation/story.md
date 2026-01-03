# Storie-08: Criar Endpoints Adicionais de Preparation (Listar, Iniciar, Finalizar)

## Descrição
Como desenvolvedor, quero criar endpoints adicionais no `PreparationController` para listar preparações, iniciar uma preparação (Received → InProgress) e finalizar uma preparação (InProgress → Finished), completando o fluxo de gerenciamento de preparações na cozinha.

## Objetivo
Expandir o `PreparationController` (criado na Story 06) com endpoints adicionais para gerenciar o ciclo de vida das preparações:
- `GET /api/preparations` - Listar preparações com paginação e filtros opcionais
- `POST /api/preparations/{id}/start` - Iniciar uma preparação (Received → InProgress)
- `POST /api/preparations/{id}/finish` - Finalizar uma preparação (InProgress → Finished)

Estes endpoints complementam o endpoint de criação (`POST /api/preparations`) criado na Story 06.

## Escopo Técnico
- **Tecnologias**: .NET 8, ASP.NET Core, Entity Framework Core
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Api` (Controller - expandir)
  - `FastFood.KitchenFlow.Application` (UseCases, InputModels, OutputModels, Presenters - novos)
  - `FastFood.KitchenFlow.Infra.Persistence` (Repository - expandir métodos se necessário)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de controllers e use cases)
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda` (padrão de Application layer)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\01-InterfacesExternas\FastFood.Api\Controllers\PreparationController.cs`
  - Adaptar endpoints `GET`, `POST take-next`, `POST finish` para novo padrão

## Endpoints

### GET /api/preparations
**Descrição**: Listar preparações com paginação e filtros opcionais por status.

**Query Parameters**:
- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10)
- `status` (int?, opcional) - Filtrar por status (0=Received, 1=InProgress, 2=Finished)

**Response 200 OK**:
```json
{
  "items": [
    {
      "id": "guid",
      "orderId": "guid",
      "status": 0,
      "createdAt": "2024-01-01T00:00:00Z",
      "orderSnapshot": "{ ... json ... }"
    }
  ],
  "totalCount": 10,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### POST /api/preparations/{id}/start
**Descrição**: Iniciar uma preparação (Received → InProgress).

**Response 200 OK**:
```json
{
  "id": "guid",
  "orderId": "guid",
  "status": 1,
  "createdAt": "2024-01-01T00:00:00Z",
  "message": "Preparação iniciada com sucesso"
}
```

**Response 400 Bad Request**: Preparation não encontrada ou status inválido
**Response 404 Not Found**: Preparation não encontrada

### POST /api/preparations/{id}/finish
**Descrição**: Finalizar uma preparação (InProgress → Finished). Quando uma preparação é finalizada, pode-se criar uma Delivery associada.

**Response 200 OK**:
```json
{
  "id": "guid",
  "orderId": "guid",
  "status": 2,
  "createdAt": "2024-01-01T00:00:00Z",
  "message": "Preparação finalizada com sucesso"
}
```

**Response 400 Bad Request**: Preparation não encontrada ou status inválido
**Response 404 Not Found**: Preparation não encontrada

## Fluxo de Negócio

### Listagem de Preparations
1. Sistema lista preparações com paginação
2. Filtro opcional por status (Received, InProgress, Finished)
3. Retorna lista paginada com OrderSnapshot

### Início de Preparação
1. Preparation está com status `Received`
2. Cozinheiro inicia a preparação
3. Sistema altera status para `InProgress`
4. Preparation fica disponível para finalização

### Finalização de Preparação
1. Preparation está com status `InProgress`
2. Cozinheiro finaliza a preparação
3. Sistema altera status para `Finished`
4. Preparation fica pronta para criação de Delivery (pode ser criada automaticamente ou via endpoint)

## Subtasks

- [ ] [Subtask 01: Expandir Port IPreparationRepository](./subtask/Subtask-01-Expandir_Port.md)
- [ ] [Subtask 02: Expandir Repository com novos métodos](./subtask/Subtask-02-Expandir_Repository.md)
- [ ] [Subtask 03: Criar InputModels, OutputModels e Responses](./subtask/Subtask-03-Criar_Models.md)
- [ ] [Subtask 04: Criar UseCase GetPreparationsUseCase](./subtask/Subtask-04-Criar_UseCase_Get.md)
- [ ] [Subtask 05: Criar UseCase StartPreparationUseCase](./subtask/Subtask-05-Criar_UseCase_Start.md)
- [ ] [Subtask 06: Criar UseCase FinishPreparationUseCase](./subtask/Subtask-06-Criar_UseCase_Finish.md)
- [ ] [Subtask 07: Criar Presenters](./subtask/Subtask-07-Criar_Presenters.md)
- [ ] [Subtask 08: Expandir PreparationController](./subtask/Subtask-08-Expandir_Controller.md)
- [ ] [Subtask 09: Configurar Dependency Injection](./subtask/Subtask-09-Configurar_DI.md)
- [ ] [Subtask 10: Validar endpoints completos](./subtask/Subtask-10-Validar_endpoints.md)

## Critérios de Aceite da História

- [ ] Port `IPreparationRepository` expandido:
  - `GetPagedAsync(int pageNumber, int pageSize, int? status)` - listar com paginação e filtro
  - `GetByIdAsync(Guid id)` - buscar por Id (se ainda não existir)
  - `UpdateAsync(Preparation)` - atualizar preparation (se ainda não existir)
- [ ] Repository `PreparationRepository` expandido:
  - Implementa novos métodos do Port
  - Suporta paginação e filtro por status
- [ ] UseCases criados:
  - `GetPreparationsUseCase` - lista preparações
  - `StartPreparationUseCase` - inicia preparação
  - `FinishPreparationUseCase` - finaliza preparação
- [ ] Controller `PreparationController` expandido:
  - `GET /api/preparations` - listar
  - `POST /api/preparations/{id}/start` - iniciar
  - `POST /api/preparations/{id}/finish` - finalizar
- [ ] Validações implementadas:
  - Só pode iniciar se status é `Received`
  - Só pode finalizar se status é `InProgress`
  - Transições de status validadas
- [ ] Paginação implementada para listagem
- [ ] Filtro por status implementado
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

#### Início de Preparação
- **Status válido**: Só pode iniciar se status é `Received`
- **Transição**: `Received` → `InProgress`
- **Validação**: Método de domínio `StartPreparation()` já valida a transição

#### Finalização de Preparação
- **Status válido**: Só pode finalizar se status é `InProgress`
- **Transição**: `InProgress` → `Finished`
- **Validação**: Método de domínio `FinishPreparation()` já valida a transição
- **Delivery**: Após finalizar, pode-se criar Delivery (não automático nesta story)

#### Listagem
- **Paginação**: Suporta paginação (pageNumber, pageSize)
- **Filtro**: Filtro opcional por status
- **OrderSnapshot**: Incluído na resposta para exibição no display da cozinha

### Transições de Status
- **Received (0)**: Preparação criada, aguardando início
- **InProgress (1)**: Preparação em andamento
- **Finished (2)**: Preparação finalizada, pronta para entrega

### Análise dos Projetos de Referência

**Monolito (PreparationController):**
- `GET /api/preparations` - lista preparações (paginação)
- `POST /api/preparations/take-next` - pega próximo pedido (funcionalidade avançada, não incluída nesta story)
- `POST /api/preparations/finish` - finaliza preparação
- Adaptar para novo padrão com UseCases e validações de domínio

**Aplicação no KitchenFlow:**
- Seguir padrão do OrderHub (Clean Architecture)
- Criar UseCases focados (um por operação)
- Implementar validações de negócio usando métodos de domínio
- Suportar paginação e filtros

### Integração com Delivery
- Quando Preparation é finalizada (Finished), pode-se criar Delivery
- Criação de Delivery pode ser feita automaticamente (futuro) ou via endpoint (Story 07)
- Esta story foca apenas no ciclo de vida da Preparation
