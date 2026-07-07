using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Services;

namespace Klippr_Backend.Analytics.Interface.Facade;

public class AnalyticsContextFacade(IAnalyticsCommandService commandService)
{
    public Task RegisterRedemptionAsync(Guid campaignId, Guid businessId)
    {
        return commandService.Handle(new UpdateMetricsCommand(
            campaignId,
            businessId,
            ViewsToAdd: null,
            RedemptionsToAdd: 1,
            NewRating: null));
    }
}
