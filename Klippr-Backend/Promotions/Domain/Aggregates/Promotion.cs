namespace Klippr_Backend.Promotions.Domain.Aggregates;

public class Promotion
{
    public Guid Id { get; private set; }
    public Guid BusinessId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DiscountValue Discount { get; private set; } = null!;
    public TimeFrame ValidityPeriod { get; private set; } = null!;
    public int? RedemptionCap { get; private set; } // null = unlimited
    public PromotionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = [];
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

    public void Cancel(CancelPromotionCommand command)
    {
        if (Status is not (PromotionStatus.Draft or PromotionStatus.Published))
            throw new InvalidOperationException("Only active promotions can be cancelled.");

        Status = PromotionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;

        _domainEvents.Add(new PromotionCancelledEvent(Id, BusinessId));
    }

    public bool IsActive() =>
        Status == PromotionStatus.Published &&
        ValidityPeriod.Contains(DateTime.UtcNow);

    public void ClearDomainEvents() => _domainEvents.Clear();
}