using Klippr_Backend.Community.Domain.Commands;

namespace Klippr_Backend.Community.Domain.Aggregates;

/// <summary>
/// Modela una reseña de promoción, con su calificación, likes y comentarios.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Único agregado del bounded context de reseñas; los comentarios viven como entidad hija
/// dentro de este agregado en lugar de ser un agregado separado.
/// </remarks>
public class Review
{
    /// <summary>
    /// Identificador único de la reseña.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador de la promoción reseñada.
    /// </summary>
    public Guid PromotionId { get; private set; }

    /// <summary>
    /// Identificador del usuario que escribió la reseña.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Calificación otorgada, entre 1 y 5.
    /// </summary>
    public int Rating { get; private set; }

    /// <summary>
    /// Contenido de la reseña.
    /// </summary>
    public string Comment { get; private set; } = string.Empty;

    /// <summary>
    /// Fecha y hora de creación de la reseña.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    private readonly List<Guid> _likedByUserIds = [];

    /// <summary>
    /// Identificadores de los usuarios que dieron like a la reseña.
    /// </summary>
    public IReadOnlyList<Guid> LikedByUserIds => _likedByUserIds.AsReadOnly();

    /// <summary>
    /// Cantidad total de likes recibidos.
    /// </summary>
    public int LikeCount => _likedByUserIds.Count;

    private readonly List<ReviewComment> _comments = [];

    /// <summary>
    /// Comentarios asociados a la reseña.
    /// </summary>
    public IReadOnlyList<ReviewComment> Comments => _comments.AsReadOnly();

    private Review() { }

    private Review(Guid promotionId, Guid userId, int rating, string comment)
    {
        if (promotionId == Guid.Empty)
            throw new ArgumentException("Promotion id cannot be empty.", nameof(promotionId));

        if (userId == Guid.Empty)
            throw new ArgumentException("User id cannot be empty.", nameof(userId));

        if (rating < 1 || rating > 5)
            throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.", nameof(comment));

        Id = Guid.NewGuid();
        PromotionId = promotionId;
        UserId = userId;
        Rating = rating;
        Comment = comment.Trim();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Crea una nueva reseña a partir del comando de publicación.
    /// </summary>
    /// <param name="command">Datos necesarios para publicar la reseña.</param>
    /// <returns>Una nueva instancia del agregado <see cref="Review"/>.</returns>
    public static Review Create(CreateReviewCommand command) =>
        new(command.PromotionId, command.UserId, command.Rating, command.Comment);

    /// <summary>
    /// Agrega un comentario a la reseña.
    /// </summary>
    /// <param name="userId">Identificador del usuario que comenta.</param>
    /// <param name="comment">Contenido del comentario.</param>
    /// <returns>El comentario creado.</returns>
    public ReviewComment AddComment(Guid userId, string comment)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User id cannot be empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty.", nameof(comment));

        var reviewComment = ReviewComment.Create(Id, userId, comment);
        _comments.Add(reviewComment);
        return reviewComment;
    }

    /// <summary>
    /// Activa o desactiva el like de un usuario sobre la reseña.
    /// </summary>
    /// <param name="userId">Identificador del usuario que reacciona.</param>
    /// <returns><see langword="true"/> si el like quedó activo; <see langword="false"/> si fue removido.</returns>
    public bool ToggleLike(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User id cannot be empty.", nameof(userId));

        if (_likedByUserIds.Contains(userId))
        {
            _likedByUserIds.Remove(userId);
            return false;
        }

        _likedByUserIds.Add(userId);
        return true;
    }
}
