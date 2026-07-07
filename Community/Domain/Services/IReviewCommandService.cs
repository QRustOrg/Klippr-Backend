using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Domain.Commands;

namespace Klippr_Backend.Community.Domain.Services;

/// <summary>
/// Define las operaciones de escritura disponibles para reseñas, likes y comentarios.
/// </summary>
/// <author>Samuel Bonifacio</author>
public interface IReviewCommandService
{
    /// <summary>
    /// Publica una nueva reseña, validando elegibilidad del usuario.
    /// </summary>
    /// <param name="command">Datos de la reseña a publicar.</param>
    /// <returns>La reseña creada.</returns>
    Task<Review> Handle(CreateReviewCommand command);

    /// <summary>
    /// Agrega un comentario a una reseña existente.
    /// </summary>
    /// <param name="command">Datos del comentario a agregar.</param>
    /// <returns>El comentario creado.</returns>
    Task<ReviewComment> Handle(AddCommentCommand command);

    /// <summary>
    /// Activa o desactiva el like de un usuario sobre una reseña.
    /// </summary>
    /// <param name="command">Datos del like a aplicar.</param>
    /// <returns>La reseña actualizada o <see langword="null"/> si no existe.</returns>
    Task<Review?> Handle(ToggleLikeCommand command);
}
