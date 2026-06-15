namespace Klippr_Backend.Promotions.Interface.Transform;

/// <summary>
/// Representa la respuesta al crear una promocion.
/// </summary>
/// <param name="PromotionId">Identificador de la promocion creada.</param>
public record PromotionCreatedResource(Guid PromotionId);
