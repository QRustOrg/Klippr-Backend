namespace Klippr_Backend.Community.Domain.Aggregates;

/// <summary>
/// Comentario asociado a una reseña.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Entidad hija del agregado <see cref="Review"/>; no se persiste ni se consulta de forma independiente.
/// </remarks>
public class ReviewComment
{
    /// <summary>
    /// Identificador único del comentario.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador de la reseña a la que pertenece el comentario.
    /// </summary>
    public Guid ReviewId { get; private set; }

    /// <summary>
    /// Identificador del usuario que escribió el comentario.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Contenido del comentario.
    /// </summary>
    public string Comment { get; private set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de creación del comentario.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    private ReviewComment() { }

    private ReviewComment(Guid reviewId, Guid userId, string comment)
    {
        Id = Guid.NewGuid();
        ReviewId = reviewId;
        UserId = userId;
        Comment = comment.Trim();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Crea un nuevo comentario para una reseña.
    /// </summary>
    /// <param name="reviewId">Identificador de la reseña comentada.</param>
    /// <param name="userId">Identificador del usuario que comenta.</param>
    /// <param name="comment">Contenido del comentario.</param>
    /// <returns>Una nueva instancia de <see cref="ReviewComment"/>.</returns>
    public static ReviewComment Create(Guid reviewId, Guid userId, string comment) =>
        new(reviewId, userId, comment);
}
