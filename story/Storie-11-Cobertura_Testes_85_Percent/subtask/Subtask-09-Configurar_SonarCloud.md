# Subtask 09: Configurar SonarCloud

## Descrição
Configurar o projeto no SonarCloud, incluindo criação do projeto, configuração do Quality Gate com cobertura mínima de 85%, desabilitação da Análise Automática e configuração do token como secret no GitHub.

## Passos de implementação
- Criar projeto no SonarCloud
- Configurar Quality Gate com cobertura mínima de 85%
- Desabilitar Análise Automática (CRÍTICO)
- Gerar token do SonarCloud
- Configurar token como secret no GitHub (`SONAR_TOKEN`)
- Validar configuração através de Pull Request

## Criar projeto no SonarCloud

### Passos:
1. Acessar https://sonarcloud.io
2. Fazer login com GitHub
3. Clicar em "Create Project"
4. Selecionar organização (ex: `diegoknsk`)
5. Selecionar repositório (ex: `fiap-fase4-kitchenflow-api`)
6. Configurar:
   - **Project Key**: `{ORGANIZACAO}_{PROJETO}` (ex: `diegoknsk_fiap-fase4-kitchenflow-api`)
   - **Project Name**: `fiap-fase4-kitchenflow-api`
   - **Main Branch**: `main`

## Configurar Quality Gate

### Passos:
1. Acessar SonarCloud → Quality Gates
2. Criar novo Quality Gate ou editar existente
3. Configurar regras:
   - **Coverage**: >= 85%
   - **Duplicated Lines**: <= 3%
   - **Maintainability Rating**: A
   - **Reliability Rating**: A
   - **Security Rating**: A
4. Aplicar Quality Gate ao projeto

### Configuração mínima:
- **Coverage on New Code**: >= 85%
- **Overall Coverage**: >= 85%
- **Bugs**: 0
- **Vulnerabilities**: 0
- **Code Smells**: <= 10

## Desabilitar Análise Automática (CRÍTICO)

### ⚠️ IMPORTANTE: Este passo é OBRIGATÓRIO

**Erro comum**: `ERROR: You are running CI analysis while Automatic Analysis is enabled.`

### Passos:
1. Acessar SonarCloud → Projeto
2. Ir em **Administration** → **Analysis Method**
3. **Desativar** "Automatic Analysis"
4. Salvar alterações

**Por quê?**: Análise automática e CI/CD são mutuamente exclusivas. Se a Análise Automática estiver habilitada, o workflow do GitHub Actions falhará.

## Gerar token do SonarCloud

### Passos:
1. Acessar SonarCloud → My Account → Security
2. Clicar em "Generate Token"
3. Configurar:
   - **Name**: `GitHub Actions - KitchenFlow`
   - **Type**: `User Token`
   - **Expires**: Nunca (ou data futura)
4. Copiar o token gerado (será usado no próximo passo)

## Configurar token como secret no GitHub

### Passos:
1. Acessar repositório no GitHub
2. Ir em **Settings** → **Secrets and variables** → **Actions**
3. Clicar em "New repository secret"
4. Configurar:
   - **Name**: `SONAR_TOKEN`
   - **Secret**: Colar o token do SonarCloud
5. Salvar secret

## Validar configuração

### Passos:
1. Criar Pull Request de teste
2. Verificar que o workflow `quality.yml` executa
3. Verificar que o SonarCloud recebe os dados
4. Verificar que a cobertura aparece no SonarCloud
5. Verificar que o Quality Gate passa (se cobertura >= 85%)

## Configurações adicionais (opcional)

### Exclusões de cobertura:
- Já configuradas no workflow: `**/*Program.cs,**/*Startup.cs,**/Migrations/**,**/*Dto.cs`

### Análise de Pull Requests:
- Já configurada no workflow para análise de PRs
- Comentários automáticos em PRs com resultados do SonarCloud

## Observações importantes
- **Análise Automática**: DEVE estar desabilitada (CRÍTICO)
- **Token**: Manter seguro, não commitar no código
- **Quality Gate**: Configurar cobertura mínima de 85%
- **Project Key**: Deve corresponder ao configurado no workflow

## Como testar
- Criar Pull Request de teste
- Verificar que o workflow executa
- Verificar que o SonarCloud recebe os dados
- Verificar que a cobertura aparece no SonarCloud
- Verificar que o Quality Gate passa quando cobertura >= 85%
- Verificar que o Quality Gate bloqueia quando cobertura < 85%

## Critérios de aceite
- Projeto criado no SonarCloud
- Project Key configurado corretamente
- Quality Gate configurado com cobertura mínima de 85%
- Análise Automática desabilitada (CRÍTICO)
- Token gerado no SonarCloud
- Token configurado como secret no GitHub (`SONAR_TOKEN`)
- Workflow executa com sucesso
- Cobertura aparece no SonarCloud
- Quality Gate passa quando cobertura >= 85%
- Quality Gate bloqueia quando cobertura < 85%
