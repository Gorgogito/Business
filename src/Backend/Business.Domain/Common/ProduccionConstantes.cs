namespace Business.Domain.Common;

public static class EstadoReceta
{
    public const string Activa = "ACTIVA";
    public const string Inactiva = "INACTIVA";
}

public static class EstadoOrdenFabricacion
{
    public const string Pendiente = "PENDIENTE";   // creada, aún no producida
    public const string Terminada = "TERMINADA";   // producida y costeada
    public const string Anulada = "ANULADA";
}
