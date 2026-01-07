using System.Collections.Generic;

namespace FastFood.KitchenFlow.Application.Models.Common;

/// <summary>
/// Extensões para formatação de conteúdo com nome camelCase.
/// </summary>
public static class NamedContentExtensions
{
    /// <summary>
    /// Formata um objeto para o campo Content do ApiResponse, removendo sufixos "Model" ou "Response" e convertendo para camelCase.
    /// </summary>
    /// <typeparam name="T">Tipo do objeto a ser formatado.</typeparam>
    /// <param name="obj">Objeto a ser formatado.</param>
    /// <returns>Objeto formatado como dicionário com chave camelCase ou o próprio objeto se for tipo simples.</returns>
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
        
        // Converter para camelCase
        var camelCaseName = baseName.Length > 1
            ? char.ToLowerInvariant(baseName[0]) + baseName[1..]
            : char.ToLowerInvariant(baseName[0]).ToString();

        return new Dictionary<string, object>
        {
            { camelCaseName, obj! }
        };
    }

    /// <summary>
    /// Verifica se um tipo é um tipo simples (primitivo, string, Guid, decimal, DateTime).
    /// </summary>
    /// <param name="type">Tipo a ser verificado.</param>
    /// <returns>True se for tipo simples, False caso contrário.</returns>
    private static bool IsSimpleType(Type type)
    {
        return type.IsPrimitive
               || type == typeof(string)
               || type == typeof(Guid)
               || type == typeof(decimal)
               || type == typeof(DateTime);
    }
}
