
using Klippr_Backend.Analytics.Interface.Resources;
using System.Linq;

namespace Klippr_Backend.Analytics.Interface.Assemblers;

public static class AbuseReportResourceFromEntityAssembler
{
    public static AbuseReportResource ToResourceFromEntity(AbuseReport entity)
    {
        return new AbuseReportResource(
            entity.Id.Value,
            entity.ReportedEntityId,
            entity.ReportedBy,
            entity.Reason,
            entity.Description,
            entity.Status.ToString(),
            entity.CreatedAt
        );
    }

    public static IEnumerable<AbuseReportResource> ToResourceList(IEnumerable<AbuseReport> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }
}