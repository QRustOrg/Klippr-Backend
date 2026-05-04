# Profile Bounded Context

## Overview

The Profile bounded context manages user profiles for both consumers and businesses within the Klippr platform. It handles profile creation, updates, verification, and rating aggregation using Clean Architecture principles and Domain-Driven Design.

## Architecture

The context is organized into four distinct layers following Clean Architecture:

### 1. Domain Layer (`Domain/`)
Contains core business logic without external dependencies:

- **Aggregates**: `ConsumerProfile` and `BusinessProfile` - root entities managing profile data
- **Value Objects**: 
  - `Location` - Immutable geographic information
  - `VerificationStatus` - Profile verification state (Pending, Approved, Rejected)
  - `BusinessCategory` - Business classification
  - `Rating` - Business rating metrics
  - `SavingsStatistics` - Consumer savings tracking
- **Commands**: Intent specifications for profile operations
  - `CreateConsumerProfileCommand`
  - `UpdateConsumerProfileCommand`
  - `CreateBusinessProfileCommand`
  - `UpdateBusinessProfileCommand`
  - `SubmitBusinessVerificationCommand`
  - `ApproveBusinessVerificationCommand`
- **Queries**: Data retrieval specifications
  - `GetConsumerProfileByUserIdQuery`
  - `GetBusinessProfileByUserIdQuery`
  - `GetVerificationStatusQuery`
  - `GetBusinessRatingQuery`
  - `GetProfilesWithVerificationPendingQuery`
- **Services**: Domain service contracts
  - `IConsumerProfileCommandService` - Consumer profile operations
  - `IBusinessProfileCommandService` - Business profile operations
  - `IProfileQueryService` - Profile queries
- **Repositories**: Data access contracts
  - `IConsumerProfileRepository`
  - `IBusinessProfileRepository`
- **Events**: Domain event definitions
  - `ProfileUpdated`
  - `VerificationDocumentSubmitted`

### 2. Application Layer (`Application/`)
Orchestrates domain logic and handles cross-cutting concerns:

- **Services**:
  - `ConsumerProfileCommandService` - Implements consumer profile creation/updates
  - `BusinessProfileCommandService` - Implements business profile operations
  - `ProfileQueryService` - Implements all query operations
- **OutboundServices**: Contracts for infrastructure dependencies
  - `IVerificationService` - Document verification
  - `IEventPublisher` - Domain event publishing
  - `IRatingAggregator` - Business rating aggregation

### 3. Infrastructure Layer (`Infrastructure/`)
Implements technical concerns and external integrations:

- **Persistence**:
  - `ConsumerProfileRepository` - EF Core implementation
  - `BusinessProfileRepository` - EF Core implementation
  - `ProfileDbContext` - Entity Framework configuration
- **Verification**:
  - `VerificationService` - Document validation logic
  - `DocumentStorageService` - File storage operations
- **EventPublishing**:
  - `ProfileEventPublisher` - Event publishing implementation
  - `RatingAggregatorService` - Rating calculation
- **Pipeline**:
  - `ProfileEnrichmentMiddleware` - Request/response enrichment
  - `LocationCachingMiddleware` - Geographic data caching
- **ServiceCollectionExtensions** - Dependency injection setup

### 4. Interface Layer (`Interface/`)
Exposes API endpoints and external contracts:

- **Controllers**:
  - `ProfileController` - Profile CRUD operations (public endpoints)
  - `VerificationController` - Business verification endpoints
  - `AdminProfileController` - Admin operations
- **Resources**: Data transfer objects for API communication
  - `ConsumerProfileResource`
  - `UpdateConsumerProfileResource`
  - `BusinessProfileResource`
  - `UpdateBusinessProfileResource`
  - `VerificationDocumentResource`
  - `LocationResource`, `BusinessCategoryResource`, `SavingsStatisticsResource`, `BusinessRatingResource`
- **Assemblers**: Converters between domain models and resources
  - `ConsumerProfileResourceFromEntityAssembler`
  - `CreateConsumerProfileCommandFromResourceAssembler`
  - `BusinessProfileResourceFromEntityAssembler`
  - `VerificationDocumentCommandFromResourceAssembler`
- **Facade**: `ProfileContextFacade` - Simplified external consumption interface

## API Endpoints

### Consumer Profiles

#### Create Consumer Profile
```http
POST /api/profiles/consumer
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

#### Get Consumer Profile
```http
GET /api/profiles/consumer/{profileId}
Authorization: Bearer {token}
```

#### Update Consumer Profile
```http
PUT /api/profiles/consumer
Authorization: Bearer {token}
Content-Type: application/json

