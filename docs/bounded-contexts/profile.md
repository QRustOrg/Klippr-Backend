# Profile Bounded Context

## Propósito

Gestiona perfiles de consumidores y negocios afiliados, datos personales, ubicación, verificación del negocio, rating y estadísticas asociadas.

## Source tree propuesto

```text
Profile/
├── Profile.Domain/
│   ├── Aggregates/
│   │   ├── ConsumerProfile.cs
│   │   └── BusinessProfile.cs
│   ├── Commands/
│   │   ├── CreateConsumerProfileCommand.cs
│   │   ├── UpdateConsumerProfileCommand.cs
│   │   ├── CreateBusinessProfileCommand.cs
│   │   ├── UpdateBusinessProfileCommand.cs
│   │   ├── SubmitBusinessVerificationCommand.cs
│   │   └── ApproveBusinessVerificationCommand.cs
│   ├── Queries/
│   │   ├── GetConsumerProfileByUserIdQuery.cs
│   │   ├── GetBusinessProfileByUserIdQuery.cs
│   │   ├── GetVerificationStatusQuery.cs
│   │   ├── GetBusinessRatingQuery.cs
│   │   └── GetProfilesWithVerificationPendingQuery.cs
│   ├── ValueObjects/
│   │   ├── VerificationStatus.cs
│   │   ├── Location.cs
│   │   ├── BusinessCategory.cs
│   │   ├── Rating.cs
│   │   └── SavingsStatistics.cs
│   ├── Services/
│   │   ├── IConsumerProfileCommandService.cs
│   │   ├── IBusinessProfileCommandService.cs
│   │   └── IProfileQueryService.cs
│   ├── Repositories/
│   │   ├── IConsumerProfileRepository.cs
│   │   └── IBusinessProfileRepository.cs
│   └── Events/
│       ├── ProfileUpdated.cs
│       └── VerificationDocumentSubmitted.cs
├── Profile.Application/
│   ├── Services/
│   │   ├── ConsumerProfileCommandService.cs
│   │   └── BusinessProfileCommandService.cs
│   ├── OutboundServices/
│   │   ├── IVerificationService.cs
│   │   ├── IEventPublisher.cs
│   │   └── IRatingAggregator.cs
│   └── QueryServices/
│       └── ProfileQueryService.cs
├── Profile.Infrastructure/
│   ├── Persistence/
│   │   ├── ConsumerProfileRepository.cs
│   │   └── BusinessProfileRepository.cs
│   ├── Verification/
│   │   ├── VerificationService.cs
│   │   └── DocumentStorageService.cs
│   ├── Pipeline/
│   │   ├── ProfileEnrichmentMiddleware.cs
│   │   └── LocationCachingMiddleware.cs
│   └── EventPublishing/
│       ├── ProfileEventPublisher.cs
│       └── RatingAggregatorService.cs
└── Profile.Interface/
    ├── Controllers/
    │   ├── ProfileController.cs
    │   ├── VerificationController.cs
    │   └── AdminProfileController.cs
    ├── Resources/
    │   ├── ConsumerProfileResource.cs
    │   ├── UpdateConsumerProfileResource.cs
    │   ├── BusinessProfileResource.cs
    │   ├── UpdateBusinessProfileResource.cs
    │   └── VerificationDocumentResource.cs
    ├── Assemblers/
    │   ├── ConsumerProfileResourceFromEntityAssembler.cs
    │   ├── CreateConsumerProfileCommandFromResourceAssembler.cs
    │   ├── BusinessProfileResourceFromEntityAssembler.cs
    │   └── VerificationDocumentCommandFromResourceAssembler.cs
    └── Facade/
        └── ProfileContextFacade.cs
```
