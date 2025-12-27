# Subtask 06: Criar estrutura básica de pastas em cada projeto

## Descrição
Criar a estrutura de pastas em cada projeto conforme definido no `kitchenflow-context.mdc`, preparando a organização para futuras implementações. Esta subtask estabelece a organização interna de cada camada.

## Passos de implementação
- **Domain:**
  - Criar pasta `Entities/`
  - Criar pasta `Common/` (para Exceções, Value Objects, Enums)
- **Application:**
  - Criar pastas: `Commands/`, `UseCases/`, `InputModels/`, `OutputModels/`, `Responses/`, `Presenters/`, `Ports/`
- **Infra:**
  - Criar pasta `Services/`
  - Criar pasta `BackgroundServices/`
- **Infra.Persistence:**
  - Criar pastas: `Repositories/`, `Entities/`, `Configurations/`, `DbContext/`, `Migrations/`
- **CrossCutting:**
  - Criar pasta `Extensions/`
- **Api:**
  - Criar pasta `Controllers/`
  - Manter `Program.cs` e `appsettings.json` na raiz
- **Migrator:**
  - Manter `Program.cs` na raiz (estrutura será definida em stories futuras)
- **Tests.Unit:**
  - Criar estrutura básica de pastas (será expandida conforme necessário)
- **Tests.Bdd:**
  - Criar estrutura básica de pastas (será expandida conforme necessário)

## Como testar
- Verificar que todas as pastas foram criadas usando `dir` ou `ls`
- Verificar que a estrutura está alinhada com o projeto de referência
- Executar `dotnet build` para garantir que não quebrou nada

## Critérios de aceite
- Todas as pastas criadas conforme `kitchenflow-context.mdc`
- Estrutura de pastas alinhada com projeto de referência
- Pastas criadas mesmo que vazias (preparação para futuras implementações)
- `dotnet build` ainda compila sem erros
- Estrutura organizada e pronta para receber código




