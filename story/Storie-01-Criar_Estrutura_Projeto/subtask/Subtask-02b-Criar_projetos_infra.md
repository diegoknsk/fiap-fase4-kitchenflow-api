# Subtask 02b: Criar projetos Infra (Infra, Infra.Persistence)

## Descrição
Criar os 2 projetos de Infraestrutura dentro de `src/Infra/`. Estes projetos implementam as interfaces definidas na camada Application e fornecem acesso a serviços externos e persistência.

## Passos de implementação
- Criar projeto `FastFood.KitchenFlow.Infra` (Class Library) em `src/Infra/FastFood.KitchenFlow.Infra/`
- Criar projeto `FastFood.KitchenFlow.Infra.Persistence` (Class Library) em `src/Infra/FastFood.KitchenFlow.Infra.Persistence/`
- Adicionar ambos os projetos à solução usando `dotnet sln add`
- Configurar target framework .NET 8 em todos os projetos

## Como testar
- Executar `dotnet build` em cada projeto individualmente (deve compilar mesmo que vazio)
- Executar `dotnet sln list` para verificar que todos os projetos foram adicionados
- Verificar que os arquivos `.csproj` foram criados com `<TargetFramework>net8.0</TargetFramework>`

## Critérios de aceite
- 2 projetos Infra criados nas pastas corretas (`src/Infra/`)
- Todos os projetos configurados com .NET 8
- Todos os projetos adicionados à solução
- Cada projeto compila individualmente sem erros
- Estrutura de pastas `src/Infra/` criada corretamente



