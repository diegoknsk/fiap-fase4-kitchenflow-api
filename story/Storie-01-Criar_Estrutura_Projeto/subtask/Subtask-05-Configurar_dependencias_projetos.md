# Subtask 05: Configurar dependências entre projetos

## Descrição
Configurar as referências de projeto corretas entre todas as camadas, seguindo os princípios da Clean Architecture e as regras definidas no `kitchenflow-context.mdc` e `baseprojectcontext.mdc`.

## Passos de implementação
- **Application → Domain:**
  - Adicionar referência de `FastFood.KitchenFlow.Application` para `FastFood.KitchenFlow.Domain`
- **Infra → Application, Domain:**
  - Adicionar referências de `FastFood.KitchenFlow.Infra` para `FastFood.KitchenFlow.Application` e `FastFood.KitchenFlow.Domain`
- **Infra.Persistence → Domain, Application:**
  - Adicionar referências de `FastFood.KitchenFlow.Infra.Persistence` para `FastFood.KitchenFlow.Domain` e `FastFood.KitchenFlow.Application`
- **CrossCutting → Domain, Application:**
  - Adicionar referências de `FastFood.KitchenFlow.CrossCutting` para `FastFood.KitchenFlow.Domain` e `FastFood.KitchenFlow.Application`
- **Api → Application, CrossCutting, Infra, Infra.Persistence:**
  - Adicionar referências de `FastFood.KitchenFlow.Api` para:
    - `FastFood.KitchenFlow.Application`
    - `FastFood.KitchenFlow.CrossCutting`
    - `FastFood.KitchenFlow.Infra`
    - `FastFood.KitchenFlow.Infra.Persistence`
- **Migrator → Infra.Persistence, CrossCutting:**
  - Adicionar referências de `FastFood.KitchenFlow.Migrator` para:
    - `FastFood.KitchenFlow.Infra.Persistence`
    - `FastFood.KitchenFlow.CrossCutting`
- **Tests.Unit → Todos os projetos Core:**
  - Adicionar referências de `FastFood.KitchenFlow.Tests.Unit` para:
    - `FastFood.KitchenFlow.Domain`
    - `FastFood.KitchenFlow.Application`
    - `FastFood.KitchenFlow.Infra`
    - `FastFood.KitchenFlow.Infra.Persistence`
    - `FastFood.KitchenFlow.CrossCutting`
- **Tests.Bdd → Todos os projetos Core:**
  - Adicionar referências de `FastFood.KitchenFlow.Tests.Bdd` para:
    - `FastFood.KitchenFlow.Domain`
    - `FastFood.KitchenFlow.Application`
    - `FastFood.KitchenFlow.Infra`
    - `FastFood.KitchenFlow.Infra.Persistence`
    - `FastFood.KitchenFlow.CrossCutting`

## Como testar
- Executar `dotnet build` na solução completa (deve compilar sem erros)
- Verificar que não há dependências circulares
- Verificar que Domain não referencia nenhum outro projeto
- Verificar que Application só referencia Domain
- Verificar que Api referencia todos os projetos necessários

## Critérios de aceite
- Todas as referências de projeto configuradas corretamente
- Domain não tem dependências (exceto .NET Standard)
- Application só referencia Domain
- Infra referencia Application e Domain
- Infra.Persistence referencia Domain e Application
- CrossCutting referencia Domain e Application
- Api referencia Application, CrossCutting, Infra e Infra.Persistence
- Migrator referencia Infra.Persistence e CrossCutting
- Tests.Unit e Tests.Bdd referenciam todos os projetos Core
- `dotnet build` compila toda a solução sem erros
- Não há dependências circulares


