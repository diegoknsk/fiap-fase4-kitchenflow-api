# Subtask 08: Criar DeliveryController

## Descrição
Criar o `DeliveryController` na camada API com os três endpoints para gerenciar deliveries.

## Passos de implementação
- Criar classe `DeliveryController` em `Controllers/DeliveryController.cs`:
  - Atributo `[ApiController]`
  - Rota `[Route("api/[controller]")]`
  - Construtor recebe UseCases (via DI):
    - `CreateDeliveryUseCase`
    - `GetReadyDeliveriesUseCase`
    - `FinalizeDeliveryUseCase`
  - Endpoint `POST /api/deliveries`:
    - Método `CreateDelivery([FromBody] CreateDeliveryRequest request)`:
      - Validar `ModelState`
      - Mapear `CreateDeliveryRequest` para `CreateDeliveryInputModel`
      - Chamar `createDeliveryUseCase.ExecuteAsync(inputModel)`
      - Retornar HTTP Response:
        - 201 Created com Response em caso de sucesso
        - 400 Bad Request se ModelState inválido ou Preparation não está Finished
        - 409 Conflict se delivery já existe (se aplicável)
  - Endpoint `GET /api/deliveries/ready`:
    - Método `GetReadyDeliveries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)`:
      - Criar `GetReadyDeliveriesInputModel` com parâmetros
      - Chamar `getReadyDeliveriesUseCase.ExecuteAsync(inputModel)`
      - Retornar 200 OK com Response
  - Endpoint `POST /api/deliveries/{id}/finalize`:
    - Método `FinalizeDelivery([FromRoute] Guid id)`:
      - Criar `FinalizeDeliveryInputModel` com id
      - Chamar `finalizeDeliveryUseCase.ExecuteAsync(inputModel)`
      - Retornar HTTP Response:
        - 200 OK com Response em caso de sucesso
        - 400 Bad Request se status inválido
        - 404 Not Found se delivery não existe
- Criar Request DTOs em `Models/DeliveryManagement/`:
  - `CreateDeliveryRequest.cs`:
    - `PreparationId` (Guid) - [Required]
    - `OrderId` (Guid?) - opcional
- Adicionar documentação XML (Swagger)
- Namespace: `FastFood.KitchenFlow.Api.Controllers`

## Referências
- **OrderHub**: Verificar padrão de Controllers
- **Auth**: Verificar padrão de Controllers
- Controller deve ser simples, apenas chama UseCases

## Observações importantes
- **Request DTOs**: Específicos da API (podem ter validações HTTP)
- **InputModels**: Da Application (sem dependências HTTP)
- **Mapeamento**: Controller faz mapeamento Request → InputModel
- **Validação**: Usar `ModelState.IsValid` para validações básicas

## Como testar
- Executar `dotnet build` na API
- Executar `dotnet run` e testar endpoints no Swagger
- Testar cada endpoint com dados válidos e inválidos

## Critérios de aceite
- Classe `DeliveryController` criada
- Endpoints implementados:
  - `POST /api/deliveries` - criar
  - `GET /api/deliveries/ready` - listar prontas
  - `POST /api/deliveries/{id}/finalize` - finalizar
- Validação de ModelState
- Mapeamento Request → InputModel
- Chamada aos UseCases
- Retorno HTTP apropriado (201, 200, 400, 404, 409)
- Request DTOs criados com validações
- Swagger documenta os endpoints
- Projeto API compila sem erros
- Controller segue padrão do OrderHub/Auth
