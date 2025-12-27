# Subtask 02: Criar projetos Core (Domain, Application, Infra, Infra.Persistence, CrossCutting)

## Descrição
Criar os 3 projetos Core da Clean Architecture dentro de `src/Core/`. Estes projetos formam o núcleo da aplicação e não dependem de frameworks externos (exceto Domain que não depende de nada).

## Passos de implementação
- Criar projeto `FastFood.KitchenFlow.Domain` (Class Library) em `src/Core/FastFood.KitchenFlow.Domain/`
- Criar projeto `FastFood.KitchenFlow.Application` (Class Library) em `src/Core/FastFood.KitchenFlow.Application/`
- Criar projeto `FastFood.KitchenFlow.CrossCutting` (Class Library) em `src/Core/FastFood.KitchenFlow.CrossCutting/`
- Adicionar todos os projetos à solução usando `dotnet sln add`
- Configurar target framework .NET 8 em todos os projetos

## Como testar
- Executar `dotnet build` em cada projeto individualmente (deve compilar mesmo que vazio)
- Executar `dotnet sln list` para verificar que todos os projetos foram adicionados
- Verificar que os arquivos `.csproj` foram criados com `<TargetFramework>net8.0</TargetFramework>`

## Critérios de aceite
- 3 projetos Core criados nas pastas corretas (`src/Core/`)
- Todos os projetos configurados com .NET 8
- Todos os projetos adicionados à solução
- Cada projeto compila individualmente sem erros
- Estrutura de pastas `src/Core/` criada corretamente


