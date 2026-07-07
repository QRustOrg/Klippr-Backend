using Klippr_Backend.Favorites.Application.Services;
using Klippr_Backend.Favorites.Domain.Aggregates;
using Klippr_Backend.Favorites.Domain.Commands;
using Klippr_Backend.Favorites.Domain.Queries;
using Klippr_Backend.Favorites.Domain.Repositories;
using Klippr_Backend.Favorites.Interface.Assemblers;
using Klippr_Backend.Favorites.Interface.Resources;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Tests;

public class FavoriteArchiveTests
{
    [Fact]
    public void FavoriteResourceExposesArchiveState()
    {
        var favorite = new Favorite(new SaveFavoriteCommand("user-1", "promo-1"));

        favorite.Archive("user-1");

        var resource = FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite);

        Assert.True(resource.IsArchived);
    }

    [Fact]
    public async Task QueryServiceRequestsActiveFavoritesByDefault()
    {
        var repository = new FakeFavoriteRepository();
        var service = new FavoriteQueryService(repository);

        await service.Handle(new GetUserFavoritesQuery("user-1"));

        Assert.Equal("user-1", repository.LastUserId);
        Assert.False(repository.LastArchived);
    }

    [Fact]
    public async Task QueryServiceRequestsArchivedFavoritesWhenAsked()
    {
        var repository = new FakeFavoriteRepository();
        var service = new FavoriteQueryService(repository);

        await service.Handle(new GetUserFavoritesQuery("user-1", Archived: true));

        Assert.Equal("user-1", repository.LastUserId);
        Assert.True(repository.LastArchived);
    }

    [Fact]
    public async Task SaveFavoriteRestoresArchivedFavoriteInsteadOfAddingDuplicate()
    {
        var repository = new FakeFavoriteRepository();
        var archived = new Favorite(new SaveFavoriteCommand("user-1", "promo-1"));
        archived.Archive("user-1");
        repository.ExistingByUserAndPromotion = archived;
        var unitOfWork = new FakeUnitOfWork();
        var service = new FavoriteCommandService(repository, unitOfWork);

        var result = await service.Handle(new SaveFavoriteCommand("user-1", "promo-1"));

        Assert.Same(archived, result);
        Assert.False(archived.IsArchived);
        Assert.Equal(0, repository.AddCount);
        Assert.Equal(1, repository.UpdateCount);
        Assert.Equal(1, unitOfWork.CompleteCount);
    }

    [Fact]
    public async Task ArchiveAndRestoreRequireMatchingUser()
    {
        var favorite = new Favorite(new SaveFavoriteCommand("user-1", "promo-1"));
        var repository = new FakeFavoriteRepository { ExistingByFavoriteId = favorite };
        var service = new FavoriteCommandService(repository, new FakeUnitOfWork());

        var wrongUserArchived = await service.Handle(new ArchiveFavoriteCommand("other-user", favorite.FavoriteId));
        var archived = await service.Handle(new ArchiveFavoriteCommand("user-1", favorite.FavoriteId));
        var isArchivedAfterArchive = favorite.IsArchived;
        var wrongUserRestored = await service.Handle(new RestoreFavoriteCommand("other-user", favorite.FavoriteId));
        var restored = await service.Handle(new RestoreFavoriteCommand("user-1", favorite.FavoriteId));

        Assert.False(wrongUserArchived);
        Assert.True(archived);
        Assert.True(isArchivedAfterArchive);
        Assert.False(wrongUserRestored);
        Assert.True(restored);
        Assert.False(favorite.IsArchived);
    }

    private sealed class FakeFavoriteRepository : IFavoriteRepository
    {
        public string? LastUserId { get; private set; }
        public bool LastArchived { get; private set; }
        public Favorite? ExistingByFavoriteId { get; set; }
        public Favorite? ExistingByUserAndPromotion { get; set; }
        public int AddCount { get; private set; }
        public int UpdateCount { get; private set; }

        public Task<IEnumerable<Favorite>> FindByUserIdAsync(string userId, bool archived = false)
        {
            LastUserId = userId;
            LastArchived = archived;
            return Task.FromResult(Enumerable.Empty<Favorite>());
        }

        public Task<Favorite?> FindByFavoriteIdAsync(string favoriteId) =>
            Task.FromResult(ExistingByFavoriteId?.FavoriteId == favoriteId ? ExistingByFavoriteId : null);

        public Task<Favorite?> FindByUserAndPromotionAsync(string userId, string promotionId) =>
            Task.FromResult(
                ExistingByUserAndPromotion?.UserId == userId &&
                ExistingByUserAndPromotion?.PromotionId == promotionId
                    ? ExistingByUserAndPromotion
                    : null);

        public Task<bool> ExistsAsync(string userId, string promotionId) =>
            Task.FromResult(ExistingByUserAndPromotion is { IsArchived: false });

        public Task AddAsync(Favorite entity)
        {
            AddCount++;
            return Task.CompletedTask;
        }

        public Task<Favorite?> FindByIdAsync(int id) => Task.FromResult<Favorite?>(null);

        public void Update(Favorite entity)
        {
            UpdateCount++;
        }

        public void Remove(Favorite entity)
        {
        }

        public Task<IEnumerable<Favorite>> ListAsync() =>
            Task.FromResult(Enumerable.Empty<Favorite>());
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int CompleteCount { get; private set; }

        public Task CompleteAsync()
        {
            CompleteCount++;
            return Task.CompletedTask;
        }
    }
}
