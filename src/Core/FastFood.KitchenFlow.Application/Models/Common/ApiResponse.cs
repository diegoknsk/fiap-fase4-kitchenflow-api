namespace FastFood.KitchenFlow.Application.Models.Common;

/// <summary>
/// Classe genérica para padronizar todas as respostas da API.
/// </summary>
/// <typeparam name="T">Tipo do conteúdo da resposta.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensagem descritiva sobre o resultado da operação.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Conteúdo da resposta formatado com ToNamedContent.
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Construtor da classe ApiResponse.
    /// </summary>
    /// <param name="content">Conteúdo da resposta.</param>
    /// <param name="message">Mensagem descritiva.</param>
    /// <param name="success">Indica se a operação foi bem-sucedida.</param>
    public ApiResponse(object? content, string? message = "Requisição bem-sucedida.", bool success = true)
    {
        Content = content;
        Message = message;
        Success = success;
    }

    /// <summary>
    /// Cria uma resposta de sucesso com dados.
    /// </summary>
    /// <param name="data">Dados a serem retornados.</param>
    /// <param name="message">Mensagem de sucesso.</param>
    /// <returns>ApiResponse com sucesso.</returns>
    public static ApiResponse<T> Ok(T? data, string? message = "Requisição bem-sucedida.")
        => new(data.ToNamedContent(), message, true);

    /// <summary>
    /// Cria uma resposta de sucesso sem dados.
    /// </summary>
    /// <param name="message">Mensagem de sucesso.</param>
    /// <returns>ApiResponse com sucesso.</returns>
    public static ApiResponse<T> Ok(string? message = "Requisição bem-sucedida.")
        => new(null, message, true);

    /// <summary>
    /// Cria uma resposta de falha.
    /// </summary>
    /// <param name="message">Mensagem de erro.</param>
    /// <returns>ApiResponse com falha.</returns>
    public static ApiResponse<T> Fail(string? message)
        => new(null, message, false);
}
