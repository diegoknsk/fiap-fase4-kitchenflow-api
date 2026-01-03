# Subtask 03: Configurar appsettings.json do Migrator

## Descrição
Configurar o arquivo `appsettings.json` do projeto Migrator com a connection string para PostgreSQL local, seguindo o padrão do projeto Auth.

## Passos de implementação
- Abrir `appsettings.json` do projeto `FastFood.KitchenFlow.Migrator`
- Configurar connection string:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres"
    }
  }
  ```
- Verificar que o arquivo está configurado corretamente
- (Opcional) Criar `appsettings.Development.json` com mesma connection string (não commitado)

## Referências
- **Auth**: `c:\Projetos\Fiap\fiap-fase4-auth-lambda\src\InterfacesExternas\FastFood.Auth.Migrator\appsettings.json`
- Seguir padrão do Auth (connection string vazia por padrão, preenchida em Development)

## Observações importantes
- **Connection string vazia por padrão**: O `appsettings.json` pode ter connection string vazia, sendo preenchida via variáveis de ambiente ou `appsettings.Development.json`
- **Segurança**: Não commitar senhas em `appsettings.json` (usar variáveis de ambiente em produção)

## Como testar
- Verificar que arquivo JSON é válido
- Verificar que connection string está no formato correto
- Testar leitura da connection string pelo Migrator

## Critérios de aceite
- `appsettings.json` configurado com:
  - Seção `ConnectionStrings`
  - Chave `DefaultConnection` com connection string para PostgreSQL local
- Connection string no formato correto:
  - `Host=localhost;Port=4433;Database=dbKitchenLocal;Username=postgres;Password=postgres`
- Arquivo JSON é válido
- (Opcional) `appsettings.Development.json` criado com mesma connection string
- Estrutura segue padrão do Auth
