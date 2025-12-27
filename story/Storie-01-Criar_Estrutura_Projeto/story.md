# Storie-01: Criar Estrutura Base do Projeto KitchenFlow

## Descrição
Como desenvolvedor, quero criar a estrutura completa do projeto KitchenFlow com todas as camadas da Clean Architecture (.NET 8), para que possamos começar a implementar as funcionalidades do microserviço de fluxo da cozinha.

## Objetivo
Criar a solução .NET 8 com todos os projetos das camadas (Domain, Application, Infra, Infra.Persistence, CrossCutting, Api, Migrator) seguindo a estrutura definida no `kitchenflow-context.mdc`, configurar dependências entre projetos, e implementar uma rota básica "olá mundo" na API para validação inicial.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, Entity Framework Core (preparação)
- Estrutura de projetos:
  - **Core** (`src/Core/`):
    - `FastFood.KitchenFlow.Domain` (Class Library)
    - `FastFood.KitchenFlow.Application` (Class Library)
    - `FastFood.KitchenFlow.CrossCutting` (Class Library)
  - **Infra** (`src/Infra/`):
    - `FastFood.KitchenFlow.Infra` (Class Library)
    - `FastFood.KitchenFlow.Infra.Persistence` (Class Library)
  - **InterfacesExternas** (`src/InterfacesExternas/`):
    - `FastFood.KitchenFlow.Api` (Web API)
    - `FastFood.KitchenFlow.Migrator` (Console Application)
  - **Tests** (`src/tests/`):
    - `FastFood.KitchenFlow.Tests.Unit` (xUnit Test Project)
    - `FastFood.KitchenFlow.Tests.Bdd` (xUnit Test Project com SpecFlow)
- Arquivos afetados:
  - Solução `.sln` na raiz
  - Estrutura de pastas `src/Core/`, `src/Infra/`, `src/InterfacesExternas/`, `src/tests/`
  - `Program.cs` na API com rota básica
  - `appsettings.json` na API

## Subtasks

- [x] [Subtask 01: Criar solução .NET e estrutura de diretórios](./subtask/Subtask-01-Criar_solucao_estrutura_diretorios.md)
- [x] [Subtask 02: Criar projetos Core (Domain, Application, CrossCutting)](./subtask/Subtask-02-Criar_projetos_core.md)
- [x] [Subtask 02b: Criar projetos Infra (Infra, Infra.Persistence)](./subtask/Subtask-02b-Criar_projetos_infra.md)
- [x] [Subtask 03: Criar projetos InterfacesExternas (Api, Migrator)](./subtask/Subtask-03-Criar_projetos_interfaces_externas.md)
- [x] [Subtask 04: Criar projetos de testes (Tests.Unit, Tests.Bdd)](./subtask/Subtask-04-Criar_projetos_testes.md)
- [x] [Subtask 05: Configurar dependências entre projetos](./subtask/Subtask-05-Configurar_dependencias_projetos.md)
- [x] [Subtask 06: Criar estrutura básica de pastas em cada projeto](./subtask/Subtask-06-Criar_estrutura_pastas_projetos.md)
- [x] [Subtask 07: Implementar rota "Olá Mundo" na API](./subtask/Subtask-07-Implementar_rota_ola_mundo_api.md)
- [x] [Subtask 08: Configurar Program.cs da API com Swagger](./subtask/Subtask-08-Configurar_Program_Swagger.md)
- [x] [Subtask 09: Validar compilação e estrutura completa](./subtask/Subtask-09-Validar_compilacao_estrutura.md)

## Critérios de Aceite da História

- [x] Solução `.sln` criada na raiz do projeto
- [x] Todos os 9 projetos criados nas pastas corretas (`src/Core/`, `src/Infra/`, `src/InterfacesExternas/`, `src/tests/`)
- [x] Todos os projetos adicionados à solução
- [x] Dependências entre projetos configuradas corretamente:
  - Application → Domain
  - Infra → Application, Domain
  - Infra.Persistence → Domain, Application
  - CrossCutting → Domain, Application
  - Api → Application, CrossCutting, Infra, Infra.Persistence
  - Migrator → Infra.Persistence, CrossCutting
  - Tests.Unit → Todos os projetos Core
  - Tests.Bdd → Todos os projetos Core
- [x] Estrutura de pastas criada em cada projeto conforme `kitchenflow-context.mdc`
- [x] API configurada com ASP.NET Core e Swagger
- [x] Rota GET `/api/health` ou `/api/hello` retornando "Olá Mundo" funcionando
- [x] Swagger acessível e mostrando a rota criada
- [x] Todos os projetos compilam sem erros
- [x] `dotnet build` executa com sucesso na solução completa
- [x] `dotnet run` na API inicia sem erros e expõe a rota
- [x] Estrutura segue padrão do projeto de referência `fiap-fase4-auth-lambda`
- [x] Nomenclatura de projetos segue padrão `FastFood.KitchenFlow.{Camada}`
- [x] Namespaces seguem padrão dos nomes dos projetos

