using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.ValueObjects;

namespace Klippr_Backend.Promotions.Domain.Aggregates;

/// <summary>
/// Promotions.cs modela el ciclo de vida una promoción.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Controla las transiciones de estado de la promoción y emite eventos de dominio
/// cuando ocurren cambios relevantes para otros bounded contexts.
/// </remarks>
public class Promotion
{
    /// <summary>
    /// Identificador único de la promoción.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador del negocio propietario de la promoción.
    /// </summary>
    public Guid BusinessId { get; private set; }

    /// <summary>
    /// Título comercial de la promoción.
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Descripción de la promoción.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Descuento ofrecido por la promoción.
    /// </summary>
    public DiscountValue Discount { get; private set; } = null!;

    /// <summary>
    /// Periodo temporal en el que la promoción está vigente.
    /// </summary>
    public TimeFrame ValidityPeriod { get; private set; } = null!;

    /// <summary>
    /// Límite máximo permitido de redenciones para la promoción.
    /// </summary>
    /// <remarks>
    /// Un valor <see langword="null"/> indica que la promoción no tiene límite definido.
    /// La verificación efectiva del límite corresponde al bounded context de redención.
    /// </remarks>
    public int? RedemptionCap { get; private set; }

    /// <summary>
    /// Estado actual de la promoción.
    /// </summary>
    public PromotionStatus Status { get; private set; }

    /// <summary>
    /// Fecha y hora de creación del agregado en UTC.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Fecha y hora de la última modificación del agregado en UTC.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// Eventos de dominio pendientes de publicación generados por el agregado.
    /// </summary>
    /// <remarks>
    /// La colección expuesta es de solo lectura para evitar mutaciones externas sobre e buffer interno de eventos del agregado.
    /// </remarks>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Promotion() { } // EF Core

    private Promotion(Guid businessId, string title, string description,
        DiscountValue discount, TimeFrame validityPeriod, int? redemptionCap)
    {
        Id = Guid.NewGuid();
        BusinessId = businessId;
        Title = title;
        Description = description;
        Discount = discount;
        ValidityPeriod = validityPeriod;
        RedemptionCap = redemptionCap;
        Status = PromotionStatus.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Crea una nueva promoción en estado borrador a partir de un comando de creación.
    /// </summary>
    /// <param name="command">Datos de entrada necesarios para inicializar la promoción.</param>
    /// <returns>Una nueva instancia del agregado <see cref="Promotion"/>.</returns>
    public static Promotion Create(CreatePromotionCommand command)
    {
        return new Promotion(
            command.BusinessId,
            command.Title,
            command.Description,
            command.Discount,
            command.ValidityPeriod,
            command.RedemptionCap
        );
    }

    /// <summary>
    /// Actualiza los datos editables de la promoción.
    /// </summary>
    /// <param name="command">Comando con los nuevos valores para la promoción.</param>
    /// <remarks>
    /// La actualización solo está permitida mientras la promoción permanezca en estado borrador.
    /// </remarks>
    public void Update(UpdatePromotionCommand command)
    {
        if (Status != PromotionStatus.Draft)
            throw new InvalidOperationException("Only draft promotions can be updated.");

        Title = command.Title;
        Description = command.Description;
        Discount = command.Discount;
        ValidityPeriod = command.ValidityPeriod;
        RedemptionCap = command.RedemptionCap;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Publica la promoción y la deja disponible para su consumo.
    /// </summary>
    /// <param name="command">Comando que contiene la intención de publicación y el estado de verificación del negocio.</param>
    /// <remarks>
    /// La transición a "publicado" solo es válida desde borrador y requiere que el negocio haya sido verificado previament (por el contexto de perfil).
    /// </remarks>
    public void Publish(PublishPromotionCommand command)
    {
        if (Status != PromotionStatus.Draft)
            throw new InvalidOperationException("Only draft promotions can be published.");

        if (!command.IsBusinessVerified)
            throw new InvalidOperationException("Business must be verified to publish a promotion.");

        Status = PromotionStatus.Published;
        UpdatedAt = DateTime.UtcNow;

        _domainEvents.Add(new PromotionPublishedEvent(Id, BusinessId));
    }

    /// <summary>
    /// Cancela una promoción activa.
    /// </summary>
    /// <param name="command">Comando que representa la intención de cancelación.</param>
    /// <remarks>
    /// La cancelación solo aplica a promociones en borrador o publicadas. Al completarse,
    /// se registra un evento de dominio para notificar a otros componentes.
    /// </remarks>
    public void Cancel(CancelPromotionCommand command)
    {
        if (Status is not (PromotionStatus.Draft or PromotionStatus.Published))
            throw new InvalidOperationException("Only active promotions can be cancelled.");

        Status = PromotionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        _domainEvents.Add(new PromotionCancelledEvent(Id, BusinessId));
    }

    /// <summary>
    /// Determina si la promoción está publicada y su vigencia incluye el momento actual.
    /// </summary>
    /// <returns><see langword="true"/> si la promoción está activa; en caso contrario, <see langword="false"/>.</returns>
    /// <remarks>
    /// La evaluación usa la hora actual en UTC y delega la validación temporal al value object
    /// <c>TimeFrame</c>.
    /// </remarks>
    public bool IsActive() =>
        Status == PromotionStatus.Published &&
        ValidityPeriod.Contains(DateTime.UtcNow);

    /// <summary>
    /// Limpia los eventos de dominio acumulados después de ser despachados.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}
