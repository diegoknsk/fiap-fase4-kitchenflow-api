# Subtask 02: Criar Repository implementando Port

## Descrição
Criar a implementação concreta do `IDeliveryRepository` na camada `Infra.Persistence`, que usa o `KitchenFlowDbContext` para persistir dados e faz o mapeamento entre entidades de domínio e entidades de persistência.

## Passos de implementação
- Criar classe `DeliveryRepository` em `Repositories/DeliveryRepository.cs`:
  - Implementar `IDeliveryRepository`
  - Construtor recebe `KitchenFlowDbContext`
  - Implementar `CreateAsync(Delivery delivery)`:
    - Mapear entidade de domínio `Delivery` para `DeliveryEntity`
    - Adicionar ao DbContext
    - Salvar alterações (`SaveChangesAsync`)
    - Retornar `Id` da delivery criada
  - Implementar `GetByPreparationIdAsync(Guid preparationId)`:
    - Buscar `DeliveryEntity` por `PreparationId` no banco
    - Se encontrada, mapear para entidade de domínio `Delivery`
    - Retornar `Delivery?` (null se não encontrada)
  - Implementar `GetReadyDeliveriesAsync(int pageNumber, int pageSize)`:
    - Buscar `DeliveryEntity` com status `ReadyForPickup` (EnumDeliveryStatus.ReadyForPickup = 1)
    - Aplicar paginação (Skip/Take)
    - Contar total de registros
    - Mapear para entidades de domínio
    - Retornar tupla `(IEnumerable<Delivery>, int)`
  - Implementar `GetByIdAsync(Guid id)`:
    - Buscar `DeliveryEntity` por `Id`
    - Mapear para entidade de domínio
    - Retornar `Delivery?` (null se não encontrada)
  - Implementar `UpdateAsync(Delivery delivery)`:
    - Buscar `DeliveryEntity` existente
    - Atualizar propriedades
    - Salvar alterações
- Criar métodos auxiliares de mapeamento:
  - `DeliveryEntity ToEntity(Delivery domain)` - domínio → persistência
  - `Delivery ToDomain(DeliveryEntity entity)` - persistência → domínio
- Adicionar documentação XML na classe
- Namespace: `FastFood.KitchenFlow.Infra.Persistence.Repositories`

## Referências
- **Auth**: Verificar implementação de `CustomerRepository`
- **OrderHub**: Verificar implementação de DataSources
- Seguir padrão de mapeamento entre domínio e persistência

## Observações importantes
- **Mapeamento**: Entidades de domínio têm métodos de negócio, entidades de persistência são simples POCOs
- **Status**: Mapear `EnumDeliveryStatus` (domínio) para `int` (persistência)
- **Paginação**: Usar `Skip((pageNumber - 1) * pageSize).Take(pageSize)` para paginação
- **Contagem**: Usar `CountAsync()` para contar total de registros

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que repository pode ser instanciado
- (Opcional) Criar teste unitário básico

## Critérios de aceite
- Classe `DeliveryRepository` criada em `Repositories/DeliveryRepository.cs`
- Implementa `IDeliveryRepository`
- Construtor recebe `KitchenFlowDbContext`
- Todos os métodos implementados:
  - `CreateAsync` - mapeia, adiciona, salva, retorna Id
  - `GetByPreparationIdAsync` - busca, mapeia, retorna
  - `GetReadyDeliveriesAsync` - busca com filtro, pagina, conta, mapeia
  - `GetByIdAsync` - busca, mapeia, retorna
  - `UpdateAsync` - busca, atualiza, salva
- Métodos de mapeamento criados
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence.Repositories`
- Implementação segue padrão do Auth/OrderHub
