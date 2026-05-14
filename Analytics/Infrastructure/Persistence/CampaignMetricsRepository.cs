using Klippr_Backend.Analytics.Domain.Aggregates;
using Klippr_Backend.Analytics.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Analytics.Infrastructure.Persistence;

public class CampaignMetricsRepository : ICampaignMetricsRepository
{
    private readonly AnalyticsDbContext _context;

    public CampaignMetricsRepository(AnalyticsDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CampaignMetrics>> FindByBusinessIdAsync(Guid businessId)
    {
        return await _context.CampaignMetrics
            .Where(m => m.BusinessId == businessId)
            .ToListAsync();
    }

    public async Task<CampaignMetrics?> FindByCampaignIdAsync(Guid campaignId)
    {
        return await _context.CampaignMetrics
            .FirstOrDefaultAsync(m => m.CampaignId == campaignId);
    }

    public async Task AddAsync(CampaignMetrics metrics)
    {
        await _context.CampaignMetrics.AddAsync(metrics);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CampaignMetrics metrics)
    {
        _context.CampaignMetrics.Update(metrics);
        await _context.SaveChangesAsync();
    }
}