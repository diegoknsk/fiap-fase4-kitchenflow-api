# Storie-02: Push para ECR e Deploy no Kubernetes

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Como desenvolvedor DevOps, quero configurar pipelines de CI/CD para fazer push das imagens Docker (API e Migrator) para o Amazon ECR e realizar o deploy automático no cluster Kubernetes (EKS), para que as atualizações da aplicação sejam implantadas de forma automatizada e confiável.

## Objetivo
Criar workflows GitHub Actions para automatizar o build e push das imagens Docker da API e do Migrator para o Amazon ECR, e implementar o deploy automático dessas imagens no cluster Kubernetes (EKS), garantindo que ambas as aplicações sejam atualizadas de forma consistente e segura.

## Escopo Técnico
- Tecnologias: GitHub Actions, Docker, Amazon ECR, Kubernetes, kubectl, AWS CLI
- Arquivos afetados:
  - `.github/workflows/push-ecr.yml` (workflow para push no ECR)
  - `.github/workflows/deploy-kubernetes.yml` (workflow para deploy no EKS)
  - `Dockerfile` (na raiz ou em `src/InterfacesExternas/FastFood.KitchenFlow.Api/`)
  - `Dockerfile.migrator` (na raiz ou em `src/InterfacesExternas/FastFood.KitchenFlow.Migrator/`)
  - `k8s/app/api/deployment.yaml` (se necessário criar)
  - `k8s/app/migrator/job.yaml` (se necessário criar)
- Recursos AWS: Amazon ECR (repositórios de imagens), EKS (cluster Kubernetes)
- Secrets GitHub necessárias: `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_REGION`, `ECR_REPOSITORY_API`, `ECR_REPOSITORY_MIGRATOR`, `EKS_CLUSTER_NAME`

## Subtasks

- [ ] [Subtask 01: Criar Dockerfile para a API](./subtask/Subtask-01-Criar_Dockerfile_API.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 02: Criar Dockerfile para o Migrator](./subtask/Subtask-02-Criar_Dockerfile_Migrator.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 03: Criar workflow GitHub Actions para push no ECR](./subtask/Subtask-03-Criar_workflow_push_ECR.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 04: Configurar secrets e variáveis no GitHub](./subtask/Subtask-04-Configurar_secrets_github.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 05: Criar workflow GitHub Actions para deploy no Kubernetes](./subtask/Subtask-05-Criar_workflow_deploy_kubernetes.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 06: Validar push de imagens no ECR](./subtask/Subtask-06-Validar_push_imagens_ECR.md) - *Data de Conclusão: [DD/MM/AAAA]*
- [ ] [Subtask 07: Validar deploy no cluster Kubernetes](./subtask/Subtask-07-Validar_deploy_kubernetes.md) - *Data de Conclusão: [DD/MM/AAAA]*

## Critérios de Aceite da História

- [ ] Dockerfile da API criado e funcional, buildando imagem .NET 8 corretamente
- [ ] Dockerfile do Migrator criado e funcional, incluindo migrations do EF Core
- [ ] Workflow de push para ECR criado em `.github/workflows/push-ecr.yml`
- [ ] Workflow faz push de ambas as imagens (API e Migrator) para repositórios ECR separados
- [ ] Workflow usa commit SHA como tag da imagem (seguindo padrão de segurança)
- [ ] Workflow de deploy no Kubernetes criado em `.github/workflows/deploy-kubernetes.yml`
- [ ] Workflow de deploy atualiza o deployment da API no cluster EKS
- [ ] Workflow de deploy atualiza o job do Migrator no cluster EKS (quando necessário)
- [ ] Secrets do GitHub configuradas corretamente (AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, AWS_REGION, etc.)
- [ ] Workflows executam com sucesso quando acionados manualmente (workflow_dispatch)
- [ ] Imagens aparecem corretamente no Amazon ECR após push
- [ ] Deployment da API é atualizado no cluster Kubernetes após deploy
- [ ] Documentação de configuração criada explicando como configurar secrets e executar workflows
- [ ] Workflows seguem padrão de segurança usando commit SHA ao invés de tags de versão
- [ ] Workflows validam que build, testes e análise Sonar foram bem-sucedidos antes de push (se aplicável)




