namespace Business.API.Middleware;

using System.Text.Json;
using Business.Application.Common;

/// <summary>
/// Traduce las excepciones a respuestas HTTP consistentes:
/// las reglas de negocio a 400 (con su mensaje), y cualquier otro error a 500
/// (con un mensaje genérico, registrando el detalle en el log).
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessRuleException ex)
        {
            _logger.LogWarning("Regla de negocio incumplida: {Message}", ex.Message);
            await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);
            await WriteResponse(context, StatusCodes.Status500InternalServerError,
                "Ocurrió un error inesperado. Intente nuevamente o contacte al administrador.");
        }
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var payload = ApiResponse<object>.Fail(message);
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        await context.Response.WriteAsync(json);
    }
}
