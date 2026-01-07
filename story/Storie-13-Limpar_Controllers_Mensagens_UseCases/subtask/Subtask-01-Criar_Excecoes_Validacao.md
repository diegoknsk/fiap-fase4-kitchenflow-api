# Subtask 01: Criar exceções ValidationException e InternalServerErrorException

## Descrição
Criar as exceções `ValidationException` e `InternalServerErrorException` no projeto Application para padronizar mensagens de erro e remover mensagens hardcoded das controllers.

## Passos de implementação

### 1. Criar ValidationException
- Criar arquivo `Exceptions/ValidationException.cs` em `FastFood.KitchenFlow.Application`:
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

### 2. Criar InternalServerErrorException
- Criar arquivo `Exceptions/InternalServerErrorException.cs` em `FastFood.KitchenFlow.Application`:
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

### 3. Adicionar documentação XML
- Adicionar documentação XML completa em ambas as exceções
- Seguir padrão das outras exceções do projeto

## Arquivos a criar
- `src/Core/FastFood.KitchenFlow.Application/Exceptions/ValidationException.cs`
- `src/Core/FastFood.KitchenFlow.Application/Exceptions/InternalServerErrorException.cs`

## Referências
- Seguir padrão das outras exceções do projeto:
  - `PreparationAlreadyExistsException`
  - `PreparationNotFoundException`
  - `DeliveryAlreadyExistsException`
  - `DeliveryNotFoundException`
  - `PreparationNotFinishedException`

## Como testar
- Executar `dotnet build` no projeto Application para verificar compilação
- Verificar que exceções podem ser instanciadas e lançadas
- Verificar que mensagens são preservadas corretamente

## Critérios de aceite
- Exceção `ValidationException` criada em `Exceptions/ValidationException.cs`
- Exceção `InternalServerErrorException` criada em `Exceptions/InternalServerErrorException.cs`
- Ambas as exceções têm documentação XML completa
- Exceções seguem padrão das outras exceções do projeto
- Projeto Application compila sem erros
- Exceções podem ser instanciadas e lançadas corretamente
