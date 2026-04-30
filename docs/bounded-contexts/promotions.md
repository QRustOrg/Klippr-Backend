# Promotions Bounded Context

## Propósito

Gestiona el ciclo de vida de promociones: creación, edición, publicación, cancelación, consulta y validación de disponibilidad.

## Source tree propuesto

```text
Promotions/
├── Promotions.Domain/
│   ├── Aggregates/
│   │   └── Promotion.cs
│   ├── Commands/
│   │   ├── CreatePromotionCommand.cs
│   │   ├── UpdatePromotionCommand.cs
│   │   ├── PublishPromotionCommand.cs
│   │   └── CancelPromotionCommand.cs
│   ├── Queries/
│   │   ├── GetPromotionByIdQuery.cs
│   │   ├── GetActivePromotionsQuery.cs
│   │   └── GetPromotionsByBusinessIdQuery.cs
│   ├── ValueObjects/
│   │   ├── PromotionStatus.cs
│   │   ├── DiscountValue.cs
│   │   └── TimeFrame.cs
│   ├── Services/
│   │   ├── IPromotionCommandService.cs
│   │   └── IPromotionQueryService.cs
│   └── Repositories/
│       └── IPromotionRepository.cs
├── Promotions.Application/
│   └── Services/
│       ├── PromotionCommandService.cs
│       └── PromotionQueryService.cs
├── Promotions.Infrastructure/
│   ├── Persistence/
│   │   └── PromotionRepository.cs
│   └── EventPublishing/
│       └── PromotionEventPublisher.cs
└── Promotions.Interface/
    ├── Controllers/
    │   └── PromotionController.cs
    ├── Resources/
    │   ├── PromotionResource.cs
    │   └── CreatePromotionResource.cs
    ├── Assemblers/
    │   ├── PromotionResourceFromEntityAssembler.cs
    │   └── CreatePromotionCommandFromResourceAssembler.cs
    └── Facade/
        └── PromotionsContextFacade.cs
```
