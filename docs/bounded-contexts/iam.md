# IAM Bounded Context

## Propósito

Gestiona autenticación, registro, autorización, usuarios, roles, hashing de contraseñas y emisión de tokens JWT.

## Source tree propuesto

```text
IAM/
├── IAM.Domain/
│   ├── Aggregates/
│   │   └── User.cs
│   ├── Commands/
│   │   ├── SignInCommand.cs
│   │   ├── SignUpConsumerCommand.cs
│   │   └── SignUpBusinessCommand.cs
│   ├── Queries/
│   │   ├── GetAllUsersQuery.cs
│   │   ├── GetUserByEmailQuery.cs
│   │   ├── GetUserByIdQuery.cs
│   │   └── GetUsersByRoleQuery.cs
│   ├── ValueObjects/
│   │   ├── Role.cs
│   │   └── Email.cs
│   ├── Services/
│   │   ├── IUserCommandService.cs
│   │   └── IUserQueryService.cs
│   ├── Repositories/
│   │   └── IUserRepository.cs
│   └── Events/
├── IAM.Application/
│   ├── Services/
│   │   ├── UserCommandService.cs
│   │   └── UserQueryService.cs
│   └── OutboundServices/
│       ├── Hashing/
│       │   └── IHashingService.cs
│       └── Tokens/
│           └── ITokenService.cs
├── IAM.Infrastructure/
│   ├── Persistence/
│   │   └── UserRepository.cs
│   ├── Hashing/
│   │   └── HashingService.cs
│   ├── Tokens/
│   │   └── TokenService.cs
│   └── Pipeline/
│       └── AuthorizationMiddleware.cs
└── IAM.Interface/
    ├── Controllers/
    │   ├── AuthenticationController.cs
    │   └── UsersController.cs
    ├── Resources/
    │   ├── SignInResource.cs
    │   ├── SignUpConsumerResource.cs
    │   ├── SignUpBusinessResource.cs
    │   ├── AuthenticatedUserResource.cs
    │   └── UserResource.cs
    ├── Assemblers/
    │   ├── SignInCommandFromResourceAssembler.cs
    │   ├── SignUpConsumerCommandFromResourceAssembler.cs
    │   ├── SignUpBusinessCommandFromResourceAssembler.cs
    │   ├── AuthenticatedUserResourceFromEntityAssembler.cs
    │   └── UserResourceFromEntityAssembler.cs
    └── Facade/
        └── IamContextFacade.cs
```
