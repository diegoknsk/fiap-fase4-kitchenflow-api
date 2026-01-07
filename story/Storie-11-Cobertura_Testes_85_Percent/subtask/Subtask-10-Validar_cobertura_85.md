# Subtask 10: Validar cobertura de 85%

## Descrição
Validar que o projeto atinge pelo menos 85% de cobertura de testes unitários, tanto localmente quanto no SonarCloud, e garantir que o Quality Gate está configurado corretamente para manter essa cobertura.

## Passos de implementação
- Executar testes localmente com cobertura:
  ```bash
  dotnet test --collect:"XPlat Code Coverage" /p:CoverletOutputFormat=opencover
  ```
- Verificar relatório de cobertura local
- Verificar cobertura no SonarCloud após execução do workflow
- Verificar que o Quality Gate está configurado com cobertura mínima de 85%
- Verificar que o workflow bloqueia merges quando cobertura < 85%
- Documentar cobertura atual e identificar áreas que precisam de mais testes

## Verificação local de cobertura

### Executar testes com cobertura:
```bash
dotnet test --collect:"XPlat Code Coverage" /p:CoverletOutputFormat=opencover
```

### Verificar arquivo de cobertura gerado:
- Arquivo `coverage.opencover.xml` deve ser gerado
- Verificar cobertura por classe/método
- Identificar áreas com baixa cobertura

### Usar ferramenta para visualizar cobertura (opcional):
- ReportGenerator: `dotnet tool install -g dotnet-reportgenerator-globaltool`
- Gerar relatório HTML: `reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report -reporttypes:Html`

## Verificação no SonarCloud

### Após execução do workflow:
1. Acessar https://sonarcloud.io
2. Navegar até o projeto
3. Verificar aba "Measures" → "Coverage"
4. Verificar que a cobertura é >= 85%
5. Verificar que o Quality Gate passou

### Verificar Quality Gate:
1. Acessar "Quality Gates"
2. Verificar que a cobertura mínima está configurada para 85%
3. Verificar que o Quality Gate bloqueia quando cobertura < 85%

## Áreas a verificar cobertura

### UseCases (prioridade alta):
- [ ] CreatePreparationUseCase >= 90%
- [ ] GetPreparationsUseCase >= 90%
- [ ] StartPreparationUseCase >= 90%
- [ ] FinishPreparationUseCase >= 90%
- [ ] CreateDeliveryUseCase >= 90%
- [ ] GetReadyDeliveriesUseCase >= 90%
- [ ] FinalizeDeliveryUseCase >= 90%

### Controllers (prioridade média):
- [ ] PreparationController >= 85%
- [ ] DeliveryController >= 85%

### Domain (já testado na Story 03):
- [ ] Preparation >= 90%
- [ ] Delivery >= 90%

### Repositories (se necessário para atingir 85%):
- [ ] PreparationRepository >= 80%
- [ ] DeliveryRepository >= 80%

## Estratégia para atingir 85%

### Se cobertura < 85%:
1. **Identificar áreas com baixa cobertura**:
   - UseCases não testados
   - Controllers não testados
   - Métodos não testados em classes parcialmente testadas

2. **Priorizar testes**:
   - UseCases primeiro (maior impacto)
   - Controllers depois
   - Repositories apenas se necessário

3. **Adicionar testes**:
   - Criar testes para áreas não cobertas
   - Adicionar edge cases
   - Adicionar testes de validação

4. **Verificar novamente**:
   - Executar testes localmente
   - Verificar cobertura
   - Repetir até atingir 85%

## Documentação da cobertura

### Criar documento com:
- Cobertura atual por camada
- Áreas com baixa cobertura
- Plano para atingir 85%
- Testes adicionais necessários

## Observações importantes
- **Meta de 85%**: Cobertura mínima obrigatória
- **Foco em qualidade**: Melhor ter menos testes de qualidade do que muitos testes ruins
- **Prioridade**: UseCases > Controllers > Repositories
- **Exclusões**: Program.cs, Startup.cs, Migrations, DTOs são excluídos da cobertura
- **Manutenção**: Manter cobertura >= 85% em todas as novas features

## Como testar
- Executar `dotnet test --collect:"XPlat Code Coverage"` localmente
- Verificar relatório de cobertura
- Criar Pull Request e verificar workflow
- Verificar cobertura no SonarCloud
- Verificar que Quality Gate passa quando cobertura >= 85%
- Verificar que Quality Gate bloqueia quando cobertura < 85%

## Critérios de aceite
- Cobertura local >= 85% (verificado com `dotnet test --collect:"XPlat Code Coverage"`)
- Cobertura no SonarCloud >= 85%
- Quality Gate configurado com cobertura mínima de 85%
- Quality Gate passa quando cobertura >= 85%
- Quality Gate bloqueia quando cobertura < 85%
- Workflow executa com sucesso
- Cobertura aparece corretamente no SonarCloud
- Documentação da cobertura criada (se necessário)
