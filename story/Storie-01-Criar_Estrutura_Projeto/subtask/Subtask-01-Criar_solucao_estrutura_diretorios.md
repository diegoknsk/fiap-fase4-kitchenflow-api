# Subtask 01: Criar solução .NET e estrutura de diretórios

## Descrição
Criar a solução .NET 8 e a estrutura de diretórios base do projeto KitchenFlow conforme definido no `kitchenflow-context.mdc`. Esta subtask estabelece a fundação da organização do projeto.

## Passos de implementação
- Criar solução .NET 8 na raiz: `FastFood.KitchenFlow.sln`
- Criar estrutura de diretórios:
  - `src/Core/` (para projetos Domain, Application, CrossCutting)
  - `src/Infra/` (para projetos Infra e Infra.Persistence)
  - `src/InterfacesExternas/` (para projetos Api e Migrator)
  - `src/tests/` (para projetos de testes)
- Verificar que a estrutura está alinhada com o projeto de referência `C:\Projetos\Fiap\fiap-fase4-auth-lambda`

## Como testar
- Executar `dotnet sln list` para verificar que a solução foi criada (mesmo que vazia)
- Verificar que os diretórios foram criados corretamente
- Comparar estrutura com projeto de referência

## Critérios de aceite
- Solução `FastFood.KitchenFlow.sln` criada na raiz
- Diretórios `src/Core/`, `src/Infra/`, `src/InterfacesExternas/`, `src/tests/` criados
- Estrutura de diretórios alinhada com projeto de referência
- Solução pode ser aberta no Visual Studio ou VS Code


