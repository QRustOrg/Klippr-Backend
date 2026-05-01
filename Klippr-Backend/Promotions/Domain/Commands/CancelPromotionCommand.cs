namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para cancelar una promoción existente.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador de la promoción que se desea cancelar.</param>
/// <remarks>
/// Este comando representa únicamente la intención de cancelación. La validez de latransición de estado se determina en el agregado.
/// </remarks>
public record CancelPromotionCommand(Guid PromotionId);
