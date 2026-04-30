# Community Bounded Context

## Propósito

Gestiona reseñas, calificaciones, comentarios, moderación y reputación social de negocios/promociones.

## Source tree propuesto

```text
Community/
├── Community.Domain/
│   ├── Aggregates/
│   │   └── Review.cs
│   ├── Commands/
│   │   ├── CreateReviewCommand.cs
│   │   └── ModerateReviewCommand.cs
│   ├── Queries/
│   │   ├── GetReviewsByBusinessIdQuery.cs
│   │   └── GetReviewsByConsumerIdQuery.cs
│   ├── ValueObjects/
│   │   ├── Rating.cs
│   │   └── ReviewContent.cs
│   ├── Services/
│   │   ├── IReviewCommandService.cs
│   │   └── IReviewQueryService.cs
│   └── Repositories/
│       └── IReviewRepository.cs
├── Community.Application/
│   └── Services/
│       ├── ReviewCommandService.cs
│       └── ReviewQueryService.cs
├── Community.Infrastructure/
│   ├── Persistence/
│   │   └── ReviewRepository.cs
│   └── EventPublishing/
│       └── ReviewEventPublisher.cs
└── Community.Interface/
    ├── Controllers/
    │   └── ReviewController.cs
    ├── Resources/
    │   ├── ReviewResource.cs
    │   └── CreateReviewResource.cs
    ├── Assemblers/
    │   ├── ReviewResourceFromEntityAssembler.cs
    │   └── CreateReviewCommandFromResourceAssembler.cs
    └── Facade/
        └── CommunityContextFacade.cs
```
