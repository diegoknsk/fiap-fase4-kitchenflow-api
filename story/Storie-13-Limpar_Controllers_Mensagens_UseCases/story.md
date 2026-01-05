# Storie-13: Limpar Controllers e Mover Mensagens para Use Cases

## Descrição
Como desenvolvedor, quero remover todas as mensagens hardcoded das controllers e garantir que todas as mensagens de erro e validação venham dos use cases ou exceções, deixando os controllers limpos e seguindo os princípios da Clean Architecture.

## Objetivo
Finalizar a padronização de respostas da API removendo mensagens hardcoded das controllers:
- Criar exceções apropriadas para validação de ModelState
- Criar exceção para erros internos do servidor
- Mover tratamento de validação de ModelState para os use cases (ou criar exceção específica)
- Remover todas as mensagens hardcoded das controllers
- Garantir que todas as mensagens venham de exceções ou use cases

## Escopo Técnico
- **Tecnologias**: .NET 8, C# 12
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Application` (Exceptions, UseCases)
  - `FastFood.KitchenFlow.Api` (Controllers)
- **Referências do monolito**:
  - `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\Models\Common`
- **Controllers afetados**:
  - `PreparationController` (4 endpoints)
  - `DeliveryController` (3 endpoints)

## Problema Atual

### Mensagens Hardcoded nas Controllers

**PreparationController:**
- `"Dados inválidos."` - Validação de ModelState (linha 59)
- `"Erro interno do servidor."` - Catch genérico de Exception (linhas 100, 143, 189, 235)

**DeliveryController:**
- `"Dados inválidos."` - Validação de ModelState (linha 55)
- `"Erro interno do servidor."` - Catch genérico de Exception (linhas 104, 138, 184)

### Situação Atual
- ✅ Use Cases já retornam `ApiResponse<T>`
- ✅ Presenters já definem mensagens de sucesso
- ✅ Exceções de negócio já têm mensagens descritivas
- ❌ Controllers ainda têm mensagens hardcoded para:
  - Validação de ModelState
  - Erros genéricos (500)

## Solução Proposta

### 1. Criar Exceções Apropriadas

#### ValidationException
```csharp
namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando dados de entrada são inválidos (validação de ModelState ou InputModel).
/// </summary>
public class ValidationException : ArgumentException
{
    /// <summary>
    /// Lista de erros de validação (opcional).
    /// </summary>
    public Dictionary<string, string[]>? Errors { get; }

    /// <summary>
    /// Construtor que recebe mensagem de erro.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro de validação.</param>
    public ValidationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Construtor que recebe mensagem e erros detalhados.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro de validação.</param>
    /// <param name="errors">Dicionário com erros de validação por campo.</param>
    public ValidationException(string message, Dictionary<string, string[]>? errors)
        : base(message)
    {
        Errors = errors;
    }
}
```

#### InternalServerErrorException
```csharp
namespace FastFood.KitchenFlow.Application.Exceptions;

