# Storie-05: Ajustar Projeto Migrator para Executar Migrations no Postgres

## Descrição
Como desenvolvedor, quero ajustar o projeto `FastFood.KitchenFlow.Migrator` para executar as migrations do Entity Framework Core no banco de dados PostgreSQL, seguindo o padrão do projeto Auth, para que possamos aplicar as migrations de forma automatizada (via Docker/Kubernetes ou manualmente).

## Objetivo
Ajustar o projeto `FastFood.KitchenFlow.Migrator` (Console Application) para:
- Carregar configuração de connection string de `appsettings.json` e variáveis de ambiente
- Configurar `KitchenFlowDbContext` com connection string
- Executar migrations pendentes automaticamente
- Exibir informações sobre migrations aplicadas e pendentes
- Tratar erros adequadamente com mensagens claras
- Seguir padrão do projeto Auth

## Escopo Técnico
- **Tecnologias**: .NET 8, Entity Framework Core, Npgsql
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Migrator` (Program.cs)
  - `FastFood.KitchenFlow.Infra.Persistence` (DbContext já criado)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\InterfacesExternas\FastFood.Auth.Migrator\Program.cs` (padrão completo)
  - Seguir estrutura e lógica do Auth Migrator
- **Banco de dados**: PostgreSQL local
  - Host: localhost
  - Porta: 4433
  - Database: dbKitchenLocal
  - User: postgres
  - Password: postgres

## Subtasks

- [x] [Subtask 01: Configurar dependências do Migrator](./subtask/Subtask-01-Configurar_dependencias_Migrator.md)
- [x] [Subtask 02: Implementar Program.cs do Migrator](./subtask/Subtask-02-Implementar_Program_Migrator.md)
- [x] [Subtask 03: Configurar appsettings.json do Migrator](./subtask/Subtask-03-Configurar_appsettings_Migrator.md)
- [x] [Subtask 04: Testar execução de migrations](./subtask/Subtask-04-Testar_execucao_migrations.md)
- [x] [Subtask 05: Validar Migrator completo](./subtask/Subtask-05-Validar_Migrator.md)

## Critérios de Aceite da História

- [x] Projeto `FastFood.KitchenFlow.Migrator` referencia:
  - `FastFood.KitchenFlow.Infra.Persistence`
  - `Microsoft.Extensions.Configuration` (adicionado)
  - `Microsoft.Extensions.Configuration.Json` (adicionado)
  - `Microsoft.Extensions.Configuration.EnvironmentVariables` (adicionado)
- [x] `Program.cs` do Migrator implementado:
  - Carrega configuração de `appsettings.json` e variáveis de ambiente
  - Prioriza variável de ambiente `ConnectionStrings__DefaultConnection`
  - Configura `KitchenFlowDbContext` com connection string
  - Verifica migrations pendentes
  - Aplica migrations automaticamente
  - Exibe informações sobre migrations aplicadas
  - Trata erros com mensagens claras
- [x] `appsettings.json` do Migrator configurado:
  - Connection string para PostgreSQL local
- [x] Migrator executa migrations corretamente:
  - Aplica migration inicial (`InitialCreate`)
  - Cria tabelas `Preparations` e `Deliveries` no banco
  - Exibe mensagens informativas durante execução
- [x] Migrator trata erros adequadamente:
  - Connection string não encontrada
  - Erro de conexão com banco
  - Erro ao aplicar migrations
- [x] Migrator segue padrão do Auth:
  - Estrutura similar ao `Program.cs` do Auth
  - Mensagens informativas
  - Tratamento de erros robusto
- [x] Projeto compila e executa sem erros
- [x] Migrations são aplicadas corretamente no banco PostgreSQL (quando executado)

## Observações Arquiteturais

### Padrão do Migrator
- **Console Application**: Aplicativo console simples que executa migrations
- **Configuração flexível**: Suporta variáveis de ambiente e arquivos de configuração
- **Prioridade**: Variáveis de ambiente > appsettings.Development.json > appsettings.json
- **Execução automática**: Aplica todas as migrations pendentes automaticamente
- **Informações claras**: Exibe mensagens informativas sobre o processo

### Uso do Migrator
- **Desenvolvimento local**: Executar manualmente via `dotnet run`
- **Docker**: Executar como Job no Kubernetes antes do deployment da API
- **CI/CD**: Executar como step no pipeline de deploy

### Análise do Projeto de Referência

**Auth (Program.cs do Migrator):**
- Carrega configuração com prioridade: variáveis de ambiente > appsettings.Development.json > appsettings.json
- Extrai connection string com fallback
- Configura DbContext com `UseNpgsql`
- Verifica migrations pendentes
- Aplica migrations com `MigrateAsync()`
- Exibe informações detalhadas sobre migrations
- Trata erros com mensagens claras e exit codes apropriados

**Aplicação no KitchenFlow:**
- Seguir estrutura idêntica ao Auth
- Adaptar para `KitchenFlowDbContext`
- Manter mesma lógica de configuração e execução
- Garantir compatibilidade com Docker/Kubernetes

### Configuração de Connection String

O Migrator deve suportar múltiplas formas de configuração:

1. **Variável de ambiente** (prioridade máxima):
   ```bash
   ConnectionStrings__DefaultConnection="Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
   ```

2. **appsettings.Development.json** (não commitado):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
     }
   }
   ```

3. **appsettings.json** (fallback):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": ""
     }
   }
   ```

### Exit Codes
- **0**: Sucesso
- **1**: Erro (connection string não encontrada, erro ao aplicar migrations, etc.)
