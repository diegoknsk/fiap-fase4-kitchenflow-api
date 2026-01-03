# Storie-11: Atingir 85% de Cobertura de Testes Unitários no SonarCloud

## Descrição
Como desenvolvedor, quero criar testes unitários completos para todas as camadas do KitchenFlow (UseCases, Controllers, Repositories) e configurar integração com SonarCloud, para garantir que o projeto atinja pelo menos 85% de cobertura de testes e mantenha alta qualidade de código.

## Objetivo
Atingir 85% de cobertura de testes unitários através de:
- Configuração completa do projeto de testes com pacotes necessários (Moq, FluentAssertions, Coverlet)
- Criação de testes unitários para todos os UseCases (7 UseCases)
- Criação de testes unitários para todos os Controllers (2 Controllers)
- Criação de testes unitários para Repositories (se necessário)
- Configuração do workflow `quality.yml` para integração com SonarCloud
- Configuração do SonarCloud (projeto, Quality Gate, desabilitar análise automática)
- Validação de que a cobertura mínima de 85% é atingida e mantida

## Escopo Técnico
- **Tecnologias**: .NET 8, xUnit, Moq, FluentAssertions, Coverlet, SonarCloud
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Tests.Unit` (estrutura completa de testes)
  - `.github/workflows/quality.yml` (workflow de qualidade)
  - SonarCloud (configuração do projeto)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de testes e workflows)
  - `docs/PROMPT_MICROSERVICOS_TESTES_DEPLOY.md` (lições aprendidas)
- **Meta de cobertura**: >= 85%

## Componentes a Testar

### UseCases (7 UseCases)
1. **PreparationManagement**:
   - `CreatePreparationUseCase` - Criar preparação
   - `GetPreparationsUseCase` - Listar preparações
   - `StartPreparationUseCase` - Iniciar preparação
   - `FinishPreparationUseCase` - Finalizar preparação

2. **DeliveryManagement**:
   - `CreateDeliveryUseCase` - Criar entrega
   - `GetReadyDeliveriesUseCase` - Listar entregas prontas
   - `FinalizeDeliveryUseCase` - Finalizar entrega

### Controllers (2 Controllers)
1. `PreparationController` - 4 endpoints
2. `DeliveryController` - 3 endpoints

### Repositories (Opcional, se necessário para cobertura)
1. `PreparationRepository` - Métodos principais
2. `DeliveryRepository` - Métodos principais

## Subtasks

- [ ] [Subtask 01: Configurar pacotes NuGet do projeto de testes](./subtask/Subtask-01-Configurar_pacotes_NuGet.md)
- [ ] [Subtask 02: Criar estrutura de pastas para testes](./subtask/Subtask-02-Criar_estrutura_pastas.md)
- [ ] [Subtask 03: Criar testes para UseCases de PreparationManagement](./subtask/Subtask-03-Testes_UseCases_PreparationManagement.md)
- [ ] [Subtask 04: Criar testes para UseCases de DeliveryManagement](./subtask/Subtask-04-Testes_UseCases_DeliveryManagement.md)
- [ ] [Subtask 05: Criar testes para PreparationController](./subtask/Subtask-05-Testes_PreparationController.md)
- [ ] [Subtask 06: Criar testes para DeliveryController](./subtask/Subtask-06-Testes_DeliveryController.md)
- [ ] [Subtask 07: Criar testes para Repositories (se necessário)](./subtask/Subtask-07-Testes_Repositories.md)
- [ ] [Subtask 08: Criar workflow quality.yml para SonarCloud](./subtask/Subtask-08-Criar_workflow_quality.md)
- [ ] [Subtask 09: Configurar SonarCloud](./subtask/Subtask-09-Configurar_SonarCloud.md)
- [ ] [Subtask 10: Validar cobertura de 85%](./subtask/Subtask-10-Validar_cobertura_85.md)

## Critérios de Aceite da História

### Configuração de Testes
- [ ] Projeto `FastFood.KitchenFlow.Tests.Unit` configurado com:
  - `Moq` (Version 4.20.70)
  - `FluentAssertions` (Version 6.12.0)
  - `coverlet.collector` (Version 6.0.0)
  - `coverlet.msbuild` (Version 6.0.0)
  - `Microsoft.NET.Test.Sdk` (Version 17.8.0)
  - `xunit` (Version 2.6.2)
  - `xunit.runner.visualstudio` (Version 2.5.4)
- [ ] Estrutura de pastas espelha código de produção:
  - `Application/UseCases/PreparationManagement/`
  - `Application/UseCases/DeliveryManagement/`
  - `InterfacesExternas/Controllers/`

### Testes de UseCases
- [ ] `CreatePreparationUseCase` testado:
  - Cenário de sucesso (criação normal)
  - Validação de OrderId vazio
  - Validação de OrderSnapshot nulo/vazio
  - Validação de JSON inválido
  - Validação de OrderSnapshot inválido (Data Annotations)
  - Validação de OrderId não corresponde
  - Idempotência (Preparation já existe)
- [ ] `GetPreparationsUseCase` testado:
  - Listagem com paginação
  - Filtro por status
  - Validação de parâmetros inválidos
- [ ] `StartPreparationUseCase` testado:
  - Iniciar preparação com sucesso
  - Preparation não encontrada
  - Status inválido para iniciar
- [ ] `FinishPreparationUseCase` testado:
  - Finalizar preparação com sucesso
  - Preparation não encontrada
  - Status inválido para finalizar
- [ ] `CreateDeliveryUseCase` testado:
  - Criar entrega com sucesso
  - Preparation não encontrada
  - Preparation não está Finished
  - Idempotência (Delivery já existe)
- [ ] `GetReadyDeliveriesUseCase` testado:
  - Listar entregas prontas
  - Validação de parâmetros
- [ ] `FinalizeDeliveryUseCase` testado:
  - Finalizar entrega com sucesso
  - Delivery não encontrada
  - Status inválido para finalizar

### Testes de Controllers
- [ ] `PreparationController` testado:
  - `POST /api/preparations` - 201, 400, 409
  - `GET /api/preparations` - 200, 400
  - `POST /api/preparations/{id}/start` - 200, 400, 404
  - `POST /api/preparations/{id}/finish` - 200, 400, 404
- [ ] `DeliveryController` testado:
  - `POST /api/deliveries` - 201, 400, 404, 409
  - `GET /api/deliveries/ready` - 200, 400
  - `POST /api/deliveries/{id}/finalize` - 200, 400, 404

### Padrões de Testes
- [ ] Todos os testes seguem padrão AAA (Arrange, Act, Assert)
- [ ] Nomenclatura descritiva: `[ClasseOuMétodo]_[Cenário]_[ResultadoEsperado]`
- [ ] Testes usam mocks para dependências externas
- [ ] Testes são independentes (podem executar isoladamente)
- [ ] Testes cobrem casos de sucesso e falha
- [ ] Testes cobrem valores limite e edge cases

### Workflow e SonarCloud
- [ ] Workflow `quality.yml` criado em `.github/workflows/`
- [ ] Workflow configurado com:
  - Build com símbolos de debug (`/p:DebugType=portable /p:DebugSymbols=true`)
  - Testes com cobertura em formato OpenCover
  - Consolidação de arquivos de cobertura
  - Verificação de threshold de 85% de cobertura
  - Integração com SonarCloud
- [ ] SonarCloud configurado:
  - Projeto criado no SonarCloud
  - Análise Automática desabilitada
  - Quality Gate configurado com cobertura mínima de 85%
  - Token configurado como secret no GitHub (`SONAR_TOKEN`)
- [ ] Workflow executa em Pull Requests e push para main
- [ ] Quality Gate bloqueia merges quando cobertura < 85%

### Validação Final
- [ ] `dotnet test` executa sem erros
- [ ] Todos os testes passam
- [ ] Cobertura de código >= 85% (verificado localmente e no SonarCloud)
- [ ] Workflow de qualidade executa com sucesso
- [ ] Cobertura aparece no SonarCloud
- [ ] Quality Gate passa quando cobertura >= 85%
- [ ] Quality Gate bloqueia quando cobertura < 85%

## Observações Arquiteturais

### Estrutura de Testes
A estrutura de testes deve **espelhar** a estrutura do código de produção para facilitar manutenção:

```
FastFood.KitchenFlow.Tests.Unit/
├── Application/
│   └── UseCases/
│       ├── PreparationManagement/
│       │   ├── CreatePreparationUseCaseTests.cs
│       │   ├── GetPreparationsUseCaseTests.cs
│       │   ├── StartPreparationUseCaseTests.cs
│       │   └── FinishPreparationUseCaseTests.cs
│       └── DeliveryManagement/
│           ├── CreateDeliveryUseCaseTests.cs
│           ├── GetReadyDeliveriesUseCaseTests.cs
│           └── FinalizeDeliveryUseCaseTests.cs
└── InterfacesExternas/
    └── Controllers/
        ├── PreparationControllerTests.cs
        └── DeliveryControllerTests.cs
