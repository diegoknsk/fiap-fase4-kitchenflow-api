# Subtask 07: Implementar rota "Olá Mundo" na API

## Descrição
Criar um controller básico na API com uma rota GET simples que retorna "Olá Mundo" para validar que a API está funcionando corretamente. Esta rota servirá como ponto de validação inicial.

## Passos de implementação
- Criar controller `HealthController.cs` em `src/InterfacesExternas/FastFood.KitchenFlow.Api/Controllers/`
- Implementar endpoint GET `/api/health` que retorna uma mensagem simples:
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class HealthController : ControllerBase
  {
      [HttpGet]
      public IActionResult Get()
      {
          return Ok(new { message = "Olá Mundo - KitchenFlow API está funcionando!" });
      }
  }
  ```
- Garantir que o controller está no namespace correto: `FastFood.KitchenFlow.Api.Controllers`
- Verificar que o controller é descoberto automaticamente pelo ASP.NET Core

## Como testar
- Executar `dotnet run` no projeto Api
- Acessar `http://localhost:5000/api/health` ou `https://localhost:5001/api/health` (portas podem variar)
- Verificar que retorna JSON com a mensagem "Olá Mundo"
- Acessar Swagger e verificar que a rota aparece na documentação
- Teste manual: Fazer requisição HTTP GET para o endpoint

## Critérios de aceite
- Controller `HealthController` criado em `Controllers/`
- Endpoint GET `/api/health` implementado e funcionando
- Retorna JSON com mensagem "Olá Mundo"
- Endpoint aparece no Swagger
- API inicia sem erros
- Rota responde corretamente a requisições HTTP GET
- Código segue convenções do ASP.NET Core

