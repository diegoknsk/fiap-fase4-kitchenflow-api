# Subtask 06: Criar PreparationController

## Descrição
Criar o `PreparationController` na camada API com o endpoint `POST /api/preparations` que será chamado pelo Payment quando um pagamento for confirmado.

## Passos de implementação
- Criar classe `PreparationController` em `Controllers/PreparationController.cs`:
  - Atributo `[ApiController]`
  - Rota `[Route("api/[controller]")]`
  - Construtor recebe `CreatePreparationUseCase` (via DI)
  - Endpoint `POST /api/preparations`:
    - Método `CreatePreparation([FromBody] CreatePreparationRequest request)`:
      - Validar `ModelState`
      - Mapear `CreatePreparationRequest` (da API) para `CreatePreparationInputModel` (da Application)
      - Chamar `useCase.ExecuteAsync(inputModel)`
      - Retornar HTTP Response:
        - 201 Created com Response em caso de sucesso
        - 400 Bad Request se ModelState inválido
        - 409 Conflict se preparação já existe (se aplicável)
- Criar `CreatePreparationRequest` (DTO da API) em `Models/PreparationManagement/CreatePreparationRequest.cs`:
  - `OrderId` (Guid)
  - `OrderSnapshot` (string)
  - Data Annotations para validação
- Adicionar documentação XML (Swagger)
- Namespace: `FastFood.KitchenFlow.Api.Controllers`

## Referências
- **OrderHub**: Verificar padrão de Controllers
- **Auth**: Verificar padrão de Controllers
- Controller deve ser simples, apenas chama UseCase

## Observações importantes
- **Request DTO**: `CreatePreparationRequest` é específico da API (pode ter validações HTTP)
- **InputModel**: `CreatePreparationInputModel` é da Application (sem dependências HTTP)
- **Mapeamento**: Controller faz mapeamento Request → InputModel
- **Validação**: Usar `ModelState.IsValid` para validações básicas

## Como testar
- Executar `dotnet build` na API
- Executar `dotnet run` e testar endpoint no Swagger
- Testar com dados válidos (deve retornar 201)
- Testar com dados inválidos (deve retornar 400)

## Critérios de aceite
- Classe `PreparationController` criada
- Endpoint `POST /api/preparations` implementado
- Validação de ModelState
- Mapeamento Request → InputModel
- Chamada ao UseCase
- Retorno HTTP apropriado (201, 400, 409)
- `CreatePreparationRequest` criado com validações
- Swagger documenta o endpoint
- Projeto API compila sem erros
- Controller segue padrão do OrderHub/Auth
