# Storie-16: Aumentar Cobertura de Testes para 85%

## Descrição
Como desenvolvedor, quero aumentar a cobertura de testes do projeto de 48.11% para pelo menos 85%, criando testes unitários adicionais para áreas não cobertas e garantindo que todas as camadas críticas (UseCases, Controllers, Repositories) tenham cobertura adequada.

## Situação Atual
- **Cobertura atual**: 48.11%
- **Meta**: 85%
- **Gap**: 36.89% (precisa aumentar)
- **Total de sequence points**: 3,573
- **Sequence points visitados**: 1,719

## Objetivo
Aumentar a cobertura de testes de 48.11% para 85% através de:
- Análise detalhada da cobertura atual no SonarCloud
- Identificação de áreas com baixa cobertura
- Criação de testes unitários adicionais para UseCases não cobertos
- Criação de testes unitários adicionais para Controllers não cobertos
- Criação de testes unitários para Repositories (se necessário)
- Criação de testes para edge cases e cenários de erro
- Validação de que a cobertura mínima de 85% é atingida no SonarCloud

## Escopo Técnico
- **Tecnologias**: .NET 8, xUnit, Moq, FluentAssertions, Coverlet, SonarCloud
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Tests.Unit` (adicionar novos testes)
  - SonarCloud (monitoramento de cobertura)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de testes)
  - `docs/PROMPT_MICROSERVICOS_TESTES_DEPLOY.md` (lições aprendidas)
- **Meta de cobertura**: >= 85%
- **Monitoramento**: SonarCloud (verificação de threshold local comentada)

## Análise de Cobertura Atual

### Áreas Prioritárias para Aumentar Cobertura
1. **UseCases** (prioridade alta):
   - Verificar cobertura de cada UseCase individualmente
   - Adicionar testes para cenários não cobertos
   - Adicionar testes para edge cases
   - Adicionar testes para validações de negócio

2. **Controllers** (prioridade alta):
   - Verificar cobertura de cada endpoint
   - Adicionar testes para cenários de erro não cobertos
   - Adicionar testes para validações de entrada

3. **Repositories** (prioridade média):
   - Adicionar testes se necessário para atingir 85%
   - Focar em métodos de consulta e persistência

4. **Domain Entities** (prioridade baixa):
   - Adicionar testes apenas se necessário para atingir 85%

## Estratégia de Aumento de Cobertura

### Fase 1: Análise e Identificação (10%)
- [ ] Analisar relatório de cobertura no SonarCloud
- [ ] Identificar classes/métodos com 0% de cobertura
- [ ] Identificar classes/métodos com cobertura < 50%
- [ ] Priorizar áreas críticas (UseCases > Controllers > Repositories)
- [ ] Criar lista de testes necessários

### Fase 2: Testes de UseCases (40%)
- [ ] Revisar testes existentes de UseCases
- [ ] Adicionar testes para cenários não cobertos
- [ ] Adicionar testes para edge cases
- [ ] Adicionar testes para validações de negócio
- [ ] Verificar cobertura após cada UseCase

### Fase 3: Testes de Controllers (30%)
- [ ] Revisar testes existentes de Controllers
- [ ] Adicionar testes para cenários de erro não cobertos
- [ ] Adicionar testes para validações de entrada
- [ ] Adicionar testes para diferentes status codes
- [ ] Verificar cobertura após cada Controller

### Fase 4: Testes de Repositories (15%)
- [ ] Adicionar testes para métodos de consulta
- [ ] Adicionar testes para métodos de persistência
- [ ] Adicionar testes para cenários de erro
- [ ] Verificar cobertura após cada Repository

### Fase 5: Validação Final (5%)
- [ ] Executar testes completos
- [ ] Verificar cobertura no SonarCloud
- [ ] Validar que cobertura >= 85%
- [ ] Documentar cobertura final

## Subtasks

- [ ] [Subtask 01: Analisar cobertura atual no SonarCloud](./subtask/Subtask-01-Analisar_cobertura_atual.md)
- [ ] [Subtask 02: Identificar áreas com baixa cobertura](./subtask/Subtask-02-Identificar_areas_baixa_cobertura.md)
- [ ] [Subtask 03: Adicionar testes para UseCases não cobertos](./subtask/Subtask-03-Testes_UseCases_adicionais.md)
- [ ] [Subtask 04: Adicionar testes para Controllers não cobertos](./subtask/Subtask-04-Testes_Controllers_adicionais.md)
- [ ] [Subtask 05: Adicionar testes para Repositories (se necessário)](./subtask/Subtask-05-Testes_Repositories_adicionais.md)
- [ ] [Subtask 06: Adicionar testes para edge cases](./subtask/Subtask-06-Testes_edge_cases.md)
- [ ] [Subtask 07: Validar cobertura de 85% no SonarCloud](./subtask/Subtask-07-Validar_cobertura_85.md)

## Critérios de Aceite da História

### Análise de Cobertura
- [ ] Relatório de cobertura atual analisado no SonarCloud
- [ ] Lista de classes/métodos com baixa cobertura identificada
- [ ] Priorização de áreas para aumentar cobertura definida
- [ ] Plano de testes adicionais criado

### Testes Adicionais
- [ ] Testes adicionais criados para UseCases não cobertos:
  - Cenários de sucesso não cobertos
  - Edge cases não cobertos
  - Validações de negócio não cobertas
- [ ] Testes adicionais criados para Controllers não cobertos:
  - Cenários de erro não cobertos
  - Validações de entrada não cobertas
  - Diferentes status codes não cobertos
- [ ] Testes adicionais criados para Repositories (se necessário):
  - Métodos de consulta não cobertos
  - Métodos de persistência não cobertos
  - Cenários de erro não cobertos

### Cobertura Final
- [ ] Cobertura >= 85% no SonarCloud
- [ ] Todos os testes passam
- [ ] Cobertura de UseCases >= 90%
- [ ] Cobertura de Controllers >= 85%
- [ ] Cobertura de Repositories >= 80% (se testados)

### Qualidade dos Testes
- [ ] Todos os testes seguem padrão AAA (Arrange, Act, Assert)
- [ ] Nomenclatura descritiva: `[ClasseOuMétodo]_[Cenário]_[ResultadoEsperado]`
- [ ] Testes usam mocks para dependências externas
- [ ] Testes são independentes (podem executar isoladamente)
- [ ] Testes cobrem casos de sucesso e falha
- [ ] Testes cobrem valores limite e edge cases

### Monitoramento
- [ ] Verificação de threshold local comentada no workflow
- [ ] Monitoramento de cobertura configurado no SonarCloud
- [ ] Quality Gate configurado para 85% de cobertura
- [ ] Cobertura visível no SonarCloud após cada PR

## Observações Arquiteturais

### Estratégia de Priorização
1. **UseCases primeiro**: Maior impacto na cobertura, lógica de negócio crítica
2. **Controllers segundo**: Validação de contrato HTTP, importante para API
3. **Repositories terceiro**: Apenas se necessário para atingir 85%
4. **Domain Entities último**: Apenas se necessário para atingir 85%

### Padrão de Testes
- Seguir padrão AAA (Arrange, Act, Assert)
- Usar mocks para todas as dependências externas
- Testar casos de sucesso e falha
- Testar edge cases e valores limite
- Manter testes independentes

### Monitoramento no SonarCloud
- **Verificação local comentada**: A verificação de threshold local foi comentada para permitir monitoramento no SonarCloud
- **SonarCloud como fonte única**: O SonarCloud será a fonte única de verdade para cobertura
- **Quality Gate**: Configurado para bloquear merges quando cobertura < 85%
- **Métricas**: Cobertura visível no dashboard do SonarCloud

### Exclusões de Cobertura
As seguintes exclusões estão configuradas no SonarCloud:
- `**/*Program.cs`
- `**/*Startup.cs`
- `**/Migrations/**`
- `**/*Dto.cs`

### Lições Aprendidas
1. **Focar em áreas críticas primeiro**: UseCases têm maior impacto na cobertura
2. **Testar edge cases**: Muitas vezes são os cenários não cobertos
3. **Revisar testes existentes**: Pode haver cenários não cobertos mesmo com testes existentes
4. **Monitorar incrementalmente**: Verificar cobertura após cada subtask
5. **Usar SonarCloud**: Melhor visualização de cobertura por classe/método

## Métricas de Progresso

### Cobertura Atual
- **Total**: 48.11%
- **Gap para meta**: 36.89%

### Meta por Camada
- **UseCases**: >= 90%
- **Controllers**: >= 85%
- **Repositories**: >= 80% (se testados)
- **Domain**: >= 70% (se necessário)

### Checkpoints
- [ ] Fase 1 concluída: Análise completa (10%)
- [ ] Fase 2 concluída: UseCases >= 90% (40%)
- [ ] Fase 3 concluída: Controllers >= 85% (30%)
- [ ] Fase 4 concluída: Repositories >= 80% (15%)
- [ ] Fase 5 concluída: Cobertura total >= 85% (5%)

---

## ✅ Story Concluída

**Data de Conclusão**: [A preencher após implementação]

### Resumo da Implementação

[Será preenchido após conclusão]

### Status Final

- [ ] Compilação: Sem erros
- [ ] Testes: Todos passando
- [ ] Cobertura: >= 85% no SonarCloud
- [ ] UseCases: >= 90%
- [ ] Controllers: >= 85%
- [ ] Repositories: >= 80% (se testados)
- [ ] SonarCloud: Cobertura visível e validada
- [ ] Quality Gate: Passando

**Próximos Passos**: Manter cobertura >= 85% em todas as novas features.
