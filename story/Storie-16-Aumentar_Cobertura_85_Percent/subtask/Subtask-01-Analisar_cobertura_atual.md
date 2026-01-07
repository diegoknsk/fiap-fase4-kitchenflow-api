# Subtask 01: Analisar cobertura atual no SonarCloud

## Descrição
Analisar o relatório de cobertura atual no SonarCloud para identificar áreas com baixa cobertura e priorizar onde adicionar novos testes.

## Passos de implementação
- Acessar SonarCloud: https://sonarcloud.io
- Navegar até o projeto `diegoknsk_fiap-fase4-kitchenflow-api`
- Acessar aba "Measures" → "Coverage"
- Analisar cobertura por:
  - **Diretório**: Identificar camadas com baixa cobertura
  - **Arquivo**: Identificar arquivos com baixa cobertura
  - **Classe**: Identificar classes com baixa cobertura
  - **Método**: Identificar métodos com 0% de cobertura
- Exportar relatório de cobertura (se disponível)
- Documentar áreas identificadas:
  - Classes com 0% de cobertura
  - Classes com cobertura < 50%
  - Métodos não cobertos
  - UseCases com baixa cobertura
  - Controllers com baixa cobertura
  - Repositories com baixa cobertura

## Análise Esperada

### Cobertura Atual
- **Total**: 48.11%
- **Sequence points**: 3,573 total, 1,719 visitados
- **Gap para meta**: 36.89%

### Áreas a Identificar
1. **UseCases**:
   - Quais UseCases têm cobertura < 90%?
   - Quais métodos de UseCases não estão cobertos?
   - Quais cenários não estão cobertos?

2. **Controllers**:
   - Quais Controllers têm cobertura < 85%?
   - Quais endpoints não estão cobertos?
   - Quais cenários de erro não estão cobertos?

3. **Repositories**:
   - Quais Repositories têm cobertura < 80%?
   - Quais métodos não estão cobertos?

4. **Domain**:
   - Quais entidades têm cobertura < 70%?
   - Quais métodos não estão cobertos?

## Documentação Esperada

### Lista de Classes com Baixa Cobertura
```
- [Classe] - [Cobertura atual]% - [Prioridade]
  - Métodos não cobertos:
    - [Método 1]
    - [Método 2]
  - Cenários não cobertos:
    - [Cenário 1]
    - [Cenário 2]
```

### Priorização
1. **Alta**: UseCases com cobertura < 50%
2. **Média**: Controllers com cobertura < 50%
3. **Baixa**: Repositories e Domain com cobertura < 50%

## Critérios de aceite
- [ ] SonarCloud acessado e relatório de cobertura visualizado
- [ ] Cobertura atual documentada (48.11%)
- [ ] Lista de classes com 0% de cobertura criada
- [ ] Lista de classes com cobertura < 50% criada
- [ ] Lista de métodos não cobertos criada
- [ ] Priorização de áreas para aumentar cobertura definida
- [ ] Documentação criada com análise completa

## Observações importantes
- **Foco em áreas críticas**: Priorizar UseCases e Controllers
- **Métricas do SonarCloud**: Usar métricas do SonarCloud como fonte única de verdade
- **Análise detalhada**: Identificar não apenas classes, mas métodos específicos não cobertos
- **Cenários não cobertos**: Identificar cenários de teste que estão faltando
