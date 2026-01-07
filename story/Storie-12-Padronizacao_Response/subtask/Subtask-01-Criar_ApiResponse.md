# Subtask 01: Criar estrutura Models/Common e ApiResponse

## Descrição
Criar a estrutura de pastas `Models/Common` no projeto Application e implementar a classe `ApiResponse<T>` para padronizar todas as respostas da API.

## Passos de implementação
- Criar pasta `Models/Common/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar classe `ApiResponse<T>` em `Models/Common/ApiResponse.cs`:
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
- Adicionar documentação XML completa
- Namespace: `FastFood.KitchenFlow.Application.Models.Common`

## Referências
- **Monolito**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\Models\Common\ApiResponse.cs`
- Seguir exatamente o padrão do projeto de referência

## Observações importantes
- A classe usa `ToNamedContent()` que será criada na próxima subtask
- Por enquanto, pode deixar um comentário TODO ou criar um método temporário
- O construtor permite criar respostas customizadas quando necessário
- Os métodos estáticos `Ok()` e `Fail()` facilitam a criação de respostas

## Validação
- [ ] Pasta `Models/Common/` criada
- [ ] Classe `ApiResponse<T>` criada com todos os métodos
- [ ] Documentação XML completa
- [ ] Código compila (mesmo que ToNamedContent ainda não exista)
- [ ] Namespace correto
