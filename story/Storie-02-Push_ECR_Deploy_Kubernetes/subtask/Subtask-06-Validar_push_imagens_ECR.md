# Subtask 06: Validar push de imagens no ECR

## Status
- **Estado:** 🔄 Em desenvolvimento
- **Data de Conclusão:** [DD/MM/AAAA]

## Descrição
Validar que o workflow de push para ECR está funcionando corretamente, verificando que as imagens aparecem no Amazon ECR com as tags corretas.

## Passos de implementação
- [ ] Executar workflow de push para ECR via GitHub Actions UI
- [ ] Verificar logs do workflow para garantir execução sem erros
- [ ] Acessar console AWS ECR e verificar repositório da API
- [ ] Verificar que imagem da API está presente no repositório
- [ ] Verificar que tag da imagem corresponde ao commit SHA usado
- [ ] Acessar console AWS ECR e verificar repositório do Migrator
- [ ] Verificar que imagem do Migrator está presente no repositório
- [ ] Verificar que tag da imagem corresponde ao commit SHA usado
- [ ] Validar tamanho e metadata das imagens no ECR
- [ ] Testar pull das imagens localmente para garantir que estão acessíveis

## Como testar
- Executar `aws ecr describe-images --repository-name <nome-repositorio-api> --region <regiao>` e verificar imagens
- Executar `aws ecr describe-images --repository-name <nome-repositorio-migrator> --region <regiao>` e verificar imagens
- Verificar tags das imagens correspondem ao commit SHA do workflow
- Executar `docker pull <ecr-url>/<repositorio>:<tag>` localmente (após login ECR)
- Verificar que imagens podem ser executadas localmente após pull
- Verificar logs do workflow para garantir que não há warnings ou erros

## Critérios de aceite
- [ ] Workflow de push executa sem erros
- [ ] Imagem da API está presente no repositório ECR correto
- [ ] Imagem do Migrator está presente no repositório ECR correto
- [ ] Tags das imagens correspondem ao commit SHA usado no workflow
- [ ] Imagens podem ser baixadas (pull) do ECR
- [ ] Imagens podem ser executadas localmente após pull
- [ ] Metadata das imagens está correto (tamanho, data de criação, etc.)




