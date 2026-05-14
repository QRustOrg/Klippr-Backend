using Klippr_Backend.Redemption.Domain.Commands;
using Klippr_Backend.Redemption.Domain.ValueObjects;

namespace Klippr_Backend.Redemption.Domain.Aggregates;

/// <summary>
/// Modela el ciclo de vida de un canje de promocion.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Controla las reglas de generacion, confirmacion, expiracion y bloqueo de un canje.
/// </remarks>
public class Redemption
{
    /// <summary>
    /// Identificador interno del canje.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Identificador del consumidor que genero el canje.
    /// </summary>
    public Guid ConsumerId { get; private set; }

    /// <summary>
    /// Identificador del negocio afiliado donde se usa el canje.
    /// </summary>
    public Guid BusinessId { get; private set; }

    /// <summary>
    /// Identificador de la promocion canjeada.
    /// </summary>
    public string PromotionId { get; private set; } = string.Empty;

    /// <summary>
    /// Codigo unico usado para validar el canje.
    /// </summary>
    public RedemptionCode Code { get; private set; } = null!;

    /// <summary>
    /// Token unico para prevenir reutilizacion o fraude.
    /// </summary>
    public Guid UniqueToken { get; private set; }

    /// <summary>
    /// Estado actual del canje.
    /// </summary>
    public RedemptionStatus Status { get; private set; }

    /// <summary>
    /// Fecha y hora en la que se genero el canje.
    /// </summary>
    public DateTimeOffset GeneratedAt { get; private set; }

    /// <summary>
    /// Fecha y hora en la que se confirmo el uso del canje.
    /// </summary>
    public DateTimeOffset? RedeemedAt { get; private set; }

    /// <summary>
    /// Fecha y hora en la que el canje fue bloqueado.
    /// </summary>
    public DateTimeOffset? BlockedAt { get; private set; }

    /// <summary>
    /// Fecha y hora en la que el canje deja de ser valido.
    /// </summary>
    public DateTimeOffset ExpiresAt { get; private set; }

    /// <summary>
    /// Metodo de validacion asociado al canje.
    /// </summary>
    public RedemptionValidationMethod ValidationMethod { get; private set; }

    /// <summary>
    /// Monto de descuento aplicado al canje.
    /// </summary>
    public decimal DiscountAppliedAmount { get; private set; }

    private Redemption() { }

    private Redemption(
        Guid consumerId,
        Guid businessId,
        string promotionId,
        DateTimeOffset generatedAt,
        DateTimeOffset expiresAt,
        decimal discountAppliedAmount,
        RedemptionValidationMethod validationMethod)
    {
        if (consumerId == Guid.Empty)
            throw new ArgumentException("Consumer id cannot be empty.", nameof(consumerId));

        if (businessId == Guid.Empty)
            throw new ArgumentException("Business id cannot be empty.", nameof(businessId));

        if (string.IsNullOrWhiteSpace(promotionId))
            throw new ArgumentException("Promotion id cannot be empty.", nameof(promotionId));

        if (discountAppliedAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(discountAppliedAmount), "Discount applied amount cannot be negative.");

        if (expiresAt <= generatedAt)
            throw new ArgumentException("Expiration date must be greater than generation date.", nameof(expiresAt));

        ConsumerId = consumerId;
        BusinessId = businessId;
        PromotionId = promotionId;
        Code = RedemptionCode.Generate();
        UniqueToken = Guid.NewGuid();
        Status = RedemptionStatus.Generated;
        GeneratedAt = generatedAt;
        ExpiresAt = expiresAt;
        ValidationMethod = validationMethod;
        DiscountAppliedAmount = discountAppliedAmount;
    }

    /// <summary>
    /// Crea un nuevo canje en estado generado a partir del comando de redencion.
    /// </summary>
    /// <param name="command">Datos necesarios para generar el canje.</param>
    /// <returns>Una nueva instancia del agregado <see cref="Redemption"/>.</returns>
    public static Redemption Create(RedeemPromotionCommand command)
    {
        return new Redemption(
            command.ConsumerId,
            command.BusinessId,
            command.PromotionId,
            DateTimeOffset.UtcNow,
            command.ExpiresAt,
            command.DiscountAppliedAmount,
            command.ValidationMethod
        );
    }

    /// <summary>
    /// Confirma el uso del canje si se encuentra generado y vigente.
    /// </summary>
    /// <param name="redeemedAt">Fecha y hora de confirmacion del canje.</param>
    /// <param name="validationMethod">Metodo usado para validar el canje.</param>
    public void Confirm(DateTimeOffset redeemedAt, RedemptionValidationMethod validationMethod)
    {
        if (Status != RedemptionStatus.Generated)
            throw new InvalidOperationException("Only generated redemptions can be confirmed.");

        if (IsExpired(redeemedAt))
        {
            Status = RedemptionStatus.Expired;
            throw new InvalidOperationException("Expired redemptions cannot be confirmed.");
        }

        ValidationMethod = validationMethod;
        Status = RedemptionStatus.Redeemed;
        RedeemedAt = redeemedAt;
    }

    /// <summary>
    /// Bloquea un canje usado para evitar su reutilizacion.
    /// </summary>
    /// <param name="blockedAt">Fecha y hora del bloqueo.</param>
    public void Block(DateTimeOffset blockedAt)
    {
        if (Status != RedemptionStatus.Redeemed)
            throw new InvalidOperationException("Only redeemed redemptions can be blocked.");

        if (RedeemedAt.HasValue && blockedAt < RedeemedAt.Value)
            throw new ArgumentException("Blocked date cannot be earlier than redeemed date.", nameof(blockedAt));

        Status = RedemptionStatus.Blocked;
        BlockedAt = blockedAt;
    }

    /// <summary>
    /// Expira un canje generado cuando su fecha limite ya fue alcanzada.
    /// </summary>
    /// <param name="currentDate">Fecha y hora usada para evaluar la expiracion.</param>
    public void Expire(DateTimeOffset currentDate)
    {
        if (Status != RedemptionStatus.Generated)
            throw new InvalidOperationException("Only generated redemptions can expire.");

        if (!IsExpired(currentDate))
            throw new InvalidOperationException("Redemption has not expired yet.");

        Status = RedemptionStatus.Expired;
    }

    /// <summary>
    /// Determina si el canje puede ser confirmado en la fecha indicada.
    /// </summary>
    /// <param name="currentDate">Fecha y hora de evaluacion.</param>
    /// <returns><see langword="true"/> si el canje esta generado y vigente; en caso contrario, <see langword="false"/>.</returns>
    public bool CanBeRedeemed(DateTimeOffset currentDate) =>
        Status == RedemptionStatus.Generated && !IsExpired(currentDate);

    /// <summary>
    /// Determina si el canje ya vencio para la fecha indicada.
    /// </summary>
    /// <param name="currentDate">Fecha y hora de evaluacion.</param>
    /// <returns><see langword="true"/> si la fecha actual es posterior o igual a la fecha de expiracion.</returns>
    public bool IsExpired(DateTimeOffset currentDate) => currentDate >= ExpiresAt;
}
