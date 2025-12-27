# Subtask 01: Criar Dockerfile para a API

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Criar Dockerfile multi-stage para a API do KitchenFlow, otimizado para produção com build em estágio separado e imagem final leve baseada em aspnet runtime.

## Passos de implementação
- [ ] Criar arquivo `Dockerfile` na raiz do projeto ou em `src/InterfacesExternas/FastFood.KitchenFlow.Api/`
- [ ] Configurar estágio de build usando `mcr.microsoft.com/dotnet/sdk:8.0`
- [ ] Configurar WORKDIR e copiar arquivos do projeto
- [ ] Executar `dotnet restore` para restaurar dependências
- [ ] Executar `dotnet publish` com configurações de Release
- [ ] Configurar estágio de runtime usando `mcr.microsoft.com/dotnet/aspnet:8.0`
- [ ] Copiar arquivos publicados do estágio de build
- [ ] Configurar variáveis de ambiente (ASPNETCORE_URLS, ASPNETCORE_ENVIRONMENT)
- [ ] Expor porta 80
- [ ] Configurar ENTRYPOINT para executar a aplicação

## Como testar
- Executar `docker build -t kitchenflow-api:test .` (ou com caminho correto do Dockerfile)
- Verificar que o build completa sem erros
- Executar `docker run -p 8080:80 kitchenflow-api:test` e verificar que a API inicia
- Acessar `http://localhost:8080/api/health` (ou rota de health check) e verificar resposta
- Verificar tamanho da imagem com `docker images kitchenflow-api:test` (deve ser otimizado)

## Critérios de aceite
- [ ] Dockerfile criado e funcional
- [ ] Build da imagem completa sem erros
- [ ] Imagem executa corretamente quando rodada em container
- [ ] API responde corretamente nas rotas configuradas
- [ ] Imagem usa multi-stage build para otimização
- [ ] Imagem final baseada em `aspnet:8.0` (runtime, não SDK)


