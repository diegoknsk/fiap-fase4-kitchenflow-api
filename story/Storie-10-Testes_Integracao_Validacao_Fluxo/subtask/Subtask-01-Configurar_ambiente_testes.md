# Subtask 01: Configurar ambiente de testes de integração

## Descrição
Configurar o ambiente de testes de integração, incluindo banco de dados de teste, configuração de Testcontainers ou banco em memória, e setup/teardown de dados.

## Passos de implementação
- Decidir abordagem de banco de dados para testes:
  - **Opção 1**: Testcontainers (PostgreSQL em container Docker)
  - **Opção 2**: Banco em memória (se suportado pelo EF Core)
  - **Opção 3**: Banco PostgreSQL local de teste
  - **Recomendação**: Testcontainers (mais próximo do ambiente real)
- Se usar Testcontainers:
  - Adicionar pacote `Testcontainers.PostgreSql` ao projeto de testes
  - Criar classe base `IntegrationTestBase`:
    - Setup: Criar container PostgreSQL
    - Configurar DbContext com connection string do container
    - Teardown: Remover container
- Se usar banco local:
  - Configurar connection string de teste em `appsettings.Test.json`
  - Criar método de setup/teardown para limpar dados
- Criar classe helper para setup de dados de teste:
  - Métodos para criar Preparations de teste
  - Métodos para criar Deliveries de teste
  - Métodos para limpar dados
- Configurar xUnit:
  - `IClassFixture` ou `ICollectionFixture` para compartilhar contexto
  - Setup/Teardown de testes

## Referências
- **Testcontainers**: Documentação oficial do Testcontainers para .NET
- **xUnit**: Documentação de fixtures e setup/teardown

## Observações importantes
- **Isolamento**: Cada teste deve ser independente
- **Performance**: Testcontainers pode ser mais lento, mas mais realista
- **Limpeza**: Sempre limpar dados entre testes

## Como testar
- Executar `dotnet build` no projeto de testes
- Executar `dotnet test` para verificar que ambiente funciona
- Verificar que banco de teste é criado/limpo corretamente

## Critérios de aceite
- Ambiente de testes configurado:
  - Testcontainers ou banco de teste configurado
  - Connection string de teste configurada
  - Setup/Teardown implementado
- Classe base de testes criada:
  - `IntegrationTestBase` ou similar
  - Métodos de setup/teardown
- Helpers de dados de teste criados:
  - Métodos para criar dados de teste
  - Métodos para limpar dados
- Projeto de testes compila sem erros
- Ambiente está pronto para testes de integração
