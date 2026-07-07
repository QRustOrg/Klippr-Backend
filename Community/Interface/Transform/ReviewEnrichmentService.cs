using Klippr_Backend.Community.Domain.Aggregates;
using Klippr_Backend.Community.Domain.Services;
using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Repositories;
using Klippr_Backend.Profile.Domain.Repositories;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;

namespace Klippr_Backend.Community.Interface.Transform;

/// <summary>
/// Enriquece agregados de reseñas con datos denormalizados de otros bounded contexts
/// (Promotions, IAM, Profile) para construir los recursos HTTP de respuesta.
/// </summary>
/// <remarks>
/// Inyecta directamente servicios/repositorios de otros contextos, siguiendo el mismo
/// patrón que <c>AdminAnalyticsController</c> al inyectar <see cref="IPromotionQueryService"/>.
/// </remarks>
public class ReviewEnrichmentService(
    IPromotionQueryService promotionQueryService,
    IUserRepository userRepository,
    IBusinessProfileRepository businessProfileRepository,
    IReviewQueryService reviewQueryService)
{
    public async Task<ReviewResource> ToResourceAsync(Review review, Guid? requestingUserId, CancellationToken cancellationToken = default)
    {
        var promotion = await promotionQueryService
            .GetByIdAsync(new GetPromotionByIdQuery(review.PromotionId), cancellationToken)
            .ConfigureAwait(false);

        var user = await userRepository.GetByIdAsync(review.UserId, cancellationToken).ConfigureAwait(false);
        var businessProfile = promotion is null
            ? null
            : await businessProfileRepository.GetByUserIdAsync(promotion.BusinessId, cancellationToken).ConfigureAwait(false);

        var verified = await reviewQueryService
            .HasRedeemedPromotionAsync(review.UserId, review.PromotionId)
            .ConfigureAwait(false);

        var likedByCurrentUser = requestingUserId.HasValue && review.LikedByUserIds.Contains(requestingUserId.Value);

        return ReviewResourceFromEntityAssembler.ToResource(
            review,
            promotion?.Title ?? string.Empty,
            promotion?.ImageKey,
            businessProfile?.BusinessName ?? string.Empty,
            BuildUserName(user),
            userAvatar: null,
            verified,
            likedByCurrentUser);
    }

    public async Task<IReadOnlyList<ReviewResource>> ToResourcesAsync(IEnumerable<Review> reviews, Guid? requestingUserId, CancellationToken cancellationToken = default)
    {
        var resources = new List<ReviewResource>();
        foreach (var review in reviews)
            resources.Add(await ToResourceAsync(review, requestingUserId, cancellationToken).ConfigureAwait(false));

        return resources;
    }

    public async Task<CommentResource> ToCommentResourceAsync(ReviewComment comment, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(comment.UserId, cancellationToken).ConfigureAwait(false);
        return CommentResourceFromEntityAssembler.ToResource(comment, BuildUserName(user));
    }

    public async Task<IReadOnlyList<CommentResource>> ToCommentResourcesAsync(IEnumerable<ReviewComment> comments, CancellationToken cancellationToken = default)
    {
        var resources = new List<CommentResource>();
        foreach (var comment in comments)
            resources.Add(await ToCommentResourceAsync(comment, cancellationToken).ConfigureAwait(false));

        return resources;
    }

    private static string BuildUserName(User? user) =>
        user is null ? string.Empty : $"{user.FirstName} {user.LastName}".Trim();
}
