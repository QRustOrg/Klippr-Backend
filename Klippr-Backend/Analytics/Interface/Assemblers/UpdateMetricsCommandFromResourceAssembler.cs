using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Interface.Resources;

namespace Klippr_Backend.Analytics.Interface.Assemblers;

public static class UpdateMetricsCommandFromResourceAssembler
{
    public static UpdateMetricsCommand ToCommand(UpdateMetricsResource resource)
    {
        return new UpdateMetricsCommand(
            resource.CampaignId,
            resource.BusinessId,
            resource.ViewsToAdd,
            resource.RedemptionsToAdd,
            resource.NewRating
        );
    }
}