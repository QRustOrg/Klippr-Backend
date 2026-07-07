using System.Net.Mime;
using System.Security.Claims;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Queries;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Interface.Assemblers;
using Klippr_Backend.Favorites.Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Favorites.Interface.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class FavoritesController(
    IFavoriteCommandService commandService,
    IFavoriteQueryService   queryService) : ControllerBase
{
    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetFavoritesByUser(string userId, [FromQuery] bool archived = false)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest();
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();

        var items = (await queryService.Handle(new GetUserFavoritesQuery(userId, archived)))
            .Select(FavoriteResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();
        return Ok(new FavoriteListResource(userId, items.Count, items.AsReadOnly()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFavoriteById(int id)
    {
        var favorite = await queryService.Handle(id);
        if (favorite is null || !favorite.BelongsToUser(GetUserId())) return NotFound();
        return Ok(FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite));
    }

    [HttpPost]
    public async Task<IActionResult> SaveFavorite([FromBody] SaveFavoriteResource resource)
    {
        var command  = SaveFavoriteCommandFromResourceAssembler.ToCommandFromResource(resource) with { UserId = GetUserId() };
        var favorite = await commandService.Handle(command);
        if (favorite is null)
            return Conflict(new { message = "This promotion is already in the user's favorites." });
        var result = FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite);
        return CreatedAtAction(nameof(GetFavoriteById), new { id = favorite.Id }, result);
    }

    [HttpDelete("{favoriteId}")]
    public async Task<IActionResult> RemoveFavorite(string favoriteId, [FromQuery] string userId)
    {
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();

        var removed = await commandService.Handle(new RemoveFavoriteCommand(userId, favoriteId));
        return removed ? NoContent() : NotFound();
    }

    [HttpPatch("{favoriteId}/archive")]
    public async Task<IActionResult> ArchiveFavorite(string favoriteId, [FromQuery] string userId)
    {
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();

        var archived = await commandService.Handle(new ArchiveFavoriteCommand(userId, favoriteId));
        return archived ? NoContent() : NotFound();
    }

    [HttpPatch("{favoriteId}/restore")]
    public async Task<IActionResult> RestoreFavorite(string favoriteId, [FromQuery] string userId)
    {
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();

        var restored = await commandService.Handle(new RestoreFavoriteCommand(userId, favoriteId));
        return restored ? NoContent() : NotFound();
    }
}
