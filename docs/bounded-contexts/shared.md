# Shared Kernel

## Propósito

Contiene componentes transversales compartidos entre bounded contexts: excepciones, persistencia común, configuración de SQLite/EF Core, mensajería y eventos de integración.

## Source tree propuesto

```text
Shared/
├── Exceptions/
├── Infrastructure/
│   └── Database/
│       └── SqliteContext.cs
├── Messaging/
│   ├── EventBus.cs
│   └── IntegrationEvents/
└── Common/
```
