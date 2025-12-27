# Subtask 04: Criar projetos de testes (Tests.Unit, Tests.Bdd)

## Descrição
Criar os 2 projetos de testes: Testes Unitários (xUnit) e Testes BDD (xUnit com SpecFlow) dentro de `src/tests/`. Estes projetos serão usados para garantir qualidade e cobertura de código.

## Passos de implementação
- Criar projeto `FastFood.KitchenFlow.Tests.Unit` (xUnit Test Project) em `src/tests/FastFood.KitchenFlow.Tests.Unit/`
  - Usar template `dotnet new xunit`
  - Configurar para .NET 8
- Criar projeto `FastFood.KitchenFlow.Tests.Bdd` (xUnit Test Project) em `src/tests/FastFood.KitchenFlow.Tests.Bdd/`
  - Usar template `dotnet new xunit`
  - Configurar para .NET 8
  - Adicionar pacote NuGet `SpecFlow` e `SpecFlow.xUnit` (versões compatíveis com .NET 8)
- Adicionar ambos os projetos à solução usando `dotnet sln add`
- Remover arquivo de teste de exemplo (UnitTest1.cs) se existir

## Como testar
- Executar `dotnet build` em cada projeto de teste
- Executar `dotnet test` em cada projeto (deve passar mesmo sem testes)
- Verificar que os pacotes SpecFlow foram instalados no projeto BDD
- Executar `dotnet sln list` para verificar que os projetos foram adicionados

## Critérios de aceite
- Projeto Tests.Unit criado como xUnit em `src/tests/FastFood.KitchenFlow.Tests.Unit/`
- Projeto Tests.Bdd criado como xUnit em `src/tests/FastFood.KitchenFlow.Tests.Bdd/`
- Pacotes SpecFlow adicionados ao projeto Tests.Bdd
- Ambos configurados com .NET 8
- Ambos adicionados à solução
- `dotnet test` executa com sucesso em ambos os projetos
- Arquivos de exemplo removidos




