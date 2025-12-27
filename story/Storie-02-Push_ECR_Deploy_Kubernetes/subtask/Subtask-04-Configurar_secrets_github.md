# Subtask 04: Configurar secrets e variáveis no GitHub

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Configurar todas as secrets e variáveis necessárias no GitHub para que os workflows possam autenticar na AWS e acessar os recursos ECR e EKS.

## Passos de implementação
- [ ] Acessar Settings → Secrets and variables → Actions no repositório GitHub
- [ ] Criar secret `AWS_ACCESS_KEY_ID` com a chave de acesso AWS
- [ ] Criar secret `AWS_SECRET_ACCESS_KEY` com a chave secreta AWS
- [ ] Criar secret `AWS_REGION` com a região AWS (ex: us-east-1)
- [ ] Criar secret `AWS_SESSION_TOKEN` (se necessário para credenciais temporárias)
- [ ] Criar secret `ECR_REPOSITORY_API` com a URL completa do repositório ECR da API
- [ ] Criar secret `ECR_REPOSITORY_MIGRATOR` com a URL completa do repositório ECR do Migrator
- [ ] Criar secret `EKS_CLUSTER_NAME` com o nome do cluster EKS
- [ ] Verificar que todas as secrets estão configuradas corretamente (sem espaços extras)
- [ ] Documentar quais secrets são necessárias e como obtê-las

## Como testar
- Verificar na interface do GitHub que todas as secrets estão criadas
- Verificar que os nomes das secrets estão exatamente como referenciados nos workflows
- Executar workflow de push para ECR e verificar que autenticação funciona
- Verificar logs do workflow para garantir que não há erros de autenticação
- Testar acesso manual via AWS CLI usando as mesmas credenciais

## Critérios de aceite
- [ ] Todas as secrets necessárias estão configuradas no GitHub
- [ ] Secrets não contêm espaços extras ou caracteres inválidos
- [ ] Nomes das secrets correspondem exatamente aos usados nos workflows
- [ ] Workflow de push para ECR autentica corretamente na AWS
- [ ] Documentação criada explicando como configurar cada secret
- [ ] Secrets seguem boas práticas de segurança (não commitadas no código)




