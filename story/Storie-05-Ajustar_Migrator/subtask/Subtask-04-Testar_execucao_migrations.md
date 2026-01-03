# Subtask 04: Testar execução de migrations

## Descrição
Testar a execução do Migrator para garantir que as migrations são aplicadas corretamente no banco de dados PostgreSQL.

## Passos de implementação
- Verificar que o banco PostgreSQL está rodando:
  - Host: localhost
  - Porta: 4433
  - Database: dbKitchenLocal (pode não existir ainda, será criado)
- Executar o Migrator:
  ```bash
  dotnet run --project src/InterfacesExternas/FastFood.KitchenFlow.Migrator
  ```
- Verificar saída do Migrator:
  - Mensagem de início
  - Connection string encontrada
  - Lista de migrations pendentes (se houver)
  - Mensagem de aplicação de migrations
  - Lista de migrations aplicadas
  - Mensagem de sucesso
- Verificar no banco de dados:
  - Tabela `Preparations` foi criada
  - Tabela `Deliveries` foi criada
  - FK `Delivery.PreparationId` → `Preparation.Id` foi criada
  - Coluna `OrderSnapshot` é do tipo `jsonb`
  - Índices foram criados
  - Tabela `__EFMigrationsHistory` foi criada (controle de migrations)
- Executar novamente o Migrator:
  - Deve exibir "Nenhuma migration pendente"
  - Deve listar migrations aplicadas
- Testar cenário de erro:
  - Executar com connection string inválida
  - Verificar mensagem de erro clara
  - Verificar exit code 1

## Como testar
- **Execução normal**: Executar Migrator e verificar que migrations são aplicadas
- **Execução sem migrations pendentes**: Executar novamente e verificar mensagem
- **Cenário de erro**: Testar com connection string inválida
- **Verificação no banco**: Conectar no PostgreSQL e verificar tabelas criadas

## Critérios de aceite
- Migrator executa sem erros:
  - Carrega connection string corretamente
  - Aplica migration inicial
  - Exibe mensagens informativas
- Tabelas criadas no banco:
  - `Preparations` com todas as colunas
  - `Deliveries` com todas as colunas
  - FK `Delivery.PreparationId` → `Preparation.Id` criada
  - `OrderSnapshot` é do tipo `jsonb`
  - Índices criados
- Execução subsequente:
  - Exibe "Nenhuma migration pendente"
  - Lista migrations aplicadas
- Tratamento de erros:
  - Mensagens claras em caso de erro
  - Exit code 1 em caso de erro
- Migrator funciona corretamente em todos os cenários testados
