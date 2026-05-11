using System.Net.Mime;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Queries;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Favorites.Interface.Assemblers;
using Klippr_Backend.Favorites.Interface.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Favorites.Interface.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Favorites Endpoints")]
public class FavoritesController(
    IFavoriteCommandService commandService,
    IFavoriteQueryService   queryService) : ControllerBase
{
    /// <summary>Returns all favorites saved by a user.</summary>
    [HttpGet("user/{userId}")]
    [SwaggerOperation("GetFavoritesByUser")]
    [SwaggerResponse(200, "Favorites list", typeof(FavoriteListResource))]
    [SwaggerResponse(400, "Invalid userId")]
    public async Task<IActionResult> GetFavoritesByUser(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest();

        var items = (await queryService.Handle(new GetUserFavoritesQuery(userId)))
            .Select(FavoriteResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        return Ok(new FavoriteListResource(userId, items.Count, items.AsReadOnly()));
    }

    /// <summary>Returns a single favorite by its internal id.</summary>
    [HttpGet("{id:int}")]
    [SwaggerOperation("GetFavoriteById")]
    [SwaggerResponse(200, "Favorite found",    typeof(FavoriteResource))]
    [SwaggerResponse(404, "Favorite not found")]
    public async Task<IActionResult> GetFavoriteById(int id)
    {
        var favorite = await queryService.Handle(id);
        if (favorite is null) return NotFound();
        return Ok(FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite));
    }

    /// <summary>Saves a promotion as a favorite.</summary>
    [HttpPost]
    [SwaggerOperation("SaveFavorite")]
    [SwaggerResponse(201, "Favorite saved",            typeof(FavoriteResource))]
    [SwaggerResponse(400, "Invalid payload")]
    [SwaggerResponse(409, "Already in favorites")]
    public async Task<IActionResult> SaveFavorite([FromBody] SaveFavoriteResource resource)
    {
        var command  = SaveFavoriteCommandFromResourceAssembler.ToCommandFromResource(resource);
        var favorite = await commandService.Handle(command);

        if (favorite is null)
            return Conflict(new { message = "This promotion is already in the user's favorites." });

        var favoriteResource = FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite);
        return CreatedAtAction(nameof(GetFavoriteById), new { id = favorite.Id }, favoriteResource);
    }

    /// <summary>Removes a promotion from a user's favorites.</summary>
    [HttpDelete("{favoriteId}")]
    [SwaggerOperation("RemoveFavorite")]
    [SwaggerResponse(204, "Favorite removed")]
    [SwaggerResponse(404, "Favorite not found or user mismatch")]
    public async Task<IActionResult> RemoveFavorite(string favoriteId, [FromQuery] string userId)
    {
        var removed = await commandService.Handle(new RemoveFavoriteCommand(userId, favoriteId));
        return removed ? NoContent() : NotFound();
    }
}
