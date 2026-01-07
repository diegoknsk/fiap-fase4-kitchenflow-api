# Subtask 01: Configurar dependências EF Core e Npgsql

## Descrição
Adicionar as dependências necessárias do Entity Framework Core e Npgsql no projeto `FastFood.KitchenFlow.Infra.Persistence` para suportar PostgreSQL.

## Passos de implementação
- Abrir o arquivo `.csproj` do projeto `FastFood.KitchenFlow.Infra.Persistence`
- Adicionar pacotes NuGet:
  - `Microsoft.EntityFrameworkCore` (versão compatível com .NET 8)
  - `Npgsql.EntityFrameworkCore.PostgreSQL` (versão compatível com .NET 8)
  - `Microsoft.EntityFrameworkCore.Design` (para migrations, se necessário)
- Verificar que o projeto já referencia `FastFood.KitchenFlow.Domain` (necessário para usar os enums)
- Restaurar pacotes: `dotnet restore`
- Verificar que não há conflitos de versão

## Como testar
- Executar `dotnet restore` no projeto
- Executar `dotnet build` para verificar que compila
- Verificar que os pacotes foram instalados corretamente

## Critérios de aceite
- Pacote `Microsoft.EntityFrameworkCore` adicionado ao `.csproj`
- Pacote `Npgsql.EntityFrameworkCore.PostgreSQL` adicionado ao `.csproj`
- Versões dos pacotes são compatíveis com .NET 8
- Projeto referencia `FastFood.KitchenFlow.Domain` (para usar enums)
- `dotnet restore` executa sem erros
- `dotnet build` compila sem erros
- Versões alinhadas com o projeto de referência (Auth)
