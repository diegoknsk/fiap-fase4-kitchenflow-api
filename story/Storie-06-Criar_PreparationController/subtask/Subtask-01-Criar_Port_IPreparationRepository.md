# Subtask 01: Criar Port IPreparationRepository

## Descrição
Criar a interface `IPreparationRepository` (Port) na camada Application que define o contrato para acesso a dados de Preparation. Esta interface será implementada pela camada Infra.Persistence.

## Passos de implementação
- Criar pasta `Ports/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar interface `IPreparationRepository` em `Ports/IPreparationRepository.cs`:
  - Método `Task<Guid> CreateAsync(Preparation preparation)`:
    - Recebe entidade de domínio `Preparation`
    - Retorna `Guid` (Id da preparação criada)
  - Método `Task<Preparation?> GetByOrderIdAsync(Guid orderId)`:
    - Busca preparação por OrderId
    - Retorna `Preparation?` (nullable, pode não existir)
    - Usado para verificar idempotência
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
- Interface `IPreparationRepository` criada em `Ports/IPreparationRepository.cs`
- Método `CreateAsync(Preparation)` definido:
  - Recebe entidade de domínio `Preparation`
  - Retorna `Task<Guid>`
- Método `GetByOrderIdAsync(Guid orderId)` definido:
  - Recebe `Guid orderId`
  - Retorna `Task<Preparation?>`
- Projeto Application compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Application.Ports`
- Interface segue padrão do OrderHub/Auth
