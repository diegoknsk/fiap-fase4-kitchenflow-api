# Subtask 01: Criar Port IDeliveryRepository

## Descrição
Criar a interface `IDeliveryRepository` (Port) na camada Application que define o contrato para acesso a dados de Delivery. Esta interface será implementada pela camada Infra.Persistence.

## Passos de implementação
- Criar pasta `Ports/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar interface `IDeliveryRepository` em `Ports/IDeliveryRepository.cs`:
  - Método `Task<Guid> CreateAsync(Delivery delivery)`:
    - Recebe entidade de domínio `Delivery`
    - Retorna `Guid` (Id da delivery criada)
  - Método `Task<Delivery?> GetByPreparationIdAsync(Guid preparationId)`:
    - Busca delivery por PreparationId
    - Retorna `Delivery?` (nullable, pode não existir)
    - Usado para verificar idempotência
  - Método `Task<(IEnumerable<Delivery>, int)> GetReadyDeliveriesAsync(int pageNumber, int pageSize)`:
    - Busca deliveries com status `ReadyForPickup`
    - Retorna lista de deliveries e total de registros
    - Suporta paginação
  - Método `Task<Delivery?> GetByIdAsync(Guid id)`:
    - Busca delivery por Id
    - Retorna `Delivery?` (nullable)
  - Método `Task UpdateAsync(Delivery delivery)`:
    - Atualiza delivery existente
    - Retorna `Task`
- Adicionar documentação XML na interface
- Namespace: `FastFood.KitchenFlow.Application.Ports`

## Referências
- **OrderHub**: Verificar padrão de Ports (ex: `IOrderDataSource`)
- **Auth**: Verificar padrão de Ports (ex: `ICustomerRepository`)
- Seguir convenção de nomenclatura: `I<Entity>Repository`

## Como testar
- Executar `dotnet build` no projeto Application para verificar compilação
- Verificar que interface pode ser referenciada

## Critérios de aceite
- Interface `IDeliveryRepository` criada em `Ports/IDeliveryRepository.cs`
- Métodos definidos:
  - `CreateAsync(Delivery)` - retorna `Task<Guid>`
  - `GetByPreparationIdAsync(Guid)` - retorna `Task<Delivery?>`
  - `GetReadyDeliveriesAsync(int, int)` - retorna `Task<(IEnumerable<Delivery>, int)>`
  - `GetByIdAsync(Guid)` - retorna `Task<Delivery?>`
  - `UpdateAsync(Delivery)` - retorna `Task`
- Projeto Application compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Application.Ports`
- Interface segue padrão do OrderHub/Auth