/// <summary>
/// Exceção lançada quando ocorre um erro interno do servidor não esperado.
/// Esta exceção deve ser usada apenas em casos excepcionais onde não há uma exceção mais específica.
/// </summary>
public class InternalServerErrorException : Exception
{
    /// <summary>
    /// Construtor que recebe mensagem de erro.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro interno.</param>
    public InternalServerErrorException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Construtor que recebe mensagem e exceção interna.
    /// </summary>
    /// <param name="message">Mensagem descritiva do erro interno.</param>
    /// <param name="innerException">Exceção que causou o erro.</param>
    public InternalServerErrorException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
```

### 2. Atualizar Use Cases para Validar ModelState

**Opção A (Recomendada)**: Mover validação de ModelState para os Use Cases
- Use Cases recebem InputModels já validados
- Se InputModel for inválido, Use Case lança `ValidationException`
- Controllers apenas mapeiam Request → InputModel e chamam Use Case

**Opção B**: Criar ActionFilter ou Middleware
- Criar ActionFilter que valida ModelState e lança `ValidationException`
- Controllers removem validação manual de ModelState

**Recomendação**: Usar **Opção A** - mover validação para Use Cases, pois:
- Mantém lógica de validação na camada Application
- Controllers ficam mais limpos
- Facilita testes unitários

### 3. Atualizar Controllers

#### Antes
```csharp
if (!ModelState.IsValid)
{
    return BadRequest(ApiResponse<CreatePreparationResponse>.Fail("Dados inválidos."));
}

try
{
    var apiResponse = await _createPreparationUseCase.ExecuteAsync(inputModel);
    return CreatedAtAction(nameof(CreatePreparation), new { id }, apiResponse);
}
catch (PreparationAlreadyExistsException ex)
{
    return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
catch (Exception)
{
    return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail("Erro interno do servidor."));
}
```

#### Depois
```csharp
try
{
    // Mapear Request para InputModel (validação será feita no Use Case)
    var inputModel = new CreatePreparationInputModel
    {
        OrderId = request.OrderId,
        OrderSnapshot = request.OrderSnapshot
    };

    // Chamar UseCase (já retorna ApiResponse<T>)
    var apiResponse = await _createPreparationUseCase.ExecuteAsync(inputModel);

    // Extrair Id do Content para CreatedAtAction
    Guid id = ExtractIdFromContent(apiResponse.Content);

    // Retornar HTTP 201 Created
    return CreatedAtAction(nameof(CreatePreparation), new { id }, apiResponse);
}
catch (ValidationException ex)
{
    return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
catch (PreparationAlreadyExistsException ex)
{
    return Conflict(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
catch (ArgumentException ex)
{
    return BadRequest(ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
catch (InternalServerErrorException ex)
{
    return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail(ex.Message));
}
catch (Exception ex)
{
    // Log do erro (não expor detalhes ao cliente)
    // Em produção, usar mensagem genérica
    return StatusCode(500, ApiResponse<CreatePreparationResponse>.Fail("Erro interno do servidor."));
}
```

### 4. Atualizar Use Cases para Validar InputModels

#### Exemplo: CreatePreparationUseCase
```csharp
public async Task<ApiResponse<CreatePreparationResponse>> ExecuteAsync(CreatePreparationInputModel inputModel)
{
    // Validar InputModel (substituir validações atuais)
    if (inputModel == null)
    {
        throw new ValidationException("Dados de entrada não podem ser nulos.");
    }

    if (inputModel.OrderId == Guid.Empty)
    {
        throw new ValidationException("OrderId não pode ser vazio.");
    }

    if (string.IsNullOrWhiteSpace(inputModel.OrderSnapshot))
    {
        throw new ValidationException("OrderSnapshot não pode ser nulo ou vazio.");
    }

    // ... resto da lógica ...
}
```

## Regras de Negócio

### Fluxo de Validação
1. **Request → InputModel**: Controller mapeia Request para InputModel (sem validação de ModelState)
2. **Use Case valida InputModel**: Use Case valida InputModel e lança `ValidationException` se inválido
3. **Controller trata exceções**: Controller captura exceções e retorna `ApiResponse<T>.Fail()` com mensagem da exceção

### Mensagens de Erro
- **Validação (400)**: `ValidationException` com mensagem descritiva
- **Erros de negócio (404, 409)**: Exceções específicas (`PreparationNotFoundException`, `DeliveryAlreadyExistsException`, etc.)
- **Erros internos (500)**: `InternalServerErrorException` ou catch genérico com mensagem padrão

### Tratamento de Exceções
- **Controllers**: Apenas tratam exceções e criam `ApiResponse<T>.Fail()` com mensagem da exceção
- **Use Cases**: Lançam exceções apropriadas com mensagens descritivas
- **Exceções**: Contêm mensagens claras e descritivas

## Subtasks

- [ ] [Subtask 01: Criar exceções ValidationException e InternalServerErrorException](./subtask/Subtask-01-Criar_Excecoes_Validacao.md)
- [ ] [Subtask 02: Atualizar Use Cases para validar InputModels e lançar ValidationException](./subtask/Subtask-02-Atualizar_UseCases_Validacao.md)
- [ ] [Subtask 03: Remover validação de ModelState das Controllers](./subtask/Subtask-03-Remover_Validacao_ModelState.md)
- [ ] [Subtask 04: Atualizar tratamento de exceções nas Controllers](./subtask/Subtask-04-Atualizar_Tratamento_Excecoes.md)
- [ ] [Subtask 05: Validar padronização e testes](./subtask/Subtask-05-Validar_Padronizacao.md)

## Critérios de Aceite da História

- [ ] Exceções `ValidationException` e `InternalServerErrorException` criadas
- [ ] Todos os Use Cases validam InputModels e lançam `ValidationException` quando apropriado
- [ ] Validação de ModelState removida das Controllers
- [ ] Todas as mensagens hardcoded removidas das Controllers
- [ ] Controllers apenas tratam exceções e criam `ApiResponse<T>.Fail()` com mensagem da exceção
- [ ] Estrutura de resposta padronizada em todos os endpoints:
  - `Success`: boolean
  - `Message`: string com mensagem descritiva (vinda de exceção ou use case)
  - `Content`: objeto formatado com ToNamedContent
- [ ] Controllers não contêm mais mensagens de negócio hardcoded
- [ ] Todos os testes existentes continuam passando
- [ ] Documentação XML atualizada nas Exceções e Controllers
- [ ] Código compila sem erros
- [ ] Respostas da API seguem o padrão estabelecido

## Alinhamento com Projeto de Referência

### Padrão do Monolito
O projeto de referência (`fiap-fastfood`) usa o seguinte padrão:

1. **Validação nos Use Cases**
   - Use Cases validam InputModels
   - Lançam exceções apropriadas com mensagens descritivas

2. **Controllers tratam exceções**
   - Controllers capturam exceções
   - Retornam `ApiResponse<T>.Fail()` com mensagem da exceção

3. **Mensagens definidas nas Exceções**
   - Mensagens de erro: nas exceções
   - Mensagens de sucesso: nos Presenters

### Aplicação no KitchenFlow
Seguindo o mesmo padrão:
- **Use Cases**: Validam InputModels e lançam exceções apropriadas
- **Exceções**: Contêm mensagens descritivas
- **Controllers**: Apenas tratam exceções e retornam `ApiResponse<T>`

## Observações Arquiteturais

### Clean Architecture
- **Application Layer**: Contém exceções e lógica de validação
- **Use Cases**: Validam InputModels e lançam exceções apropriadas
- **Controllers**: Apenas adaptadores HTTP, tratam exceções e retornam `ApiResponse<T>`

### Padrões Aplicados
- **Separação de Responsabilidades**: Validação na camada Application, não na camada de Interface
- **Consistência**: Todas as mensagens vêm de exceções ou use cases
- **Manutenibilidade**: Mensagens centralizadas nas exceções

### Benefícios
- **Consistência**: Todas as mensagens seguem o mesmo padrão
- **Manutenibilidade**: Mensagens centralizadas nas exceções
- **Testabilidade**: Fácil testar validações e exceções
- **Clean Code**: Controllers limpos e focados apenas em adaptação HTTP

### Exemplo de Resposta JSON

**Validação (400):**
```json
{
  "success": false,
  "message": "OrderId não pode ser vazio.",
  "content": null
}
```

**Erro de negócio (409):**
```json
{
  "success": false,
  "message": "Já existe uma preparação para o pedido 3fa85f64-5717-4562-b3fc-2c963f66afa6.",
  "content": null
}
```

**Erro interno (500):**
```json
{
  "success": false,
  "message": "Erro interno do servidor.",
  "content": null
}
```

## Notas Importantes

- **Validação de ModelState**: Pode ser mantida como validação adicional na Controller, mas deve lançar `ValidationException` em vez de retornar mensagem hardcoded
- **Erros genéricos**: Catch genérico de `Exception` deve usar mensagem padrão "Erro interno do servidor." e fazer log do erro real
- **Logging**: Implementar logging adequado para erros internos (não expor detalhes ao cliente)
