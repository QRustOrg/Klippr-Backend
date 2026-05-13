namespace Klippr_Backend.Redemption.Domain.ValueObjects;

/// <summary>
/// Encapsula el codigo unico utilizado para identificar y validar un canje.
/// </summary>
/// <author>Samuel Bonifacio</author>
public sealed record RedemptionCode
{
    /// <summary>
    /// Valor unico del codigo de canje.
    /// </summary>
    public Guid Value { get; }

    private RedemptionCode(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Redemption code cannot be empty.", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Genera un nuevo codigo unico de canje.
    /// </summary>
    /// <returns>Codigo de canje generado.</returns>
    public static RedemptionCode Generate() => new(Guid.NewGuid());

    /// <summary>
    /// Reconstruye un codigo de canje desde un valor existente.
    /// </summary>
    /// <param name="value">Valor unico del codigo.</param>
    /// <returns>Codigo de canje valido.</returns>
    public static RedemptionCode From(Guid value) => new(value);

    /// <summary>
    /// Devuelve el codigo en un formato legible y estable.
    /// </summary>
    /// <returns>Codigo de canje sin guiones y en mayusculas.</returns>
    public override string ToString() => Value.ToString("N").ToUpperInvariant();
}
