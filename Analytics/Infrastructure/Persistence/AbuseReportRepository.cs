using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Klippr_Backend.Analytics.Domain.ValueObjects;

namespace Klippr_Backend.Analytics.Infrastructure.Persistence;

public class AbuseReportRepository : IAbuseReportRepository
{
    private readonly AnalyticsDbContext _context;

    public AbuseReportRepository(AnalyticsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AbuseReport report)
    {
        await _context.AbuseReports.AddAsync(report);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AbuseReport>> FindAllAsync()
    {
        return await _context.AbuseReports
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AbuseReport>> FindByStatusAsync(AbuseReportStatus status)
    {
        return await _context.AbuseReports
            .Where(r => r.Status == status)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }
}