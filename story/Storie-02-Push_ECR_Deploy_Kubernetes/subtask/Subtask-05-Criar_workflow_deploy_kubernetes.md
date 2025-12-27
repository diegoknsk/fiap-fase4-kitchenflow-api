# Subtask 05: Criar workflow GitHub Actions para deploy no Kubernetes

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Criar workflow GitHub Actions que automatiza o deploy das imagens no cluster Kubernetes (EKS), atualizando os deployments da API e do job do Migrator.

## Passos de implementação
- [ ] Criar arquivo `.github/workflows/deploy-kubernetes.yml`
- [ ] Configurar trigger `workflow_dispatch` para execução manual
- [ ] Adicionar step de checkout do código
- [ ] Configurar credenciais AWS usando `aws-actions/configure-aws-credentials` (usar commit SHA)
- [ ] Configurar kubectl para acesso ao cluster EKS
- [ ] Obter commit SHA para identificar a imagem correta
- [ ] Adicionar step para atualizar deployment da API com nova imagem
- [ ] Adicionar step para atualizar job do Migrator (se necessário) com nova imagem
- [ ] Adicionar step para verificar status do deployment após atualização
- [ ] Adicionar step para verificar health check da API (se aplicável)
- [ ] Adicionar tratamento de erros e rollback em caso de falha
- [ ] Adicionar mensagens informativas sobre o progresso do deploy

## Como testar
- Executar workflow manualmente via GitHub Actions UI
- Verificar que todos os steps executam com sucesso
- Verificar no cluster Kubernetes que o deployment foi atualizado
- Verificar que a imagem do deployment corresponde ao commit SHA usado
- Verificar logs do workflow para garantir que não há erros
- Testar rollback em caso de falha (se implementado)
- Verificar que pods da API estão rodando corretamente após deploy

## Critérios de aceite
- [ ] Workflow criado em `.github/workflows/deploy-kubernetes.yml`
- [ ] Workflow executa com sucesso quando acionado manualmente
- [ ] Deployment da API é atualizado no cluster Kubernetes
- [ ] Job do Migrator é atualizado (quando necessário)
- [ ] Workflow usa commit SHA para identificar a imagem correta
- [ ] Workflow valida que o deployment foi atualizado com sucesso
- [ ] Workflow tem tratamento de erros adequado
- [ ] Workflow tem mensagens de log claras e informativas




