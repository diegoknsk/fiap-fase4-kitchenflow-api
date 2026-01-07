# Storie-12: Padronização de Respostas da API

## Descrição
Como desenvolvedor, quero padronizar todas as respostas da API usando um modelo `ApiResponse<T>` consistente e mover todas as mensagens de negócio dos controllers para os use cases, para garantir uma estrutura de resposta uniforme e deixar os controllers mais coesos, seguindo os princípios da Clean Architecture.

## Objetivo
Padronizar todas as respostas da API do microserviço KitchenFlow, incluindo:
- Criar classe `ApiResponse<T>` no projeto Application para padronizar respostas
- Criar extensão `ToNamedContent` para formatação de conteúdo
- Refatorar todos os Responses existentes para usar `ApiResponse<T>`
- Mover mensagens de negócio dos controllers para os use cases (via Presenters)
- Atualizar todos os controllers para usar o padrão `ApiResponse<T>`
- Garantir que todas as mensagens de sucesso e erro venham dos use cases

## Escopo Técnico
- **Tecnologias**: .NET 8, C# 12
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Application` (Models/Common, Responses, Presenters, UseCases)
  - `FastFood.KitchenFlow.Api` (Controllers)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\Models\Common\ApiResponse.cs`
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\Models\Common\NamedContentExtensions.cs`
- **Controllers afetados**:
  - `PreparationController` (4 endpoints)
  - `DeliveryController` (3 endpoints)
  - `HealthController` (opcional, manter simples)
  - `HelloController` (opcional, manter simples)

## Modelo de Resposta Padrão

### Classe ApiResponse<T>
```csharp
namespace FastFood.KitchenFlow.Application.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Content { get; set; }

        public ApiResponse(object? content, string? message = "Requisição bem-sucedida.", bool success = true)
        {
            Content = content;
            Message = message;
            Success = success;
        }

        public static ApiResponse<T> Ok(T? data, string? message = "Requisição bem-sucedida.")
            => new(data.ToNamedContent(), message, true);

        public static ApiResponse<T> Ok(string? message = "Requisição bem-sucedida.")
            => new(null, message, true);

        public static ApiResponse<T> Fail(string? message)
            => new(null, message, false);
    }
}
```

### Extensão ToNamedContent
```csharp
namespace FastFood.KitchenFlow.Application.Models.Common
{
    public static class NamedContentExtensions
    {
        public static object? ToNamedContent<T>(this T obj)
        {
            if (obj is null) return null;

            Type type = typeof(T);

            if (IsSimpleType(type))
                return obj;

            // Remove sufixo "Model" ou "Response" se presente
            var rawName = type.Name;
            var baseName = rawName;
            if (rawName.EndsWith("Model"))
                baseName = rawName[..^"Model".Length];
            else if (rawName.EndsWith("Response"))
                baseName = rawName[..^"Response".Length];
            
            var camelCaseName = char.ToLowerInvariant(baseName[0]) + baseName.Substring(1);

            return new Dictionary<string, object>
            {
                { camelCaseName, obj! }
            };
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                   || type == typeof(string)
                   || type == typeof(Guid)
                   || type == typeof(decimal)
                   || type == typeof(DateTime);
        }
    }
}
```

## Padrão de Refatoração

### Antes (Response atual)
```csharp
public class CreatePreparationResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; } = string.Empty;
}
```

### Depois (usando ApiResponse)
```csharp
// Response específico mantido para tipagem
public class CreatePreparationResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Presenter retorna ApiResponse<CreatePreparationResponse>
public static ApiResponse<CreatePreparationResponse> Present(CreatePreparationOutputModel outputModel)
{
    var response = new CreatePreparationResponse { ... };
    return ApiResponse<CreatePreparationResponse>.Ok(response, "Preparação criada com sucesso.");
}
```

### Use Case
```csharp
// Use Case retorna ApiResponse<T> (não apenas Response)
public async Task<ApiResponse<CreatePreparationResponse>> ExecuteAsync(CreatePreparationInputModel inputModel)
{
    // ... lógica de negócio ...
    
    // Criar OutputModel
    var outputModel = new CreatePreparationOutputModel { ... };
    
    // Chamar Presenter que retorna ApiResponse<T>
    return CreatePreparationPresenter.Present(outputModel);
}
```

### Controller
```csharp
// Antes
return Ok(response);

