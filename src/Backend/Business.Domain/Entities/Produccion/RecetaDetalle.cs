namespace Business.Domain.Entities.Produccion;

using Business.Domain.Common;
using Business.Domain.Entities.Maestros;

/// <summary>Insumo (materia prima) de una receta, con la cantidad para el rendimiento base.</summary>
public class RecetaDetalle : BaseEntity
{
    public int RecetaId { get; set; }
    public Receta? Receta { get; set; }
    public int InsumoId { get; set; }           // producto materia prima
    public Producto? Insumo { get; set; }
    public decimal Cantidad { get; set; }
}
