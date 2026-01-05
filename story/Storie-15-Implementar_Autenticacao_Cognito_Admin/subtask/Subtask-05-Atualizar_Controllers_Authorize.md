# Subtask 05: Atualizar Controllers com Authorize

## Descrição
Adicionar o atributo `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` em todos os endpoints, exceto `CreatePreparation` que deve permanecer anônimo.

## Passos de implementação

### PreparationController
- Abrir arquivo `src/InterfacesExternas/FastFood.KitchenFlow.Api/Controllers/PreparationController.cs`
- Adicionar `using Microsoft.AspNetCore.Authorization;` no topo
- **CreatePreparation** (método `HttpPost`):
  - **NÃO adicionar** `[Authorize]` - manter anônimo
- **GetPreparations** (método `HttpGet`):
  - Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` antes do método
- **StartPreparation** (método `HttpPost("take-next")`):
  - Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` antes do método
- **FinishPreparation** (método `HttpPost("{id}/finish")`):
  - Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` antes do método

### DeliveryController
- Abrir arquivo `src/InterfacesExternas/FastFood.KitchenFlow.Api/Controllers/DeliveryController.cs`
- Adicionar `using Microsoft.AspNetCore.Authorization;` no topo
- **GetReadyDeliveries** (método `HttpGet("ready")`):
  - Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` antes do método
- **FinalizeDelivery** (método `HttpPost("{id}/finalize")`):
  - Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` antes do método

### HealthController (se existir)
- Verificar se existe `HealthController.cs`
- Se existir, **NÃO adicionar** `[Authorize]` - health checks devem ser anônimos

## Referências
- **OrderHub OrderController**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\Controllers\OrderController.cs`
  - Linha 30: `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- **OrderHub ProductsController**: `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\Controllers\ProductsController.cs`
  - Linha 68: `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`

## Como testar
- Executar `dotnet build` no projeto `FastFood.KitchenFlow.Api`
- Verificar que não há erros de compilação
- Testar endpoints (sem token devem retornar 401, exceto CreatePreparation):
  - `POST /api/preparations` → Deve retornar 201 (sem autenticação)
  - `GET /api/preparations` → Deve retornar 401 (sem autenticação)
  - `POST /api/preparations/take-next` → Deve retornar 401 (sem autenticação)
  - `POST /api/preparations/{id}/finish` → Deve retornar 401 (sem autenticação)
  - `GET /api/deliveries/ready` → Deve retornar 401 (sem autenticação)
  - `POST /api/deliveries/{id}/finalize` → Deve retornar 401 (sem autenticação)

## Critérios de aceite
- `PreparationController`:
  - `CreatePreparation`: **SEM** `[Authorize]` (anônimo)
  - `GetPreparations`: **COM** `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `StartPreparation`: **COM** `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinishPreparation`: **COM** `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- `DeliveryController`:
  - `GetReadyDeliveries`: **COM** `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinalizeDelivery`: **COM** `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- `using Microsoft.AspNetCore.Authorization;` adicionado em ambos os controllers
- Projeto compila sem erros
- Endpoints retornam 401 sem token (exceto CreatePreparation)
