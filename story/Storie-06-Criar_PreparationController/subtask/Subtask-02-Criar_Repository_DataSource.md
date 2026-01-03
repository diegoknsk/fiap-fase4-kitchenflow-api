# Subtask 02: Criar Repository/DataSource implementando Port

## Descrição
Criar a implementação concreta do `IPreparationRepository` na camada `Infra.Persistence`, que usa o `KitchenFlowDbContext` para persistir dados e faz o mapeamento entre entidades de domínio e entidades de persistência.

## Passos de implementação
- Criar pasta `Repositories/` no projeto `FastFood.KitchenFlow.Infra.Persistence` (se não existir)
- Criar classe `PreparationRepository` em `Repositories/PreparationRepository.cs`:
  - Implementar `IPreparationRepository`
  - Construtor recebe `KitchenFlowDbContext`
  - Implementar `CreateAsync(Preparation preparation)`:
    - Mapear entidade de domínio `Preparation` para `PreparationEntity`
    - Adicionar ao DbContext
    - Salvar alterações (`SaveChangesAsync`)
    - Retornar `Id` da preparação criada
  - Implementar `GetByOrderIdAsync(Guid orderId)`:
    - Buscar `PreparationEntity` por `OrderId` no banco
    - Se encontrada, mapear para entidade de domínio `Preparation`
    - Retornar `Preparation?` (null se não encontrada)
- Criar métodos auxiliares de mapeamento:
  - `PreparationEntity ToEntity(Preparation domain)` - domínio → persistência
  - `Preparation ToDomain(PreparationEntity entity)` - persistência → domínio
- Adicionar documentação XML na classe
- Namespace: `FastFood.KitchenFlow.Infra.Persistence.Repositories`

## Referências
- **Auth**: Verificar implementação de `CustomerRepository`
- **OrderHub**: Verificar implementação de DataSources
- Seguir padrão de mapeamento entre domínio e persistência

## Observações importantes
- **Mapeamento**: Entidades de domínio têm métodos de negócio, entidades de persistência são simples POCOs
- **OrderSnapshot**: Mapear `string` (domínio) para `string` (persistência, será jsonb no banco)
- **Status**: Mapear `EnumPreparationStatus` (domínio) para `int` (persistência)

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que repository pode ser instanciado
- (Opcional) Criar teste unitário básico

## Critérios de aceite
- Classe `PreparationRepository` criada em `Repositories/PreparationRepository.cs`
- Implementa `IPreparationRepository`
- Construtor recebe `KitchenFlowDbContext`
- Método `CreateAsync` implementado:
  - Mapeia domínio → persistência
  - Adiciona ao DbContext
  - Salva alterações
  - Retorna Id
- Método `GetByOrderIdAsync` implementado:
  - Busca no banco por OrderId
  - Mapeia persistência → domínio
  - Retorna null se não encontrada
- Métodos de mapeamento criados
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence.Repositories`
- Implementação segue padrão do Auth/OrderHub
