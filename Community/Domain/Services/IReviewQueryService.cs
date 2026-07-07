using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Domain.Queries;

namespace Klippr_Backend.Community.Domain.Services;

/// <summary>
/// Define las operaciones de lectura disponibles para reseñas y comentarios.
/// </summary>
/// <author>Samuel Bonifacio</author>
public interface IReviewQueryService
{
    /// <summary>
    /// Obtiene reseñas filtradas opcionalmente por promoción y/o usuario.
    /// </summary>
    Task<IEnumerable<Review>> Handle(GetReviewsQuery query);

    /// <summary>
    /// Obtiene una reseña por su identificador.
    /// </summary>
    Task<Review?> Handle(GetReviewByIdQuery query);

    /// <summary>
    /// Obtiene los comentarios de una reseña.
    /// </summary>
    Task<IEnumerable<ReviewComment>> Handle(GetCommentsByReviewIdQuery query);

    /// <summary>
    /// Determina si un usuario puede publicar una reseña para una promoción:
    /// requiere una redención confirmada y no haber reseñado previamente.
    /// </summary>
    Task<bool> Handle(CanReviewQuery query);

    /// <summary>
    /// Determina si un usuario tiene una redención confirmada para una promoción.
    /// </summary>
    /// <remarks>
    /// Pieza reutilizada tanto por <see cref="Handle(CanReviewQuery)"/> como por el flag
    /// "verified" mostrado en cada reseña.
    /// </remarks>
    Task<bool> HasRedeemedPromotionAsync(Guid userId, Guid promotionId);
}
