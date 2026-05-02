namespace Klippr_Backend.Promotions.Domain.ValueObjects;

/// <summary>
/// Representa el rango temporal de vigencia de una promoción.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// El rango exige que la fecha final sea posterior a la fecha inicial.
/// La evaluación de inclusión usa límites cerrados.
/// </remarks>
public record TimeFrame
{
    /// <summary>
    /// Fecha y hora de inicio de vigencia.
    /// </summary>
    public DateTime StartDate { get; init; }

    /// <summary>
    /// Fecha y hora de fin de vigencia.
    /// </summary>
    public DateTime EndDate { get; init; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="TimeFrame"/>.
    /// </summary>
    /// <param name="startDate">Fecha y hora de inicio de vigencia.</param>
    /// <param name="endDate">Fecha y hora de fin de vigencia.</param>
    /// <exception cref="ArgumentException">
    /// Se produce cuando la fecha final no es posterior a la fecha inicial.
    /// </exception>
    public TimeFrame(DateTime startDate, DateTime endDate)
    {
        if (endDate <= startDate)
            throw new ArgumentException("End date must be greater than start date.", nameof(endDate));

        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Determina si una fecha está dentro del rango de vigencia.
    /// </summary>
    /// <param name="dateTime">Fecha y hora que se desea evaluar.</param>
    /// <returns><see langword="true"/> si la fecha está dentro del rango; en caso contrario, <see langword="false"/>.</returns>
    public bool Contains(DateTime dateTime) =>
        dateTime >= StartDate && dateTime <= EndDate;
}
