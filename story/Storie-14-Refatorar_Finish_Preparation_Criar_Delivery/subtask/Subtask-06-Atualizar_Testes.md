# Subtask 06: Atualizar testes do FinishPreparationUseCase

## Descrição
Atualizar os testes do `FinishPreparationUseCase` para validar a criação automática de delivery e o retorno do DeliveryId.

## Passos de implementação
- Abrir classe de testes `FinishPreparationUseCaseTests` em `Tests.Unit/Application/UseCases/PreparationManagement/FinishPreparationUseCaseTests.cs`
- Adicionar mock de `IDeliveryRepository`:
  - Criar `Mock<IDeliveryRepository>` no setup
  - Configurar mock no construtor do UseCase
- Atualizar teste existente de sucesso:
  - Adicionar setup do mock para `GetByPreparationIdAsync` retornar null (não existe delivery)
  - Adicionar setup do mock para `CreateAsync` retornar um Guid
  - Validar que `DeliveryId` está presente no response
  - Validar que `CreateAsync` foi chamado uma vez
- Adicionar novo teste para idempotência:
  - Cenário: Delivery já existe para a preparation
  - Setup: `GetByPreparationIdAsync` retorna um delivery existente
  - Validar que `CreateAsync` NÃO foi chamado
  - Validar que `DeliveryId` retornado é o do delivery existente
- Adicionar novo teste para validação de criação:
  - Validar que o delivery é criado com `PreparationId` correto
  - Validar que o delivery é criado com `OrderId` correto (ou null)
  - Validar que o delivery é criado com status `ReadyForPickup`
- Verificar que testes existentes ainda passam:
  - Testes de validação de InputModel
  - Testes de Preparation não encontrada
  - Testes de status inválido
- Atualizar testes do Controller (se necessário):
  - Verificar que o response contém `DeliveryId`
  - Remover testes do endpoint `CreateDelivery` (se existirem)

## Referências
- **Testes atuais**: Verificar estrutura existente
- **Padrão**: Seguir padrão dos outros testes do projeto
- **CreateDeliveryUseCaseTests**: Verificar como são testados os mocks de repository
- **Moq**: Usar biblioteca Moq para mocks

## Observações importantes
- **Cobertura**: Garantir que todos os cenários são cobertos:
  - Criação de novo delivery
  - Idempotência (delivery já existe)
  - Validação de dados do delivery criado
- **Mocks**: Configurar mocks corretamente para não quebrar outros testes
- **Assertions**: Validar tanto o comportamento (chamadas de métodos) quanto o resultado (DeliveryId no response)
- **Isolamento**: Garantir que os testes são independentes e não afetam outros testes
