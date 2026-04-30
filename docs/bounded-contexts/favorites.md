# Favorites Bounded Context

## Propósito

Gestiona promociones guardadas por usuarios, permitiendo guardar, eliminar y consultar favoritos.

## Source tree propuesto

```text
Favorites/
├── Favorites.Domain/
│   ├── Aggregates/
│   │   └── Favorite.cs
│   ├── Commands/
│   │   ├── SaveFavoriteCommand.cs
│   │   └── RemoveFavoriteCommand.cs
│   ├── Queries/
│   │   └── GetUserFavoritesQuery.cs
│   ├── ValueObjects/
│   │   ├── FavoriteId.cs
│   │   ├── UserId.cs
│   │   └── PromotionId.cs
│   ├── Services/
│   │   ├── IFavoriteCommandService.cs
│   │   └── IFavoriteQueryService.cs
│   └── Repositories/
│       └── IFavoriteRepository.cs
├── Favorites.Application/
│   └── Services/
│       ├── FavoriteCommandService.cs
│       └── FavoriteQueryService.cs
├── Favorites.Infrastructure/
│   └── Persistence/
│       └── FavoriteRepository.cs
└── Favorites.Interface/
    ├── Controllers/
    │   └── FavoriteController.cs
    ├── Resources/
    │   ├── FavoriteResource.cs
    │   └── FavoriteListResource.cs
    ├── Assemblers/
    │   └── FavoriteResourceFromEntityAssembler.cs
    └── Facade/
        └── FavoritesContextFacade.cs
```
