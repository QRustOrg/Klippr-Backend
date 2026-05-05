using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Domain.Repositories;


public interface IAbuseReportRepository
{
    Task AddAsync(AbuseReport report);
    Task<IEnumerable<AbuseReport>> FindAllAsync();
    Task<IEnumerable<AbuseReport>> FindByStatusAsync(AbuseReportStatus status);
}