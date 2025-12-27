# Subtask 03: Criar workflow GitHub Actions para push no ECR

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Criar workflow GitHub Actions que automatiza o build e push das imagens Docker (API e Migrator) para o Amazon ECR, usando commit SHA como tag para segurança.

## Passos de implementação
- [ ] Criar diretório `.github/workflows/` se não existir
- [ ] Criar arquivo `.github/workflows/push-ecr.yml`
- [ ] Configurar trigger `workflow_dispatch` para execução manual
- [ ] Adicionar step de checkout do código
- [ ] Configurar credenciais AWS usando `aws-actions/configure-aws-credentials` (usar commit SHA, não tag)
- [ ] Fazer login no ECR usando `aws-actions/amazon-ecr-login` (usar commit SHA, não tag)
- [ ] Obter commit SHA usando `${{ github.sha }}`
- [ ] Configurar variáveis para URLs dos repositórios ECR (API e Migrator)
- [ ] Adicionar step para build da imagem da API
- [ ] Adicionar step para tag da imagem da API com commit SHA
- [ ] Adicionar step para push da imagem da API para ECR
- [ ] Adicionar step para build da imagem do Migrator
- [ ] Adicionar step para tag da imagem do Migrator com commit SHA
- [ ] Adicionar step para push da imagem do Migrator para ECR
- [ ] Adicionar tratamento de erros e mensagens informativas

## Como testar
- Executar workflow manualmente via GitHub Actions UI
- Verificar que todos os steps executam com sucesso
- Verificar no console AWS ECR que as imagens foram criadas
- Verificar que as tags das imagens correspondem ao commit SHA
- Verificar logs do workflow para garantir que não há erros
- Testar com diferentes branches para validar comportamento

## Critérios de aceite
- [ ] Workflow criado em `.github/workflows/push-ecr.yml`
- [ ] Workflow executa com sucesso quando acionado manualmente
- [ ] Ambas as imagens (API e Migrator) são construídas corretamente
- [ ] Ambas as imagens são enviadas para repositórios ECR separados
- [ ] Tags das imagens usam commit SHA (não tags de versão)
- [ ] Workflow usa commit SHA das actions do GitHub (seguindo padrão de segurança)
- [ ] Secrets do GitHub são referenciadas corretamente
- [ ] Workflow tem mensagens de log claras e informativas




