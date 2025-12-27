# Subtask 02: Criar Dockerfile para o Migrator

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Criar Dockerfile multi-stage para o Migrator do KitchenFlow, incluindo migrations do Entity Framework Core e arquivos de configuração necessários para execução.

## Passos de implementação
- [ ] Criar arquivo `Dockerfile.migrator` na raiz do projeto ou em `src/InterfacesExternas/FastFood.KitchenFlow.Migrator/`
- [ ] Configurar estágio de build usando `mcr.microsoft.com/dotnet/sdk:8.0`
- [ ] Configurar WORKDIR e copiar arquivos do projeto
- [ ] Executar `dotnet restore` para o projeto Migrator
- [ ] Executar `dotnet publish` com configurações de Release
- [ ] Copiar migrations do projeto `FastFood.KitchenFlow.Infra.Persistence`
- [ ] Configurar estágio de runtime usando `mcr.microsoft.com/dotnet/aspnet:8.0`
- [ ] Copiar arquivos publicados do estágio de build
- [ ] Copiar arquivo `appsettings.json` do Migrator
- [ ] Copiar pasta de migrations para local acessível
- [ ] Configurar ENTRYPOINT para executar o Migrator

## Como testar
- Executar `docker build -f Dockerfile.migrator -t kitchenflow-migrator:test .`
- Verificar que o build completa sem erros
- Verificar que migrations estão presentes na imagem com `docker run --entrypoint ls kitchenflow-migrator:test /app/Migrations`
- Executar `docker run kitchenflow-migrator:test` (sem conexão de banco, deve falhar graciosamente com erro de conexão, não erro de arquivo)
- Verificar que appsettings.json está presente na imagem

## Critérios de aceite
- [ ] Dockerfile.migrator criado e funcional
- [ ] Build da imagem completa sem erros
- [ ] Migrations do EF Core estão presentes na imagem
- [ ] Arquivo appsettings.json está presente na imagem
- [ ] Imagem usa multi-stage build para otimização
- [ ] Imagem final baseada em `aspnet:8.0` (runtime, não SDK)




