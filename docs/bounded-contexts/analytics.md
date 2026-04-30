# Analytics Bounded Context

## Propósito

Gestiona métricas de campañas, dashboards de negocio, reportes de abuso y estadísticas para administración.

## Source tree propuesto

```text
Analytics/
├── Analytics.Domain/
│   ├── Aggregates/
│   │   ├── CampaignMetrics.cs
│   │   └── AbuseReport.cs
│   ├── Commands/
│   │   ├── UpdateMetricsCommand.cs
│   │   └── RegisterAbuseReportCommand.cs
│   ├── Queries/
│   │   ├── GetCampaignMetricsQuery.cs
│   │   ├── GetBusinessDashboardQuery.cs
│   │   └── GetAbuseReportsQuery.cs
│   ├── ValueObjects/
│   │   ├── MetricType.cs
│   │   └── TimeRange.cs
│   ├── Services/
│   │   ├── IAnalyticsCommandService.cs
│   │   └── IAnalyticsQueryService.cs
│   └── Repositories/
│       ├── ICampaignMetricsRepository.cs
│       └── IAbuseReportRepository.cs
├── Analytics.Application/
│   └── Services/
│       ├── AnalyticsCommandService.cs
│       └── AnalyticsQueryService.cs
├── Analytics.Infrastructure/
│   └── Persistence/
│       ├── CampaignMetricsRepository.cs
│       └── AbuseReportRepository.cs
└── Analytics.Interface/
    ├── Controllers/
    │   ├── AnalyticsController.cs
    │   └── AdminAnalyticsController.cs
    ├── Resources/
    │   ├── CampaignMetricsResource.cs
    │   ├── BusinessDashboardResource.cs
    │   └── AbuseReportResource.cs
    ├── Assemblers/
    │   ├── CampaignMetricsResourceFromEntityAssembler.cs
    │   └── AbuseReportResourceFromEntityAssembler.cs
    └── Facade/
        └── AnalyticsContextFacade.cs
```
