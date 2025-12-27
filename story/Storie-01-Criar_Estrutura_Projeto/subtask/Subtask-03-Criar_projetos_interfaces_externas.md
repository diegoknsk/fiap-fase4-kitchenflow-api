# Subtask 03: Criar projetos InterfacesExternas (Api, Migrator)

## Descrição
Criar os 2 projetos de Interfaces Externas: API HTTP (ASP.NET Core) e Migrator (Console Application) dentro de `src/InterfacesExternas/`. Estes projetos são os pontos de entrada da aplicação.

## Passos de implementação
- Criar projeto `FastFood.KitchenFlow.Api` (Web API) em `src/InterfacesExternas/FastFood.KitchenFlow.Api/`
  - Usar template `dotnet new webapi`
  - Configurar para .NET 8
- Criar projeto `FastFood.KitchenFlow.Migrator` (Console Application) em `src/InterfacesExternas/FastFood.KitchenFlow.Migrator/`
  - Usar template `dotnet new console`
  - Configurar para .NET 8
- Adicionar ambos os projetos à solução usando `dotnet sln add`
- Remover arquivos de exemplo desnecessários (WeatherForecast, etc.) da API

## Como testar
- Executar `dotnet build` em cada projeto individualmente
- Executar `dotnet sln list` para verificar que os projetos foram adicionados
- Verificar que a API tem `Program.cs` e `appsettings.json`
- Verificar que o Migrator tem `Program.cs`

## Critérios de aceite
- Projeto Api criado como Web API em `src/InterfacesExternas/FastFood.KitchenFlow.Api/`
- Projeto Migrator criado como Console App em `src/InterfacesExternas/FastFood.KitchenFlow.Migrator/`
- Ambos configurados com .NET 8
- Ambos adicionados à solução
- Arquivos de exemplo removidos da API
- Projetos compilam individualmente sem erros




