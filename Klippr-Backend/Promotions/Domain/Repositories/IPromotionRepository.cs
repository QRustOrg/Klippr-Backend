using Klippr_Backend.Promotions.Domain.Aggregates;

namespace Klippr_Backend.Promotions.Domain.Repositories;

/// <summary>
/// Define el contrato de persistencia para el agregado <see cref="Promotion"/>.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// El repositorio abstrae el mecanismo de almacenamiento y expone operaciones centradas en el agregado, sin acoplar el dominio.
/// </remarks>
public interface IPromotionRepository
{
    /// <summary>
    /// Obtiene una promocion por su identificador unico.
    /// </summary>
    /// <param name="promotionId">Identificador de la promocion consultada.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>La promocion encontrada o <see langword="null"/> si no existe.</returns>
    Task<Promotion?> GetByIdAsync(Guid promotionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las promociones asociadas a un negocio especifico.
    /// </summary>
    /// <param name="businessId">Identificador del negocio propietario de las promociones.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Coleccion de promociones pertenecientes al negocio indicado.</returns>
    Task<IReadOnlyList<Promotion>> GetByBusinessIdAsync(Guid businessId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las promociones activas disponibles para consulta.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Coleccion de promociones activas.</returns>
    /// <remarks>
    /// La implementacion debe aplicar el criterio de actividad definido por el dominio:
    /// promocion publicada y dentro de su periodo de vigencia.
    /// </remarks>
    Task<IReadOnlyList<Promotion>> GetActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Agrega una nueva promocion al almacenamiento.
    /// </summary>
    /// <param name="promotion">Agregado de promocion que se desea persistir.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    Task AddAsync(Promotion promotion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una promocion existente en el almacenamiento.
    /// </summary>
    /// <param name="promotion">Agregado de promocion con los cambios aplicados.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    Task UpdateAsync(Promotion promotion, CancellationToken cancellationToken = default);

    /// <summary>
    /// Persiste los cambios pendientes realizados a traves del repositorio.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
