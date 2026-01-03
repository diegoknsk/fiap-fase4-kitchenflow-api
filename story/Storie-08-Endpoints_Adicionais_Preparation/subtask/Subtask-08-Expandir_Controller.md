# Subtask 08: Expandir PreparationController

## Descrição
Expandir o `PreparationController` (criado na Story 06) com os três novos endpoints para listar, iniciar e finalizar preparações.

## Passos de implementação
- Abrir classe `PreparationController` em `Controllers/PreparationController.cs`
- Adicionar UseCases no construtor (via DI):
  - `GetPreparationsUseCase`
  - `StartPreparationUseCase`
  - `FinishPreparationUseCase`
- Adicionar endpoint `GET /api/preparations`:
  - Método `GetPreparations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? status = null)`:
    - Criar `GetPreparationsInputModel` com parâmetros
    - Chamar `getPreparationsUseCase.ExecuteAsync(inputModel)`
    - Retornar 200 OK com Response
- Adicionar endpoint `POST /api/preparations/{id}/start`:
  - Método `StartPreparation([FromRoute] Guid id)`:
    - Criar `StartPreparationInputModel` com id
    - Chamar `startPreparationUseCase.ExecuteAsync(inputModel)`
    - Retornar HTTP Response:
      - 200 OK com Response em caso de sucesso
      - 400 Bad Request se status inválido
      - 404 Not Found se preparation não existe
- Adicionar endpoint `POST /api/preparations/{id}/finish`:
  - Método `FinishPreparation([FromRoute] Guid id)`:
    - Criar `FinishPreparationInputModel` com id
    - Chamar `finishPreparationUseCase.ExecuteAsync(inputModel)`
    - Retornar HTTP Response:
      - 200 OK com Response em caso de sucesso
      - 400 Bad Request se status inválido
      - 404 Not Found se preparation não existe
- Verificar que endpoint existente `POST /api/preparations` (da Story 06) não foi alterado
- Adicionar documentação XML (Swagger)
- Namespace: `FastFood.KitchenFlow.Api.Controllers`

## Referências
- **OrderHub**: Verificar padrão de Controllers
- **Auth**: Verificar padrão de Controllers
- Controller deve ser simples, apenas chama UseCases

## Observações importantes
- **Endpoint existente**: Manter `POST /api/preparations` da Story 06 intacto
- **Mapeamento**: Controller faz mapeamento de parâmetros para InputModel
- **Validação**: Validações de negócio ficam no UseCase

## Como testar
- Executar `dotnet build` na API
- Executar `dotnet run` e testar endpoints no Swagger
- Testar cada endpoint com dados válidos e inválidos
- Verificar que endpoint da Story 06 ainda funciona

## Critérios de aceite
- Classe `PreparationController` expandida com:
  - Endpoint `GET /api/preparations` - listar
  - Endpoint `POST /api/preparations/{id}/start` - iniciar
  - Endpoint `POST /api/preparations/{id}/finish` - finalizar
- Endpoint existente `POST /api/preparations` mantido (Story 06)
- Validação de parâmetros
- Mapeamento para InputModels
- Chamada aos UseCases
- Retorno HTTP apropriado (200, 400, 404)
- Swagger documenta os endpoints
- Projeto API compila sem erros
- Controller segue padrão do OrderHub/Auth
