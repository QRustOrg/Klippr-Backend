using System.Net.Mime;
using Klippr_Backend.Community.Domain.Model.Commands;
using Klippr_Backend.Community.Domain.Model.Queries;
using Klippr_Backend.Community.Domain.Services;
using Klippr_Backend.Community.Interfaces.REST.Resources;
using Klippr_Backend.Community.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Community.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Review Endpoints")]
public class ReviewsController(
    IReviewCommandService reviewCommandService,
    IReviewQueryServices reviewQueryServices) : ControllerBase
{
    /// Get Review by id
    [HttpGet("{reviewId:int}")]
    [SwaggerOperation(
        Summary = "Get a Review by Id",
        Description = "Get a Review by its Id",
        OperationId = "GetReviewById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of Reviews", typeof(ReviewResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No Reviews found")]
    public async Task<IActionResult> GetReviewById(int reviewId)
    {
        var getReviewByIdQuery = new GetReviewByIdQuery(reviewId);
        var review = await reviewQueryServices.Handle(getReviewByIdQuery);
        if (review is null) return NotFound();
        var resource = ReviewResourceFromEntityAssembler.ToResourceFromEntity(review);
        return Ok(resource);
    }
    
    /// Get all Reviews
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all Reviews",
        Description = "Get all Reviews",
        OperationId = "GetAllReviews")]
    [SwaggerResponse(StatusCodes.Status200OK, "The list of Reviews", typeof(IEnumerable<ReviewResource>))]
    public async Task<IActionResult> GetAllReviews()
    {
        var reviews = await reviewQueryServices.Handle(new GetAllReviewQuery());
        var reviewResources = reviews.Select(ReviewResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(reviewResources);
    }
    
    /// Create a new Review
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new Review",
        Description = "Create a new Review",
        OperationId = "CreateReview")]
    [SwaggerResponse(StatusCodes.Status201Created, "The Review was created", typeof(ReviewResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The Review could not be created")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewResource resource)
    {
        var createReviewCommand = CreateReviewCommandFromResourceAssembler.ToCommandFromResource(resource);
        var review = await reviewCommandService.Handle(createReviewCommand);
        if (review is null) return BadRequest();
        var reviewResource = ReviewResourceFromEntityAssembler.ToResourceFromEntity(review);
        return CreatedAtAction(nameof(GetReviewById), new { reviewId = review.Id }, reviewResource);
    }
    
    /// Update a new Review
    [HttpPut("{reviewId:int}")]
    [SwaggerOperation(
        Summary = "Update Review by Id",
        Description = "Update all fields of a Review by its Id",
        OperationId = "UpdateReviewById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The Review was updated", typeof(ReviewResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Review not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The Review could not be updated")]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] UpdateReviewResource resource)
    {
        var command = UpdateReviewCommandFromResourceAssembler.ToCommandFromResource(reviewId, resource);
        var review = await reviewCommandService.Handle(command);
        if (review is null) return NotFound();
        var reviewResource = ReviewResourceFromEntityAssembler.ToResourceFromEntity(review);
        return Ok(reviewResource);
    }
    
    /// Delete a new Review
    [HttpDelete("{reviewId:int}")]
    [SwaggerOperation(
        Summary = "Delete Review by Id",
        Description = "Delete a Review by its Id",
        OperationId = "DeleteReviewById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The Review was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Review not found")]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var command = new DeleteReviewByIdCommand(reviewId);
        var result = await reviewCommandService.Handle(command);
        if (!result) return NotFound();
        return NoContent();
    }
}