// Depois - Use Case já retorna ApiResponse<T>
var apiResponse = await _createPreparationUseCase.ExecuteAsync(inputModel);
return CreatedAtAction(nameof(CreatePreparation), new { id = ... }, apiResponse);

// Para erros, Controller cria ApiResponse<T>.Fail()
catch (PreparationAlreadyExistsException ex)
{
    return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
```

## Regras de Negócio

### Fluxo de Resposta
1. **Use Cases retornam `ApiResponse<T>`**
   - Use Cases chamam Presenters que retornam `ApiResponse<T>`
   - Use Cases retornam `ApiResponse<T>` diretamente (não Response models simples)
   - Mensagens de sucesso são definidas nos Presenters

2. **Controllers recebem `ApiResponse<T>` do Use Case**
   - Controllers recebem `ApiResponse<T>` do Use Case e retornam diretamente
   - Para erros (exceções), Controllers criam `ApiResponse<T>.Fail()` com mensagem da exceção
   - Controllers não criam mensagens de sucesso (vêm dos Presenters via Use Cases)

3. **Mensagens de Negócio**:
   - **Mensagens de sucesso**: Definidas nos Presenters
   - **Mensagens de erro**: Definidas nas exceções de domínio/application
   - **Controllers**: Apenas tratam exceções e criam `ApiResponse<T>.Fail()` quando necessário

4. **Estrutura de Resposta**:
   - `Success`: boolean indicando sucesso/falha
   - `Message`: string com mensagem descritiva
   - `Content`: objeto com os dados (usando ToNamedContent para formatação)

### Tratamento de Erros
- **Erros de validação (400)**: Controller cria `ApiResponse<T>.Fail("mensagem")` ou usa mensagem da exceção
- **Erros de negócio (404, 409)**: Controller cria `ApiResponse<T>.Fail(ex.Message)` usando mensagem da exceção
- **Erros internos (500)**: Controller cria `ApiResponse<T>.Fail("Erro interno do servidor.")`

## Subtasks

- [ ] [Subtask 01: Criar estrutura Models/Common e ApiResponse](./subtask/Subtask-01-Criar_ApiResponse.md)
- [ ] [Subtask 02: Criar extensão ToNamedContent](./subtask/Subtask-02-Criar_ToNamedContent.md)
- [ ] [Subtask 03: Refatorar Responses e Presenters - PreparationManagement](./subtask/Subtask-03-Refatorar_PreparationManagement.md)
- [ ] [Subtask 04: Refatorar Responses e Presenters - DeliveryManagement](./subtask/Subtask-04-Refatorar_DeliveryManagement.md)
- [ ] [Subtask 05: Refatorar Controllers - PreparationController](./subtask/Subtask-05-Refatorar_PreparationController.md)
- [ ] [Subtask 06: Refatorar Controllers - DeliveryController](./subtask/Subtask-06-Refatorar_DeliveryController.md)
- [ ] [Subtask 07: Mover mensagens de negócio para Use Cases](./subtask/Subtask-07-Mover_mensagens_negocio.md)
- [ ] [Subtask 08: Validar padronização e testes](./subtask/Subtask-08-Validar_padronizacao.md)

## Critérios de Aceite da História

- [ ] Classe `ApiResponse<T>` criada em `FastFood.KitchenFlow.Application.Models.Common`
- [ ] Extensão `ToNamedContent` criada e funcionando corretamente
- [ ] Todos os Presenters refatorados para retornar `ApiResponse<T>`
- [ ] Todos os Use Cases refatorados para retornar `ApiResponse<T>` (não Response models simples)
- [ ] Todas as mensagens de negócio movidas dos controllers para os use cases (via Presenters)
- [ ] Todos os controllers refatorados para receber e retornar `ApiResponse<T>` do Use Case
- [ ] Estrutura de resposta padronizada em todos os endpoints:
  - `Success`: boolean
  - `Message`: string com mensagem descritiva
  - `Content`: objeto formatado com ToNamedContent
- [ ] Controllers não contêm mais mensagens de negócio hardcoded
- [ ] Todos os testes existentes continuam passando
- [ ] Documentação XML atualizada nos Presenters e Controllers
- [ ] Código compila sem erros
- [ ] Respostas da API seguem o padrão estabelecido

## Alinhamento com Projeto de Referência

### Padrão do Monolito
O projeto de referência (`fiap-fastfood`) usa o seguinte padrão:

1. **Use Cases retornam `ApiResponse<T>` diretamente**
   - Exemplo: `TakeNextOrderForPreparationUseCase` retorna `ApiResponse<PreparationModel>`
   - Use Cases criam `ApiResponse<T>` usando Presenters ou diretamente

2. **Controllers recebem `ApiResponse<T>` do Use Case**
   - Exemplo: `CustomerController` recebe resultado e retorna `Ok(ApiResponse<T>.Ok(result))`
   - Para erros, Controllers criam `ApiResponse<T>.Fail()` diretamente

3. **Mensagens definidas nos Use Cases ou Presenters**
   - Mensagens de sucesso: nos Presenters ou Use Cases
   - Mensagens de erro: nas exceções

### Aplicação no KitchenFlow
Seguindo o mesmo padrão:
- **Use Cases**: Retornam `ApiResponse<T>` (via Presenters)
- **Presenters**: Retornam `ApiResponse<T>` com mensagens de sucesso
- **Controllers**: Recebem `ApiResponse<T>` do Use Case e retornam diretamente
- **Erros**: Controllers criam `ApiResponse<T>.Fail()` com mensagem da exceção

## Observações Arquiteturais

### Clean Architecture
- **Application Layer**: Contém `ApiResponse<T>` e lógica de formatação
- **Use Cases**: Orquestram lógica de negócio, chamam Presenters e retornam `ApiResponse<T>`
- **Presenters**: Transformam OutputModels em `ApiResponse<T>` e definem mensagens de sucesso
- **Controllers**: Apenas adaptadores HTTP, recebem `ApiResponse<T>` do Use Case e retornam diretamente
- **Tratamento de Exceções**: Controllers tratam exceções e criam `ApiResponse<T>.Fail()` quando necessário

### Padrões Aplicados
- **Padrão de Resposta Uniforme**: Todas as APIs retornam `ApiResponse<T>`
- **Separação de Responsabilidades**: Mensagens de negócio nos use cases, não nos controllers
- **Extensibilidade**: Fácil adicionar novos campos ao `ApiResponse` no futuro
- **Type Safety**: Mantém tipagem forte com `ApiResponse<T>`

### Benefícios
- **Consistência**: Todas as respostas seguem o mesmo padrão
- **Manutenibilidade**: Mensagens centralizadas nos use cases
- **Testabilidade**: Fácil testar respostas padronizadas
- **Documentação**: Estrutura clara e previsível para consumidores da API

### Exemplo de Resposta JSON

**Antes:**
```json
{
  "id": "guid",
  "orderId": "guid",
  "status": 0,
  "createdAt": "2024-01-01T00:00:00Z",
  "message": "Preparação criada com sucesso"
}
```

**Depois:**
```json
{
  "success": true,
  "message": "Preparação criada com sucesso.",
  "content": {
    "createPreparation": {
      "id": "guid",
      "orderId": "guid",
      "status": 0,
      "createdAt": "2024-01-01T00:00:00Z"
    }
  }
}
```

**Nota**: O campo `content` usa a chave `"createPreparation"` (sem "Response") porque o `ToNamedContent` remove o sufixo "Response" do nome do tipo.

**Exemplo de erro:**
```json
{
  "success": false,
  "message": "Preparação não encontrada.",
  "content": null
}
```

### Nota sobre ToNamedContent
O método `ToNamedContent` formata o nome do tipo removendo sufixos como "Model" ou "Response" e convertendo para camelCase:
- `CreatePreparationResponse` → `"createPreparation"` (remove "Response")
- `GetPreparationsResponse` → `"getPreparations"` (remove "Response")
- `OrderModel` → `"order"` (remove "Model")

Isso garante que o campo `content` tenha uma chave descritiva no JSON, seguindo o padrão do projeto de referência (que remove "Model").
