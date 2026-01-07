# Subtask 02: Implementar Program.cs do Migrator

## Descrição
Implementar a lógica completa no `Program.cs` do Migrator para carregar configuração, configurar DbContext e executar migrations automaticamente, seguindo o padrão do projeto Auth.

## Passos de implementação
- Abrir `Program.cs` do projeto `FastFood.KitchenFlow.Migrator`
- Substituir conteúdo atual pela implementação completa:
  - Adicionar `using` statements necessários:
    - `Microsoft.EntityFrameworkCore`
    - `Microsoft.Extensions.Configuration`
    - `FastFood.KitchenFlow.Infra.Persistence`
  - Implementar método `Main` assíncrono:
    - Carregar configuração com prioridade:
      - Variáveis de ambiente
      - `appsettings.Development.json` (opcional)
      - `appsettings.json` (fallback)
    - Extrair connection string:
      - Priorizar variável de ambiente `ConnectionStrings__DefaultConnection`
      - Fallback para `configuration.GetConnectionString("DefaultConnection")`
    - Validar connection string:
      - Se vazia, exibir mensagem de erro e sair com código 1
    - Configurar DbContext:
      - Criar `DbContextOptionsBuilder<KitchenFlowDbContext>`
      - Configurar com `UseNpgsql(connectionString)`
    - Criar instância do `KitchenFlowDbContext`
    - Verificar migrations pendentes:
      - `await context.Database.GetPendingMigrationsAsync()`
      - Exibir lista de migrations pendentes
    - Aplicar migrations:
      - Se houver migrations pendentes, aplicar com `await context.Database.MigrateAsync()`
      - Exibir mensagem de sucesso
    - Listar migrations aplicadas:
      - `await context.Database.GetAppliedMigrationsAsync()`
      - Exibir lista de migrations aplicadas
    - Tratar erros:
      - Try-catch para capturar exceções
      - Exibir mensagem de erro clara
      - Exibir stack trace (se necessário)
      - Sair com código 1 em caso de erro
- Adicionar método auxiliar `ExtractHostFromConnectionString` (opcional, para exibir host na mensagem)
- Adicionar documentação XML (opcional, mas recomendado)

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\InterfacesExternas\FastFood.Auth.Migrator\Program.cs`
- Seguir estrutura idêntica ao Auth, adaptando para `KitchenFlowDbContext`

## Como testar
- Executar `dotnet build` no projeto para verificar compilação
- Executar `dotnet run` no projeto (sem connection string configurada) para verificar mensagem de erro
- Configurar connection string e executar `dotnet run` para verificar execução de migrations
- Verificar que migrations são aplicadas no banco

## Critérios de aceite
- `Program.cs` implementado com:
  - Carregamento de configuração (variáveis de ambiente + appsettings)
  - Extração de connection string com prioridade correta
  - Validação de connection string
  - Configuração de `KitchenFlowDbContext`
  - Verificação de migrations pendentes
  - Aplicação de migrations
  - Listagem de migrations aplicadas
  - Tratamento de erros robusto
- Mensagens informativas exibidas durante execução
- Exit codes corretos (0 para sucesso, 1 para erro)
- Projeto compila sem erros
- Estrutura segue padrão do Auth (idêntica)
