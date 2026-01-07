# Subtask 05: Adicionar testes para Repositories (se necessário)

## Descrição
Criar testes unitários para Repositories se necessário para atingir 85% de cobertura. Focar em métodos de consulta e persistência não cobertos.

## Passos de implementação
- Verificar se cobertura já está >= 85% após UseCases e Controllers
- Se não, revisar lista de Repositories com baixa cobertura (Subtask 02)
- Para cada Repository identificado:
  - Revisar métodos não cobertos
  - Criar testes para métodos faltantes:
    - Métodos de consulta não cobertos
    - Métodos de persistência não cobertos
    - Cenários de erro não cobertos
- Executar testes após cada Repository
- Verificar cobertura após cada Repository
- Documentar cobertura alcançada

## Repositories a Cobrir (se necessário)

### PreparationRepository
- [ ] Métodos de consulta não cobertos:
  - [ ] `GetByOrderIdAsync` - [cenários]
  - [ ] `GetByIdAsync` - [cenários]
  - [ ] `GetAllAsync` - [cenários]
  - [ ] `GetByStatusAsync` - [cenários]
- [ ] Métodos de persistência não cobertos:
  - [ ] `CreateAsync` - [cenários]
  - [ ] `UpdateAsync` - [cenários]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário específico]

### DeliveryRepository
- [ ] Métodos de consulta não cobertos:
  - [ ] `GetByIdAsync` - [cenários]
  - [ ] `GetReadyDeliveriesAsync` - [cenários]
  - [ ] `GetByPreparationIdAsync` - [cenários]
- [ ] Métodos de persistência não cobertos:
  - [ ] `CreateAsync` - [cenários]
  - [ ] `UpdateAsync` - [cenários]
- [ ] Cenários de erro não cobertos:
  - [ ] [Cenário específico]

## Padrão de Testes

### Estrutura AAA (Arrange, Act, Assert)
```csharp
[Fact]
public async Task [Método]_[Cenário]_[ResultadoEsperado]()
{
    // Arrange
    // ... setup ...
    
    // Act
    // ... execução ...
    
    // Assert
    // ... verificações ...
}
```

### Nomenclatura
- `[Método]_[Cenário]_[ResultadoEsperado]`
- Exemplo: `GetByOrderIdAsync_WhenOrderIdExists_ShouldReturnPreparation`

### Verificações
- Verificar resultado retornado
- Verificar chamadas ao DbContext (se aplicável)
- Verificar exceções lançadas (se aplicável)

## Observações sobre Testes de Repositories

### Abordagem
- **Testes de integração**: Repositories geralmente são testados com testes de integração
- **Testes unitários**: Se necessário, mockar DbContext
- **Foco**: Métodos de consulta e persistência

### Prioridade
- **Baixa**: Repositories têm menor impacto na cobertura total
- **Apenas se necessário**: Criar testes apenas se necessário para atingir 85%

## Critérios de aceite
- [ ] Verificação de cobertura >= 85% realizada
- [ ] Se necessário, testes adicionais criados para Repositories
- [ ] Métodos de consulta não cobertos testados
- [ ] Métodos de persistência não cobertos testados
- [ ] Cenários de erro não cobertos testados
- [ ] Todos os testes passam
- [ ] Cobertura de Repositories >= 80% (se testados)
- [ ] Testes seguem padrão AAA
- [ ] Testes seguem nomenclatura descritiva
- [ ] Testes são independentes

## Observações importantes
- **Apenas se necessário**: Criar testes apenas se necessário para atingir 85%
- **Prioridade baixa**: Repositories têm menor impacto na cobertura total
- **Testes de integração**: Considerar testes de integração em vez de unitários
- **Mockar DbContext**: Se testar unitariamente, mockar DbContext
- **Verificar incrementalmente**: Verificar cobertura após cada Repository
