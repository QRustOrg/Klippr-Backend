using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Domain.Services;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Favorites.Application.Services;

public class FavoriteCommandService(
    IFavoriteRepository favoriteRepository,
    IUnitOfWork         unitOfWork)
    : IFavoriteCommandService
{
    public async Task<Favorite?> Handle(SaveFavoriteCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UserId) ||
            string.IsNullOrWhiteSpace(command.PromotionId))
            return null;

        var existing = await favoriteRepository.FindByUserAndPromotionAsync(command.UserId, command.PromotionId);
        if (existing is { IsArchived: false })
            return null;

        if (existing is { IsArchived: true })
        {
            existing.Restore(command.UserId);
            try
            {
                favoriteRepository.Update(existing);
                await unitOfWork.CompleteAsync();
                return existing;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FavoriteCommandService] Restore existing error: {ex.Message}");
                throw;
            }
        }

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

    public async Task<bool> Handle(ArchiveFavoriteCommand command)
    {
        var favorite = await favoriteRepository.FindByFavoriteIdAsync(command.FavoriteId);
        if (favorite is null || !favorite.Archive(command.UserId))
            return false;

        try
        {
            favoriteRepository.Update(favorite);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FavoriteCommandService] Archive error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> Handle(RestoreFavoriteCommand command)
    {
        var favorite = await favoriteRepository.FindByFavoriteIdAsync(command.FavoriteId);
        if (favorite is null || !favorite.Restore(command.UserId))
            return false;

        try
        {
            favoriteRepository.Update(favorite);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FavoriteCommandService] Restore error: {ex.Message}");
            return false;
        }
    }
}
