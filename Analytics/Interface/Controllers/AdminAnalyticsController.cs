using System.Net.Mime;
using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Queries;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Domain.ValueObjects;
using Klippr_Backend.Analytics.Interface.Assemblers;
using Klippr_Backend.Analytics.Interface.Resources;
using Klippr_Backend.IAM.Interface.Facade;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Analytics.Interface.Controllers;

/// <summary>
/// Controlador REST para analítica administrativa de la plataforma.
/// </summary>
/// <remarks>
/// Restringido a administradores. Expone moderación de reportes de abuso y totales globales.
/// </remarks>
[ApiController]
[Route("api/admin/analytics")]
[Authorize(Roles = "ADMIN")]
[Produces(MediaTypeNames.Application.Json)]
public class AdminAnalyticsController(
    IAnalyticsQueryService queryService,
    IAnalyticsCommandService commandService,
    IamContextFacade iamContextFacade,
    IPromotionQueryService promotionQueryService) : ControllerBase
{
    [HttpGet("abuse-reports")]
    public async Task<IActionResult> GetAbuseReportsAsync([FromQuery] string? status)
    {
        try
        {
            AbuseReportStatus? parsedStatus = null;

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (!Enum.TryParse<AbuseReportStatus>(status, true, out var enumStatus))
                    return BadRequest("Invalid status value");

                parsedStatus = enumStatus;
            }

            var query = new GetAbuseReportsQuery(parsedStatus);
            var reports = await queryService.Handle(query);

            return Ok(AbuseReportResourceFromEntityAssembler.ToResourceList(reports));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("abuse-reports/{reportId:guid}/status")]
    public async Task<IActionResult> UpdateAbuseReportStatusAsync(
        Guid reportId,
        [FromBody] UpdateAbuseReportStatusResource resource)
    {
        if (resource == null || string.IsNullOrWhiteSpace(resource.Status))
            return BadRequest("Status is required");

        if (!Enum.TryParse<AbuseReportStatus>(resource.Status, true, out var status))
            return BadRequest("Invalid status value");

        try
        {
            await commandService.Handle(new UpdateAbuseReportStatusCommand(reportId, status));
            return Ok(new { message = "Abuse report status updated successfully", reportId, status = status.ToString() });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("platform")]
    public async Task<IActionResult> GetPlatformAnalyticsAsync(CancellationToken cancellationToken)
    {
        var totalUsers = await iamContextFacade.CountUsersAsync(cancellationToken);

        var promotions = await promotionQueryService.GetAllAsync(new GetAllPromotionsQuery(), cancellationToken);
        var totalPromotions = promotions.Count;

        var reports = (await queryService.Handle(new GetAbuseReportsQuery(null))).ToList();

        var resource = new PlatformAnalyticsResource
        {
            TotalUsers = totalUsers,
            TotalPromotions = totalPromotions,
            TotalAbuseReports = reports.Count,
            PendingAbuseReports = reports.Count(r => r.Status == AbuseReportStatus.PENDING),
            ReviewedAbuseReports = reports.Count(r => r.Status == AbuseReportStatus.REVIEWED),
            ResolvedAbuseReports = reports.Count(r => r.Status == AbuseReportStatus.RESOLVED)
        };

        return Ok(resource);
    }
}