```

### Padrão de Nomenclatura
```
[ClasseOuMétodo]_[Cenário]_[ResultadoEsperado]
```

**Exemplos:**
- `CreatePreparation_WhenValidInput_ShouldReturnSuccess`
- `CreatePreparation_WhenOrderIdIsEmpty_ShouldThrowArgumentException`
- `CreatePreparation_WhenPreparationAlreadyExists_ShouldThrowPreparationAlreadyExistsException`
- `StartPreparation_WhenStatusIsNotReceived_ShouldThrowInvalidOperationException`

### Padrão AAA (Arrange, Act, Assert)
Todos os testes devem seguir este padrão:

```csharp
[Fact]
public void CreatePreparation_WhenValidInput_ShouldReturnSuccess()
{
    // Arrange
    var orderId = Guid.NewGuid();
    var orderSnapshot = "{ ... }";
    var mockRepository = new Mock<IPreparationRepository>();
    mockRepository.Setup(r => r.GetByOrderIdAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Preparation?)null);
    mockRepository.Setup(r => r.CreateAsync(It.IsAny<Preparation>()))
        .Returns(Task.CompletedTask);
    
    var useCase = new CreatePreparationUseCase(mockRepository.Object);
    var inputModel = new CreatePreparationInputModel
    {
        OrderId = orderId,
        OrderSnapshot = orderSnapshot
    };
    
    // Act
    var result = await useCase.ExecuteAsync(inputModel);
    
    // Assert
    result.Should().NotBeNull();
    result.Id.Should().NotBeEmpty();
    result.OrderId.Should().Be(orderId);
    result.Status.Should().Be((int)EnumPreparationStatus.Received);
    mockRepository.Verify(r => r.CreateAsync(It.IsAny<Preparation>()), Times.Once);
}
```

### Regras para Testes Unitários
1. **Um teste, uma responsabilidade**: Cada teste verifica apenas um comportamento
2. **Use mocks para dependências externas**: Não use dependências reais (banco, APIs externas)
3. **Teste casos de sucesso e falha**: Cubra caminho feliz e casos de erro
4. **Teste valores limite**: Teste mínimos, máximos e edge cases
5. **Testes independentes**: Cada teste deve poder executar isoladamente
6. **SEMPRE executar testes após criá-los**: Execute `dotnet test` para verificar compilação e execução

### Workflow quality.yml
O workflow deve seguir o padrão do documento de lições aprendidas:
- Build com símbolos de debug (CRÍTICO para cobertura)
- Formato OpenCover (único formato suportado pelo SonarCloud)
- Consolidação de arquivos de cobertura
- Verificação de threshold de 85% antes do Sonar End
- Configuração correta para Pull Requests e branch main

### SonarCloud
**CRÍTICO**: Desabilitar Análise Automática no SonarCloud:
1. Acesse https://sonarcloud.io
2. Navegue até o projeto
3. Vá em **Administration** → **Analysis Method**
4. Desative **Automatic Analysis**
5. Salve as alterações

**Por quê?**: Análise automática e CI/CD são mutuamente exclusivas.

### Cobertura de Código
- **Meta obrigatória: 85% de cobertura**
- **Exclusões de cobertura**: `**/*Program.cs,**/*Startup.cs,**/Migrations/**,**/*Dto.cs`
- **Foco**: UseCases, Controllers, Repositories (lógica de negócio)
- **Prioridade**: Testar o que é mais importante primeiro (UseCases > Controllers > Repositories)

### Análise dos Projetos de Referência

**OrderHub (testes e workflows):**
- Estrutura de testes espelha código de produção
- Testes unitários para UseCases com mocks
- Testes de Controllers focados em contrato HTTP
- Workflow quality.yml com SonarCloud
- Meta de cobertura >= 80% (nós queremos 85%)

**Aplicação no KitchenFlow:**
- Seguir padrão do OrderHub
- Aumentar meta para 85%
- Focar em UseCases primeiro (maior impacto na cobertura)
- Depois Controllers (validação de contrato HTTP)
- Repositories apenas se necessário para atingir 85%

### Lições Aprendidas (do documento)
1. **Build com símbolos de debug é CRÍTICO**: Sem `/p:DebugType=portable /p:DebugSymbols=true`, a cobertura não será processada corretamente
2. **Formato OpenCover**: Único formato suportado pelo SonarCloud
3. **Consolidação de arquivos**: Coverlet gera arquivos em múltiplos locais, é necessário consolidar em um único arquivo
4. **Verificação antes do Sonar End**: Sempre verificar se o arquivo de cobertura existe e é válido
5. **Desabilitar Análise Automática**: OBRIGATÓRIO para evitar conflitos com CI/CD
6. **Quality Gate Wait**: Usar `/d:sonar.qualitygate.wait=true` para bloquear merges quando Quality Gate falhar

---

## ✅ Story Concluída

**Data de Conclusão**: [A preencher após implementação]

### Resumo da Implementação

[Será preenchido após conclusão]

### Status Final

- [ ] Compilação: Sem erros
- [ ] Testes: Todos passando
- [ ] Cobertura: >= 85%
- [ ] Workflow: Executando com sucesso
- [ ] SonarCloud: Integrado e funcionando
- [ ] Quality Gate: Passando

**Próximos Passos**: Manter cobertura >= 85% em todas as novas features.
