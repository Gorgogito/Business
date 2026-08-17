namespace Business.Application.Common;

using System.Globalization;
using System.Reflection;
using System.Text;

/// <summary>
/// Serializa una colección a CSV (UTF-8 con BOM, para que Excel respete los acentos).
/// Solo incluye propiedades de tipos simples; ignora listas y objetos anidados.
/// </summary>
public static class CsvExport
{
    public static byte[] ToCsv<T>(IEnumerable<T> rows)
    {
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => EsSimple(p.PropertyType))
            .ToArray();

        var sb = new StringBuilder();
        sb.Append('﻿'); // BOM
        sb.AppendLine(string.Join(",", props.Select(p => Escapar(p.Name))));

        foreach (var row in rows)
        {
            var valores = props.Select(p => Escapar(Formatear(p.GetValue(row))));
            sb.AppendLine(string.Join(",", valores));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static bool EsSimple(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal)
            || t == typeof(DateTime) || t == typeof(Guid);
    }

    private static string Formatear(object? value) => value switch
    {
        null => "",
        DateTime d => d.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
        decimal m => m.ToString("0.####", CultureInfo.InvariantCulture),
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString() ?? ""
    };

    private static string Escapar(string v) =>
        v.Contains(',') || v.Contains('"') || v.Contains('\n') || v.Contains('\r')
            ? $"\"{v.Replace("\"", "\"\"")}\""
            : v;
}
