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
