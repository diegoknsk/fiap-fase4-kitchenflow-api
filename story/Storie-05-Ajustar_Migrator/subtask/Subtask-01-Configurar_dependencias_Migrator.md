# Subtask 01: Configurar dependências do Migrator

## Descrição
Verificar e configurar as dependências necessárias no projeto `FastFood.KitchenFlow.Migrator` para suportar a execução de migrations do Entity Framework Core.

## Passos de implementação
- Abrir o arquivo `.csproj` do projeto `FastFood.KitchenFlow.Migrator`
- Verificar que o projeto referencia:
  - `FastFood.KitchenFlow.Infra.Persistence` (necessário para acessar DbContext e migrations)
- Adicionar pacote NuGet se necessário:
  - `Microsoft.Extensions.Configuration` (para carregar appsettings.json)
  - `Microsoft.Extensions.Configuration.Json` (para ler arquivos JSON)
  - `Microsoft.Extensions.Configuration.EnvironmentVariables` (para ler variáveis de ambiente)
- Verificar que o projeto é uma Console Application (.NET 8)
- Restaurar pacotes: `dotnet restore`

## Referências
- **Auth**: Verificar dependências do projeto `FastFood.Auth.Migrator`

## Como testar
- Executar `dotnet restore` no projeto
- Executar `dotnet build` para verificar que compila
- Verificar que as referências estão corretas

## Critérios de aceite
- Projeto referencia `FastFood.KitchenFlow.Infra.Persistence`
- Pacotes de configuração adicionados (se necessário):
  - `Microsoft.Extensions.Configuration`
  - `Microsoft.Extensions.Configuration.Json`
  - `Microsoft.Extensions.Configuration.EnvironmentVariables`
- Projeto é Console Application (.NET 8)
- `dotnet restore` executa sem erros
- `dotnet build` compila sem erros
- Dependências alinhadas com o projeto Auth
