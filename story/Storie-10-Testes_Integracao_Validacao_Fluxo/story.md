# Storie-10: Testes Básicos de Integração e Validação de Fluxo

## Descrição
Como desenvolvedor, quero criar testes de integração e testes BDD para validar o fluxo completo do KitchenFlow, desde a criação de preparação pelo Payment até a finalização da entrega, garantindo que todos os endpoints funcionam corretamente e que o fluxo end-to-end está funcionando.

## Objetivo
Criar testes de integração e BDD para validar:
- Fluxo completo: Payment cria Preparation → Preparation é iniciada → Preparation é finalizada → Delivery é criada → Delivery é finalizada
- Endpoints principais funcionando corretamente
- Validações de negócio funcionando
- Integração entre Preparation e Delivery
- Cenários de erro e validações

## Escopo Técnico
- **Tecnologias**: .NET 8, xUnit, SpecFlow (para BDD), Testcontainers ou banco em memória
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Tests.Unit` (testes unitários adicionais se necessário)
  - `FastFood.KitchenFlow.Tests.Bdd` (testes BDD do fluxo completo)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (padrão de testes)
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda` (padrão de testes BDD)

## Fluxo End-to-End a Testar

### Fluxo Completo (Happy Path)
1. Payment chama `POST /api/preparations` → Preparation criada (status Received)
2. Sistema chama `POST /api/preparations/{id}/start` → Preparation iniciada (status InProgress)
3. Sistema chama `POST /api/preparations/{id}/finish` → Preparation finalizada (status Finished)
4. Sistema chama `POST /api/deliveries` → Delivery criada (status ReadyForPickup)
5. Sistema chama `POST /api/deliveries/{id}/finalize` → Delivery finalizada (status Finalized)

### Cenários de Erro
- Preparação duplicada (idempotência)
- Transições de status inválidas
- Preparation não encontrada
- Delivery não encontrada
- Validações de dados inválidos

## Subtasks

- [ ] [Subtask 01: Configurar ambiente de testes de integração](./subtask/Subtask-01-Configurar_ambiente_testes.md)
- [ ] [Subtask 02: Criar testes de integração dos endpoints](./subtask/Subtask-02-Criar_testes_integracao.md)
- [ ] [Subtask 03: Criar cenário BDD do fluxo completo](./subtask/Subtask-03-Criar_cenario_BDD.md)
- [ ] [Subtask 04: Criar testes de validação de fluxo](./subtask/Subtask-04-Criar_testes_validacao.md)
- [ ] [Subtask 05: Validar cobertura e qualidade](./subtask/Subtask-05-Validar_cobertura.md)

## Critérios de Aceite da História

- [ ] Ambiente de testes configurado:
  - Banco de dados de teste (Testcontainers ou in-memory)
  - Configuração de testes de integração
  - Setup/Teardown de dados de teste
- [ ] Testes de integração criados:
  - Testes dos endpoints principais
  - Testes de validações
  - Testes de cenários de erro
- [ ] Cenário BDD criado:
  - Fluxo completo documentado em Gherkin
  - Steps implementados
  - Cenário passa com sucesso
- [ ] Testes de validação criados:
  - Validação de transições de status
  - Validação de relacionamento Preparation → Delivery
  - Validação de regras de negócio
- [ ] Cobertura de testes:
  - Cobertura mínima de 80% (ou o mais próximo possível)
  - Testes cobrem fluxos principais
  - Testes cobrem cenários de erro
- [ ] Todos os testes passam:
  - `dotnet test` executa com sucesso
  - Testes de integração passam
  - Testes BDD passam
- [ ] Documentação de testes criada:
  - Como executar testes
  - Estrutura de testes
  - Cenários BDD documentados

## Observações Arquiteturais

### Tipos de Testes

#### Testes Unitários
- **Foco**: Lógica de negócio isolada
- **Onde**: Domain, UseCases (com mocks)
- **Já criados**: Story 03 (entidades de domínio)

#### Testes de Integração
- **Foco**: Integração entre camadas (API → UseCase → Repository → Banco)
- **Onde**: Testes que usam banco de dados real ou em memória
- **Setup**: Configurar banco de teste, criar dados, limpar após testes

#### Testes BDD
- **Foco**: Fluxo completo end-to-end
- **Onde**: Cenários de negócio documentados em Gherkin
- **Ferramenta**: SpecFlow ou estilo BDD com xUnit

### Ambiente de Testes
- **Banco de dados**: Usar Testcontainers (PostgreSQL em container) ou banco em memória
- **Isolamento**: Cada teste deve ser independente
- **Setup/Teardown**: Limpar dados entre testes

### Cobertura
- **Meta**: >= 80% de cobertura
- **Foco**: Fluxos principais e regras de negócio
- **Prioridade**: Testar o que é mais importante primeiro

### Análise dos Projetos de Referência

**Auth (testes):**
- Testes unitários para Domain e UseCases
- Testes BDD para fluxos principais
- Meta de cobertura >= 80%

**Aplicação no KitchenFlow:**
- Testes unitários (já criados na Story 03)
- Testes de integração para endpoints
- Testes BDD para fluxo completo
- Meta de cobertura >= 80%

### Fluxo Completo a Testar

**Cenário BDD Principal:**
```
Dado que o Payment confirma um pagamento
Quando o Payment chama POST /api/preparations
Então uma Preparation é criada com status Received

Dado que existe uma Preparation com status Received
Quando o sistema chama POST /api/preparations/{id}/start
Então a Preparation fica com status InProgress

Dado que existe uma Preparation com status InProgress
Quando o sistema chama POST /api/preparations/{id}/finish
Então a Preparation fica com status Finished

Dado que existe uma Preparation com status Finished
Quando o sistema chama POST /api/deliveries
Então uma Delivery é criada com status ReadyForPickup

Dado que existe uma Delivery com status ReadyForPickup
Quando o sistema chama POST /api/deliveries/{id}/finalize
Então a Delivery fica com status Finalized
```
