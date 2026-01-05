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