{
  "profileId": "guid",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "street": "123 Main St",
  "city": "New York",
  "state": "NY",
  "country": "USA",
  "zipCode": "10001"
}
```

### Business Profiles

#### Create Business Profile
```http
POST /api/profiles/business
Authorization: Bearer {token}
Content-Type: application/json

{
  "businessName": "ACME Corp",
  "taxId": "12-3456789",
  "category": "Retail"
}
```

#### Get Business Profile
```http
GET /api/profiles/business/{profileId}
Authorization: Bearer {token}
```

#### Update Business Profile
```http
PUT /api/profiles/business
Authorization: Bearer {token}
Content-Type: application/json
```

### Verification

#### Submit Verification
```http
POST /api/verification/submit
Authorization: Bearer {token}
Content-Type: application/json

{
  "profileId": "guid",
  "documentUrl": "https://storage.example.com/document.pdf"
}
```

#### Approve Verification (Admin Only)
```http
POST /api/verification/approve
Authorization: Bearer {token}
Content-Type: application/json
```

### Admin Operations

#### Get Pending Verifications
```http
GET /api/admin/profiles/pending-verification?pageNumber=1&pageSize=10
Authorization: Bearer {token}
Roles: ADMIN
```

#### Get User Profiles
```http
GET /api/admin/profiles/by-user/{userId}
Authorization: Bearer {token}
Roles: ADMIN
```

## Integration with Main Application

### 1. Add to `appsettings.json`
```json
{
  "ConnectionStrings": {
    "ProfileConnection": "Data Source=klippr_profile.db"
  },
  "Jwt": {
    "Authority": "https://your-auth-server",
    "SecretKey": "your-secret-key"
  }
}
```

### 2. Add to `Program.cs`
```csharp
using Infrastructure;

// Register Profile services
var profileConnectionString = builder.Configuration.GetConnectionString("ProfileConnection");
builder.Services.AddProfileServices(profileConnectionString);

// Add controllers
builder.Services.AddControllers();
```

### 3. Use Profile Context
```csharp
using Interface.Facade;

public class YourService
{
    private readonly ProfileContextFacade _profileFacade;

    public YourService(ProfileContextFacade profileFacade)
    {
        _profileFacade = profileFacade;
    }

    public async Task YourMethod()
    {
        var profile = await _profileFacade.GetConsumerProfileByUserIdAsync(userId);
        // Use profile data
    }
}
```

## Database

The Profile context uses SQLite by default. Configure the database connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=klippr_profile.db"
  }
}
```

### Tables
- `ConsumerProfiles` - Consumer profile data
- `BusinessProfiles` - Business profile data

## Configuration

See `appsettings.example.json` for all available configuration options:

- **ConnectionStrings**: Database connection settings
- **Jwt**: JWT token configuration
- **Storage**: File storage service settings
- **Verification**: Business verification options

## Testing

Use `Profile.http` for REST client testing. Update the `@token` variable with a valid JWT token before testing.

## Key Features

1. **Consumer Profiles**: Manage basic user profiles with location and savings tracking
2. **Business Profiles**: Handle business-specific data including categories and locations
3. **Verification System**: Submit and approve business verification documents
4. **Rating System**: Aggregate and display business ratings
5. **Admin Operations**: Monitor pending verifications and user profiles
6. **Event Publishing**: Publish domain events for cross-context communication
7. **Location Caching**: Optimize geographic data access

## Dependencies

- .NET 8.0
- Entity Framework Core 8.0
- ASP.NET Core 8.0
- Microsoft.IdentityModel.Tokens (JWT integration)

## Project Files

- `Domain.csproj` - Domain layer (no external dependencies)
- `Application.csproj` - Application layer (depends on Domain)
- `Infrastructure.csproj` - Infrastructure layer (depends on Domain, Application)
- `Interface.csproj` - Interface layer (depends on all layers)

## Design Principles

- **SOLID Principles**: Single responsibility, Open/Closed, Liskov substitution, Interface segregation, Dependency inversion
- **Domain-Driven Design**: Aggregates, value objects, bounded context boundaries
- **CQRS Pattern**: Separation of commands and queries
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loosely coupled components
- **Immutability**: Value objects and read-only aggregates

## Related Contexts

- **IAM Context**: Provides user authentication and authorization
- **Ratings Context** (future): Detailed business ratings and reviews
- **Notifications Context** (future): Profile change notifications
