using RedemptionAggregate = Klippr_Backend.Redemption.Domain.Aggregates.Redemption;

namespace Klippr_Backend.Redemption.Domain.Repositories;

/// <summary>
/// Define el contrato de persistencia para el agregado <see cref="RedemptionAggregate"/>.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contrato abstrae el almacenamiento sin acoplar el dominio a infraestructura.
/// </remarks>
public interface IRedemptionRepository
{
    /// <summary>
    /// Obtiene un canje por su identificador.
    /// </summary>
    /// <param name="id">Identificador del canje consultado.</param>
    /// <returns>Canje encontrado o <see langword="null"/> si no existe.</returns>
    Task<RedemptionAggregate?> FindByIdAsync(int id);

    /// <summary>
    /// Obtiene un canje por su token unico antifraude.
    /// </summary>
    /// <param name="uniqueToken">Token unico asociado al canje.</param>
    /// <returns>Canje encontrado o <see langword="null"/> si no existe.</returns>
    Task<RedemptionAggregate?> FindByUniqueTokenAsync(Guid uniqueToken);

    /// <summary>
    /// Obtiene los canjes asociados a un consumidor.
    /// </summary>
    /// <param name="consumerId">Identificador del consumidor consultado.</param>
    /// <returns>Coleccion de canjes del consumidor indicado.</returns>
    Task<IEnumerable<RedemptionAggregate>> FindByConsumerIdAsync(Guid consumerId);

    /// <summary>
    /// Obtiene los canjes asociados a un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio consultado.</param>
    /// <returns>Coleccion de canjes del negocio indicado.</returns>
    Task<IEnumerable<RedemptionAggregate>> FindByBusinessIdAsync(Guid businessId);

    /// <summary>
    /// Agrega un nuevo canje al almacenamiento.
    /// </summary>
    /// <param name="redemption">Agregado de canje que se desea persistir.</param>
    Task AddAsync(RedemptionAggregate redemption);

    /// <summary>
    /// Actualiza un canje existente en el almacenamiento.
    /// </summary>
    /// <param name="redemption">Agregado de canje con cambios aplicados.</param>
    void Update(RedemptionAggregate redemption);

    /// <summary>
    /// Elimina un canje del almacenamiento.
    /// </summary>
    /// <param name="redemption">Agregado de canje que se desea eliminar.</param>
    void Remove(RedemptionAggregate redemption);

    /// <summary>
    /// Persiste los cambios pendientes en el almacenamiento.
    /// </summary>
    Task SaveChangesAsync();
}
