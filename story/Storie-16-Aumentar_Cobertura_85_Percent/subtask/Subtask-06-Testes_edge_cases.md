# Subtask 06: Adicionar testes para edge cases

## Descrição
Criar testes unitários para edge cases que não estão cobertos, focando em valores limite, casos extremos e cenários não comuns.

## Passos de implementação
- Revisar análise de cobertura (Subtask 01 e 02)
- Identificar edge cases não cobertos:
  - Valores limite (mínimos, máximos)
  - Casos extremos
  - Cenários não comuns
  - Valores nulos/vazios
  - Valores inválidos
- Criar testes para edge cases identificados:
  - UseCases: Edge cases de validação e lógica de negócio
  - Controllers: Edge cases de validação de entrada
  - Repositories: Edge cases de consulta (se necessário)
- Executar testes
- Verificar cobertura
- Documentar edge cases cobertos

## Edge Cases por Camada

### UseCases - Edge Cases Comuns
- [ ] Valores nulos:
  - [ ] Input nulo
  - [ ] Propriedades nulas
  - [ ] Dependências nulas
- [ ] Valores vazios:
  - [ ] Strings vazias
  - [ ] Collections vazias
  - [ ] Guids vazios
- [ ] Valores limite:
  - [ ] Valores mínimos
  - [ ] Valores máximos
  - [ ] Valores no limite
- [ ] Casos extremos:
  - [ ] Collections muito grandes
  - [ ] Strings muito longas
  - [ ] Datas extremas

### Controllers - Edge Cases Comuns
- [ ] Request inválido:
  - [ ] Request nulo
  - [ ] Request com campos inválidos
  - [ ] Request com tipos incorretos
- [ ] Parâmetros de rota:
  - [ ] ID inválido (não Guid)
  - [ ] ID vazio
  - [ ] ID inexistente
- [ ] Query parameters:
  - [ ] Parâmetros inválidos
  - [ ] Parâmetros fora do range
  - [ ] Parâmetros com formato incorreto

### Repositories - Edge Cases Comuns (se necessário)
- [ ] Consultas:
  - [ ] Nenhum resultado encontrado
  - [ ] Múltiplos resultados
  - [ ] Resultado único
- [ ] Persistência:
  - [ ] Entidade já existe
  - [ ] Entidade não existe
  - [ ] Conflito de chave única

## Exemplos de Edge Cases

### CreatePreparationUseCase
- [ ] `OrderId` vazio (Guid.Empty)
- [ ] `OrderSnapshot` com JSON inválido
- [ ] `OrderSnapshot` com estrutura incorreta
- [ ] `OrderSnapshot` muito grande
- [ ] `OrderSnapshot` com caracteres especiais

### GetPreparationsUseCase
- [ ] Paginação: página 0
- [ ] Paginação: página negativa
- [ ] Paginação: pageSize muito grande
- [ ] Paginação: pageSize 0
- [ ] Filtro por status: status inválido
- [ ] Filtro por status: status nulo

### StartPreparationUseCase
- [ ] Preparation já iniciada
- [ ] Preparation já finalizada
- [ ] Preparation com status inválido
- [ ] ID de Preparation inexistente

### FinishPreparationUseCase
- [ ] Preparation não iniciada
- [ ] Preparation já finalizada
- [ ] Preparation com status inválido
- [ ] ID de Preparation inexistente

## Padrão de Testes

### Estrutura AAA (Arrange, Act, Assert)
```csharp
[Fact]
public async Task [UseCase]_When[EdgeCase]_Should[ResultadoEsperado]()
{
    // Arrange
    // ... setup com edge case ...
    
    // Act
    // ... execução ...
    
    // Assert
    // ... verificações ...
}
```

### Nomenclatura
- `[UseCase]_When[EdgeCase]_Should[ResultadoEsperado]`
- Exemplo: `CreatePreparation_WhenOrderIdIsEmpty_ShouldThrowArgumentException`

## Critérios de aceite
- [ ] Edge cases identificados e documentados
- [ ] Testes criados para edge cases não cobertos
- [ ] Valores limite testados
- [ ] Casos extremos testados
- [ ] Valores nulos/vazios testados
- [ ] Valores inválidos testados
- [ ] Todos os testes passam
- [ ] Cobertura aumentada com edge cases
- [ ] Testes seguem padrão AAA
- [ ] Testes seguem nomenclatura descritiva
- [ ] Testes são independentes

## Observações importantes
- **Focar em casos reais**: Testar edge cases que podem ocorrer na prática
- **Valores limite**: Testar valores no limite (mínimos, máximos)
- **Valores inválidos**: Testar valores que devem ser rejeitados
- **Valores nulos**: Testar tratamento de valores nulos
- **Casos extremos**: Testar casos extremos que podem causar problemas
