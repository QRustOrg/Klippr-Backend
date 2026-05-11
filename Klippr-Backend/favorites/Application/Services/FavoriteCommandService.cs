using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;

namespace Klippr_Backend.Favorites.Application.Services;

public class FavoriteCommandService(
    IFavoriteRepository  favoriteRepository,
    IFavoriteUnitOfWork  unitOfWork)           // ← own interface, not from Shared
    : IFavoriteCommandService
{
    public async Task<Favorite?> Handle(SaveFavoriteCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UserId) ||
            string.IsNullOrWhiteSpace(command.PromotionId))
            return null;

        if (await favoriteRepository.ExistsAsync(command.UserId, command.PromotionId))
            return null;

        var favorite = new Favorite(command);
        try
        {
            await favoriteRepository.AddAsync(favorite);
            await unitOfWork.CompleteAsync();
            return favorite;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FavoriteCommandService] Save error: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> Handle(RemoveFavoriteCommand command)
    {
        var favorite = await favoriteRepository.FindByFavoriteIdAsync(command.FavoriteId);

        if (favorite is null || !favorite.BelongsToUser(command.UserId))
            return false;

        try
        {
            favoriteRepository.Remove(favorite);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FavoriteCommandService] Remove error: {ex.Message}");
            return false;
        }
    }
}