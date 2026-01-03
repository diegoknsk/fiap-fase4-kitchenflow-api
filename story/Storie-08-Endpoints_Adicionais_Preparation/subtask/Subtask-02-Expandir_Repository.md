# Subtask 02: Expandir Repository com novos métodos

## Descrição
Expandir a implementação do `PreparationRepository` (criada na Story 06) com os novos métodos do Port para suportar listagem paginada, busca por Id e atualização.

## Passos de implementação
- Abrir classe `PreparationRepository` em `Repositories/PreparationRepository.cs`
- Implementar `GetPagedAsync(int pageNumber, int pageSize, int? status)`:
  - Buscar `PreparationEntity` no banco
  - Aplicar filtro por status se fornecido (status != null)
  - Aplicar paginação (Skip/Take)
  - Contar total de registros (com filtro aplicado)
  - Mapear para entidades de domínio
  - Retornar tupla `(IEnumerable<Preparation>, int)`
- Implementar `GetByIdAsync(Guid id)`:
  - Buscar `PreparationEntity` por `Id`
  - Se encontrada, mapear para entidade de domínio `Preparation`
  - Retornar `Preparation?` (null se não encontrada)
- Implementar `UpdateAsync(Preparation preparation)`:
  - Buscar `PreparationEntity` existente por Id
  - Atualizar propriedades (Status principalmente)
  - Salvar alterações (`SaveChangesAsync`)
- Verificar que métodos existentes não foram alterados
- Adicionar documentação XML nos novos métodos
- Namespace: `FastFood.KitchenFlow.Infra.Persistence.Repositories`

## Referências
- **Auth**: Verificar implementação de métodos similares
- **OrderHub**: Verificar implementação de paginação
- Seguir padrão de mapeamento entre domínio e persistência

## Observações importantes
- **Paginação**: Usar `Skip((pageNumber - 1) * pageSize).Take(pageSize)` para paginação
- **Filtro**: Aplicar filtro por status apenas se `status != null`
- **Contagem**: Usar `CountAsync()` com mesmo filtro para contar total
- **Mapeamento**: Reutilizar métodos de mapeamento existentes

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Verificar que repository pode ser instanciado
- (Opcional) Criar teste unitário básico

## Critérios de aceite
- Classe `PreparationRepository` expandida com:
  - `GetPagedAsync` - busca, filtra, pagina, conta, mapeia
  - `GetByIdAsync` - busca, mapeia, retorna
  - `UpdateAsync` - busca, atualiza, salva
- Métodos existentes (`CreateAsync`, `GetByOrderIdAsync`) mantidos
- Projeto compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Infra.Persistence.Repositories`
- Implementação segue padrão do Auth/OrderHub
