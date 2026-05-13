namespace Klippr_Backend.Promotions.Domain.ValueObjects;

/// <summary>
/// Representa el valor de descuento ofrecido por una promoción.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Para descuentos porcentuales, el monto debe estar entre 0 y 100.
/// Para descuentos de monto fijo, el monto debe ser mayor que 0.
/// </remarks>
public record DiscountValue
{
    /// <summary>
    /// Monto numérico del descuento.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Tipo de descuento aplicado.
    /// </summary>
    public DiscountType Type { get; init; }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="DiscountValue"/>.
    /// </summary>
    /// <param name="amount">Monto numérico del descuento.</param>
    /// <param name="type">Tipo de descuento aplicado.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Se produce cuando el monto no cumple las reglas del tipo de descuento.
    /// </exception>
    public DiscountValue(decimal amount, DiscountType type)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Discount amount must be greater than zero.");

        if (type == DiscountType.Percentage && amount > 100)
            throw new ArgumentOutOfRangeException(nameof(amount), "Percentage discount cannot exceed 100.");

        Amount = amount;
        Type = type;
    }
}

/// <summary>
/// Representa los tipos de descuento permitidos para una promoción.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Descuento calculado como porcentaje.
    /// </summary>
    Percentage,

    /// <summary>
    /// Descuento aplicado como monto fijo.
    /// </summary>
    FixedAmount
}
