# Storie-01: Criar Estrutura Base do Projeto KitchenFlow

## Descrição
Como desenvolvedor, quero criar a estrutura completa do projeto KitchenFlow com todas as camadas da Clean Architecture (.NET 8), para que possamos começar a implementar as funcionalidades do microserviço de fluxo da cozinha.

## Objetivo
Criar a solução .NET 8 com todos os projetos das camadas (Domain, Application, Infra, Infra.Persistence, CrossCutting, Api, Migrator) seguindo a estrutura definida no `kitchenflow-context.mdc`, configurar dependências entre projetos, e implementar uma rota básica "olá mundo" na API para validação inicial.

## Escopo Técnico
- Tecnologias: .NET 8, ASP.NET Core, Entity Framework Core (preparação)
- Estrutura de projetos:
  - `FastFood.KitchenFlow.Domain` (Class Library)
  - `FastFood.KitchenFlow.Application` (Class Library)
  - `FastFood.KitchenFlow.Infra` (Class Library)
  - `FastFood.KitchenFlow.Infra.Persistence` (Class Library)
  - `FastFood.KitchenFlow.CrossCutting` (Class Library)
  - `FastFood.KitchenFlow.Api` (Web API)
  - `FastFood.KitchenFlow.Migrator` (Console Application)
  - `FastFood.KitchenFlow.Tests.Unit` (xUnit Test Project)
  - `FastFood.KitchenFlow.Tests.Bdd` (xUnit Test Project com SpecFlow)
- Arquivos afetados:
  - Solução `.sln` na raiz
  - Estrutura de pastas `src/Core/`, `src/InterfacesExternas/`, `src/tests/`
  - `Program.cs` na API com rota básica
  - `appsettings.json` na API

## Subtasks

- [Subtask 01: Criar solução .NET e estrutura de diretórios](./subtask/Subtask-01-Criar_solucao_estrutura_diretorios.md)
- [Subtask 02: Criar projetos Core (Domain, Application, Infra, Infra.Persistence, CrossCutting)](./subtask/Subtask-02-Criar_projetos_core.md)
- [Subtask 03: Criar projetos InterfacesExternas (Api, Migrator)](./subtask/Subtask-03-Criar_projetos_interfaces_externas.md)
- [Subtask 04: Criar projetos de testes (Tests.Unit, Tests.Bdd)](./subtask/Subtask-04-Criar_projetos_testes.md)
- [Subtask 05: Configurar dependências entre projetos](./subtask/Subtask-05-Configurar_dependencias_projetos.md)
- [Subtask 06: Criar estrutura básica de pastas em cada projeto](./subtask/Subtask-06-Criar_estrutura_pastas_projetos.md)
- [Subtask 07: Implementar rota "Olá Mundo" na API](./subtask/Subtask-07-Implementar_rota_ola_mundo_api.md)
- [Subtask 08: Configurar Program.cs da API com Swagger](./subtask/Subtask-08-Configurar_Program_Swagger.md)
- [Subtask 09: Validar compilação e estrutura completa](./subtask/Subtask-09-Validar_compilacao_estrutura.md)

## Critérios de Aceite da História

- [ ] Solução `.sln` criada na raiz do projeto
- [ ] Todos os 9 projetos criados nas pastas corretas (`src/Core/`, `src/InterfacesExternas/`, `src/tests/`)
- [ ] Todos os projetos adicionados à solução
- [ ] Dependências entre projetos configuradas corretamente:
  - Application → Domain
  - Infra → Application, Domain
  - Infra.Persistence → Domain, Application
  - CrossCutting → Domain, Application
  - Api → Application, CrossCutting, Infra, Infra.Persistence
  - Migrator → Infra.Persistence, CrossCutting
  - Tests.Unit → Todos os projetos Core
  - Tests.Bdd → Todos os projetos Core
- [ ] Estrutura de pastas criada em cada projeto conforme `kitchenflow-context.mdc`
- [ ] API configurada com ASP.NET Core e Swagger
- [ ] Rota GET `/api/health` ou `/api/hello` retornando "Olá Mundo" funcionando
- [ ] Swagger acessível e mostrando a rota criada
- [ ] Todos os projetos compilam sem erros
- [ ] `dotnet build` executa com sucesso na solução completa
- [ ] `dotnet run` na API inicia sem erros e expõe a rota
- [ ] Estrutura segue padrão do projeto de referência `fiap-fase4-auth-lambda`
- [ ] Nomenclatura de projetos segue padrão `FastFood.KitchenFlow.{Camada}`
- [ ] Namespaces seguem padrão dos nomes dos projetos

