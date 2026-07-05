using System.Security.Claims;
using Klippr_Backend.Community.Domain.Commands;
using Klippr_Backend.Community.Domain.Queries;
using Klippr_Backend.Community.Domain.Services;
using Klippr_Backend.Community.Interface.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Community.Interface.REST;

[ApiController]
[Route("api/reviews")]
public class ReviewsController(
    IReviewCommandService reviewCommandService,
    IReviewQueryService reviewQueryService,
    ReviewEnrichmentService reviewEnrichmentService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Listar reseñas",
        Description = "Retorna reseñas filtradas opcionalmente por promocion o por usuario.",
        OperationId = "GetReviews")]
    public async Task<IActionResult> GetAsync(
        [FromQuery] Guid? promotionId,
        [FromQuery] Guid? userId,
        CancellationToken cancellationToken)
    {
        var reviews = await reviewQueryService
            .Handle(new GetReviewsQuery(promotionId, userId))
            .ConfigureAwait(false);

        var resources = await reviewEnrichmentService
            .ToResourcesAsync(reviews, TryGetUserId(), cancellationToken)
            .ConfigureAwait(false);

        return Ok(resources);
    }

    [Authorize]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Publicar reseña",
        Description = "Crea una reseña para una promocion. El usuario debe tener una redencion confirmada y no haber reseñado previamente.",
        OperationId = "CreateReview")]
    [ProducesResponseType(typeof(ReviewResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateReviewResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var command = CreateReviewCommandFromResourceAssembler.ToCommand(userId, resource);
            var review = await reviewCommandService.Handle(command).ConfigureAwait(false);

            var resourceResult = await reviewEnrichmentService
                .ToResourceAsync(review, userId, cancellationToken)
                .ConfigureAwait(false);

            return Ok(resourceResult);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("can-review")]
    [SwaggerOperation(
        Summary = "Verificar elegibilidad para reseñar",
        Description = "Indica si el usuario puede publicar una reseña para la promocion indicada.",
        OperationId = "CanReview")]
    public async Task<IActionResult> CanReviewAsync(
        [FromQuery] Guid promotionId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var canReview = await reviewQueryService
            .Handle(new CanReviewQuery(promotionId, userId))
            .ConfigureAwait(false);

        return Ok(canReview);
    }

    [Authorize]
    [HttpPost("{reviewId:guid}/like")]
    [SwaggerOperation(
        Summary = "Reaccionar a una reseña",
        Description = "Activa o desactiva el like del usuario autenticado sobre la reseña indicada.",
        OperationId = "LikeReview")]
    public async Task<IActionResult> LikeAsync(Guid reviewId, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var review = await reviewCommandService
            .Handle(new ToggleLikeCommand(reviewId, userId))
            .ConfigureAwait(false);

        if (review is null)
            return NotFound();

        var resource = await reviewEnrichmentService
            .ToResourceAsync(review, userId, cancellationToken)
            .ConfigureAwait(false);

        return Ok(resource);
    }

    [HttpGet("{reviewId:guid}/comments")]
    [SwaggerOperation(
        Summary = "Listar comentarios de una reseña",
        Description = "Retorna todos los comentarios de una reseña.",
        OperationId = "GetReviewComments")]
    public async Task<IActionResult> GetCommentsAsync(Guid reviewId, CancellationToken cancellationToken)
    {
        var comments = await reviewQueryService
            .Handle(new GetCommentsByReviewIdQuery(reviewId))
            .ConfigureAwait(false);

        var resources = await reviewEnrichmentService
            .ToCommentResourcesAsync(comments, cancellationToken)
            .ConfigureAwait(false);

        return Ok(resources);
    }

    [Authorize]
    [HttpPost("{reviewId:guid}/comments")]
    [SwaggerOperation(
        Summary = "Comentar una reseña",
        Description = "Agrega un comentario a una reseña existente en nombre del usuario autenticado.",
        OperationId = "AddReviewComment")]
    public async Task<IActionResult> AddCommentAsync(
        Guid reviewId,
        [FromBody] AddCommentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var command = AddCommentCommandFromResourceAssembler.ToCommand(reviewId, userId, resource);
            var comment = await reviewCommandService.Handle(command).ConfigureAwait(false);

            var resourceResult = await reviewEnrichmentService
                .ToCommentResourceAsync(comment, cancellationToken)
                .ConfigureAwait(false);

            return Ok(resourceResult);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token."));

    private Guid? TryGetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var userId) ? userId : null;
    }
}
