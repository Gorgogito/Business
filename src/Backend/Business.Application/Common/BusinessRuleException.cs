namespace Business.Application.Common;

/// <summary>
/// Excepción para reglas de negocio incumplidas (p. ej. stock insuficiente, documento
/// ya facturado). El middleware la traduce a HTTP 400 con un mensaje para el usuario,
/// a diferencia de los errores inesperados que devuelven 500.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
