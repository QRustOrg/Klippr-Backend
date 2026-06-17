using Klippr_Backend.Reviews.Domain.Aggregates;

namespace Klippr_Backend.Reviews.Domain.Repositories;

/// <summary>
/// Define el contrato de persistencia para el agregado <see cref="Review"/>.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Este contrato abstrae el almacenamiento sin acoplar el dominio a infraestructura.
/// </remarks>
public interface IReviewRepository
{
    /// <summary>
    /// Obtiene una reseña por su identificador, incluyendo sus comentarios.
    /// </summary>
    /// <param name="id">Identificador de la reseña consultada.</param>
    /// <returns>Reseña encontrada o <see langword="null"/> si no existe.</returns>
    Task<Review?> FindByIdAsync(Guid id);

    /// <summary>
    /// Obtiene reseñas filtradas opcionalmente por promoción y/o usuario.
    /// </summary>
    /// <param name="promotionId">Identificador de la promoción a filtrar, si aplica.</param>
    /// <param name="userId">Identificador del usuario a filtrar, si aplica.</param>
    /// <returns>Colección de reseñas que cumplen los filtros indicados.</returns>
    Task<IEnumerable<Review>> FindAllAsync(Guid? promotionId, Guid? userId);

    /// <summary>
    /// Determina si un usuario ya reseñó una promoción.
    /// </summary>
    /// <param name="userId">Identificador del usuario consultado.</param>
    /// <param name="promotionId">Identificador de la promoción consultada.</param>
    /// <returns><see langword="true"/> si ya existe una reseña del usuario para la promoción.</returns>
    Task<bool> ExistsByUserAndPromotionAsync(Guid userId, Guid promotionId);

    /// <summary>
    /// Agrega una nueva reseña al almacenamiento.
    /// </summary>
    /// <param name="review">Agregado de reseña que se desea persistir.</param>
    Task AddAsync(Review review);

    /// <summary>
    /// Actualiza una reseña existente en el almacenamiento.
    /// </summary>
    /// <param name="review">Agregado de reseña con cambios aplicados.</param>
    void Update(Review review);

    /// <summary>
    /// Persiste los cambios pendientes en el almacenamiento.
    /// </summary>
    Task SaveChangesAsync();
}
