using Klippr_Backend.Promotions.Domain.Aggregates;
using Klippr_Backend.Promotions.Domain.Queries;

namespace Klippr_Backend.Promotions.Domain.Services;

/// <summary>
/// Define las operaciones de lectura disponibles para consultar promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contrato separa las consultas del flujo de comandos y permite que la capa de aplicación resuelva lecturas sin exponer detalles de infraestructura.
/// </remarks>
public interface IPromotionQueryService
{
    /// <summary>
    /// Obtiene una promoción por su identificador único.
    /// </summary>
    /// <param name="query">Query con el identificador de la promoción consultada.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>La promoción encontrada o <see langword="null"/> si no existe.</returns>
    Task<Promotion?> GetByIdAsync(GetPromotionByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las promociones asociadas a un negocio específico.
    /// </summary>
    /// <param name="query">Query con el identificador del negocio propietario.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Colección de promociones de el negocio indicado.</returns>
    Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(
        GetPromotionsByBusinessIdQuery query,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Obtiene las promociones activas disponibles para consumidores.
    /// </summary>
    /// <param name="query">Query que representa la consulta de promociones activas.</param>
    /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
    /// <returns>Colección de promociones activas.</returns>
    /// <remarks>
    /// La implementación debe usar el criterio de actividad definido por el dominio:
    /// promoción publicada y dentro del periodo de vigencia.
    /// </remarks>
    Task<IReadOnlyList<Promotion>> GetActiveAsync(
        GetActivePromotionsQuery query,
        CancellationToken cancellationToken = default
    );
}
