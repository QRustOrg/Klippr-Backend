namespace Klippr_Backend.Promotions.Interface.Resources;

/// <summary>
/// Representa los datos temporales requeridos para publicar una promocion.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <param name="IsBusinessVerified">Indica si el negocio propietario fue verificado por el contexto de perfil.</param>
/// <remarks>
/// Este recurso mantiene el contrato temporal hasta que el bounded context Profile entregue
/// el estado de verificacion de negocio mediante una integracion directa.
/// </remarks>
public record PublishPromotionResource(bool IsBusinessVerified);
