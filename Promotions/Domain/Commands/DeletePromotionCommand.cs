namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para eliminar una promoción existente.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador de la promoción que se desea eliminar.</param>
public record DeletePromotionCommand(Guid PromotionId);
