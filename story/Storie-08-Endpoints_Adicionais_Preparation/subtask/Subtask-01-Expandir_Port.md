# Subtask 01: Expandir Port IPreparationRepository

## Descrição
Expandir a interface `IPreparationRepository` (criada na Story 06) com métodos adicionais necessários para listar preparações e atualizar preparações.

## Passos de implementação
- Abrir interface `IPreparationRepository` em `Application/Ports/IPreparationRepository.cs`
- Adicionar métodos:
  - Método `Task<(IEnumerable<Preparation>, int)> GetPagedAsync(int pageNumber, int pageSize, int? status)`:
    - Busca preparações com paginação
    - Filtro opcional por status (nullable)
    - Retorna tupla com lista de preparações e total de registros
  - Método `Task<Preparation?> GetByIdAsync(Guid id)`:
    - Busca preparação por Id
    - Retorna `Preparation?` (nullable)
    - Usado para buscar preparação antes de iniciar/finalizar
    - Nota: Se já existir (da Story 07), manter; caso contrário, adicionar
  - Método `Task UpdateAsync(Preparation preparation)`:
    - Atualiza preparação existente
    - Retorna `Task`
    - Usado após alterar status (Start/Finish)
    - Nota: Se já existir, manter; caso contrário, adicionar
- Adicionar documentação XML nos novos métodos
- Verificar que métodos existentes não foram alterados

## Referências
- **OrderHub**: Verificar padrão de Ports com paginação
- **Auth**: Verificar padrão de Ports
- Seguir convenção de nomenclatura existente

## Como testar
- Executar `dotnet build` no projeto Application para verificar compilação
- Verificar que interface pode ser referenciada

## Critérios de aceite
- Interface `IPreparationRepository` expandida com:
  - `GetPagedAsync(int, int, int?)` - retorna `Task<(IEnumerable<Preparation>, int)>`
  - `GetByIdAsync(Guid)` - retorna `Task<Preparation?>`
  - `UpdateAsync(Preparation)` - retorna `Task`
- Métodos existentes (`CreateAsync`, `GetByOrderIdAsync`) mantidos
- Projeto Application compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Application.Ports`
- Interface segue padrão do OrderHub/Auth
