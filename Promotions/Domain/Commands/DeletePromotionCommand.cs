namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para eliminar una promoción existente.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador de la promoción que se desea eliminar.</param>
/// <param name="RequestingBusinessId">Identificador del negocio autenticado que solicita la eliminación; debe coincidir con el dueño de la promoción.</param>
public record DeletePromotionCommand(Guid PromotionId, Guid RequestingBusinessId);
