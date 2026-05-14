using System.Net.Mime;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Queries;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Interface.Assemblers;
using Klippr_Backend.Favorites.Interface.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Favorites.Interface.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class FavoritesController(
    IFavoriteCommandService commandService,
    IFavoriteQueryService   queryService) : ControllerBase
{
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetFavoritesByUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest();
        var items = (await queryService.Handle(new GetUserFavoritesQuery(userId)))
            .Select(FavoriteResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();
        return Ok(new FavoriteListResource(userId, items.Count, items.AsReadOnly()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFavoriteById(int id)
    {
        var favorite = await queryService.Handle(id);
        if (favorite is null) return NotFound();
        return Ok(FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite));
    }

    [HttpPost]
    public async Task<IActionResult> SaveFavorite([FromBody] SaveFavoriteResource resource)
    {
        var command  = SaveFavoriteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var favorite = await commandService.Handle(command);
        if (favorite is null)
            return Conflict(new { message = "This promotion is already in the user's favorites." });
        var result = FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite);
        return CreatedAtAction(nameof(GetFavoriteById), new { id = favorite.Id }, result);
    }

    [HttpDelete("{favoriteId}")]
    public async Task<IActionResult> RemoveFavorite(string favoriteId, [FromQuery] string userId)
    {
        var removed = await commandService.Handle(new RemoveFavoriteCommand(userId, favoriteId));
        return removed ? NoContent() : NotFound();
    }
}