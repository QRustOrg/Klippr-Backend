# Redemption Bounded Context

## Propósito

Gestiona el canje de promociones mediante QR o código único, validación de disponibilidad, bloqueo post-canje y consulta de historial.

## Source tree propuesto

```text
Redemption/
├── Redemption.Domain/
│   ├── Aggregates/
│   │   └── Redemption.cs
│   ├── Commands/
│   │   ├── RedeemPromotionCommand.cs
│   │   └── ConfirmRedemptionCommand.cs
│   ├── Queries/
│   │   ├── GetRedemptionByIdQuery.cs
│   │   ├── GetRedemptionsByConsumerIdQuery.cs
│   │   └── GetRedemptionsByBusinessIdQuery.cs
│   ├── ValueObjects/
│   │   ├── RedemptionStatus.cs
│   │   └── RedemptionCode.cs
│   ├── Services/
│   │   ├── IRedemptionCommandService.cs
│   │   └── IRedemptionQueryService.cs
│   └── Repositories/
│       └── IRedemptionRepository.cs
├── Redemption.Application/
│   └── Services/
│       ├── RedemptionCommandService.cs
│       └── RedemptionQueryService.cs
├── Redemption.Infrastructure/
│   ├── Persistence/
│   │   └── RedemptionRepository.cs
│   └── EventPublishing/
│       └── RedemptionEventPublisher.cs
└── Redemption.Interface/
    ├── Controllers/
    │   └── RedemptionController.cs
    ├── Resources/
    │   ├── RedemptionResource.cs
    │   └── RedeemPromotionResource.cs
    ├── Assemblers/
    │   ├── RedemptionResourceFromEntityAssembler.cs
    │   └── RedeemPromotionCommandFromResourceAssembler.cs
    └── Facade/
        └── RedemptionContextFacade.cs
```
