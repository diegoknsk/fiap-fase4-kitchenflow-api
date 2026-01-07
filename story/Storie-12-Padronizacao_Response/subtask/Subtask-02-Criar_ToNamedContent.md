# Subtask 02: Criar extensão ToNamedContent

## Descrição
Criar a extensão `ToNamedContent` que formata objetos para o campo `Content` do `ApiResponse`, seguindo o padrão do monolito.

## Passos de implementação
- Criar classe `NamedContentExtensions` em `Models/Common/NamedContentExtensions.cs`:
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
- Adicionar documentação XML
- Adicionar `using System;` e `using System.Collections.Generic;`
- Namespace: `FastFood.KitchenFlow.Application.Models.Common`

## Referências
- **Monolito**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Application\Models\Common\NamedContentExtensions.cs`
- Seguir exatamente o padrão do projeto de referência

## Observações importantes
- A extensão formata objetos complexos em um dicionário com nome camelCase
- Remove sufixo "Model" ou "Response" se presente
- Tipos simples são retornados diretamente
- Exemplo: `CreatePreparationResponse` vira `{ "createPreparation": { ... } }` (remove "Response")
- Exemplo: `OrderModel` vira `{ "order": { ... } }` (remove "Model")

## Validação
- [ ] Classe `NamedContentExtensions` criada
- [ ] Método `ToNamedContent<T>` implementado
- [ ] Método `IsSimpleType` implementado
- [ ] Documentação XML completa
- [ ] Código compila
- [ ] `ApiResponse<T>` agora funciona corretamente com `ToNamedContent()`
