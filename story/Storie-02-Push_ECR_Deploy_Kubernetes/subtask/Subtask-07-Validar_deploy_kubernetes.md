# Subtask 07: Validar deploy no cluster Kubernetes

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Validar que o workflow de deploy no Kubernetes está funcionando corretamente, verificando que os deployments e jobs são atualizados no cluster EKS.

## Passos de implementação
- [ ] Executar workflow de deploy via GitHub Actions UI
- [ ] Verificar logs do workflow para garantir execução sem erros
- [ ] Executar `kubectl get deployment -n <namespace>` para verificar deployment da API
- [ ] Verificar que a imagem do deployment foi atualizada com a nova tag
- [ ] Verificar que pods estão rodando corretamente após deploy
- [ ] Executar `kubectl get pods -n <namespace>` para verificar status dos pods
- [ ] Verificar logs dos pods para garantir que não há erros de inicialização
- [ ] Testar acesso à API após deploy (se aplicável)
- [ ] Verificar que job do Migrator foi atualizado (se aplicável)
- [ ] Validar rollback em caso de falha (se implementado)

## Como testar
- Executar `kubectl describe deployment <nome-deployment> -n <namespace>` e verificar imagem
- Executar `kubectl get pods -n <namespace> -l app=<nome-app>` e verificar status
- Executar `kubectl logs <pod-name> -n <namespace>` e verificar logs
- Executar `kubectl rollout status deployment/<nome-deployment> -n <namespace>` e verificar rollout
- Testar endpoint da API após deploy (ex: `curl http://<endpoint>/api/health`)
- Verificar eventos do deployment com `kubectl get events -n <namespace>`
- Verificar que imagem do deployment corresponde ao commit SHA usado

## Critérios de aceite
- [ ] Workflow de deploy executa sem erros
- [ ] Deployment da API é atualizado no cluster Kubernetes
- [ ] Imagem do deployment corresponde ao commit SHA usado
- [ ] Pods da API estão em status Running após deploy
- [ ] API responde corretamente após deploy
- [ ] Logs dos pods não mostram erros críticos
- [ ] Rollout do deployment foi concluído com sucesso
- [ ] Job do Migrator foi atualizado (quando necessário)




