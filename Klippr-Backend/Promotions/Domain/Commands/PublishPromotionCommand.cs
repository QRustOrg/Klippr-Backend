namespace Klippr_Backend.Promotions.Domain.Commands;

/// <summary>
/// Comando para solicitar la publicación de una promoción.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="PromotionId">Identificador de la promoción que se desea publicar.</param>
/// <param name="IsBusinessVerified">Indica si el negocio fue validado previamente por el contexto de perfil.</param>
/// <remarks>
/// El valor de <paramref name="IsBusinessVerified"/> debe ser resuelto por el caller antes de emitir el comando. El agregado usa este dato para autorizar la transición a publicado.
/// </remarks>
public record PublishPromotionCommand(
    Guid PromotionId,
    bool IsBusinessVerified
);
