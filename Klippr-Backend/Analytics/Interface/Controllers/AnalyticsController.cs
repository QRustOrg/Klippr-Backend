using Klippr_Backend.Analytics.Domain.Commands;
using Klippr_Backend.Analytics.Domain.Queries;
using Klippr_Backend.Analytics.Domain.Services;
using Klippr_Backend.Analytics.Interface.Assemblers;
using Klippr_Backend.Analytics.Interface.Resources;
using Klippr_Backend.Analytics.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Analytics.Interface.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
/// <summary>
/// Controlador REST para gestión de analítica del sistema.
/// </summary>
/// <author>Alejandro Galindo</author>
/// <remarks>
/// Expone métricas, dashboards y reportes de abuso.
/// La lógica de negocio se mantiene en la capa de dominio mediante servicios.
/// </remarks>
[ApiController]
[Route("api/analytics")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Analytics")]
public class AnalyticsController(
    IAnalyticsCommandService commandService,
    IAnalyticsQueryService queryService) : ControllerBase
{
    /// <summary>
    /// Actualiza métricas del sistema.
    /// </summary>
    /// <remarks>
    /// Recibe datos agregados de métricas y los persiste en el sistema.
    /// </remarks>
    [HttpPost("metrics")]
    [SwaggerOperation(
        Summary = "Actualiza métricas del sistema",
        Description = "Recibe y persiste métricas agregadas del sistema",
        OperationId = "UpdateMetrics")]
    [SwaggerResponse(204, "Métricas actualizadas correctamente")]
    [SwaggerResponse(400, "Datos inválidos")]
    public async Task<IActionResult> UpdateMetricsAsync(
        [FromBody] UpdateMetricsResource resource)
    {
        try
        {
            var command = UpdateMetricsCommandFromResourceAssembler.ToCommand(resource);
            await commandService.Handle(command);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene el dashboard de un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio.</param>
    [HttpGet("dashboard/{businessId:guid}")]
    [SwaggerOperation(
        Summary = "Obtiene dashboard de negocio",
        Description = "Devuelve métricas agregadas de un negocio específico",
        OperationId = "GetBusinessDashboard")]
    [SwaggerResponse(200, "Dashboard obtenido correctamente")]
    [SwaggerResponse(404, "Negocio no encontrado")]
    public async Task<IActionResult> GetDashboardAsync(Guid businessId)
    {
        var query = new GetBusinessDashboardQuery(businessId);
        var metrics = await queryService.Handle(query);

        if (metrics is null)
            return NotFound();

        return Ok(
            BusinessDashboardResourceFromEntityAssembler
                .ToResource(businessId, metrics)
        );
    }
    /// <summary>
    /// Obtiene métricas de una campaña específica.
    /// </summary>
    /// <param name="campaignId">Identificador de la campaña.</param>
    [HttpGet("campaign/{campaignId:guid}")]
    [SwaggerOperation(
        Summary = "Obtiene métricas de campaña",
        Description = "Devuelve métricas específicas de una campaña",
        OperationId = "GetCampaignMetrics")]
    [SwaggerResponse(200, "Métricas obtenidas correctamente")]
    [SwaggerResponse(404, "Campaña no encontrada")]
    public async Task<IActionResult> GetCampaignMetricsAsync(Guid campaignId)
    {
        var query = new GetCampaignMetricsQuery(campaignId);
        var result = await queryService.Handle(query);

        if (result is null)
            return NotFound();

        return Ok(
            CampaignMetricsResourceFromEntityAssembler
                .ToResourceFromEntity(result)
        );
    }

    /// <summary>
    /// Registra un reporte de abuso.
    /// </summary>
    /// <remarks>
    /// Permite registrar contenido o comportamiento abusivo en la plataforma.
    /// </remarks>
    [HttpPost("abuse-reports")]
    [SwaggerOperation(
        Summary = "Registra un reporte de abuso",
        Description = "Crea un nuevo reporte de contenido abusivo",
        OperationId = "RegisterAbuseReport")]
    [SwaggerResponse(204, "Reporte registrado correctamente")]
    [SwaggerResponse(400, "Datos inválidos")]
    public async Task<IActionResult> RegisterAbuseReportAsync(
        [FromBody] RegisterAbuseReportResource resource)
    {
        try
        {
            var command = RegisterAbuseReportCommandFromResourceAssembler.ToCommand(resource);
            await commandService.Handle(command);

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Obtiene reportes de abuso con filtro opcional.
    /// </summary>
    /// <param name="status">Estado del reporte (opcional).</param>
    [HttpGet("abuse-reports")]
    [SwaggerOperation(
        Summary = "Obtiene reportes de abuso",
        Description = "Devuelve reportes filtrados opcionalmente por estado",
        OperationId = "GetAbuseReports")]
    [SwaggerResponse(200, "Reportes obtenidos correctamente")]
    [SwaggerResponse(400, "Parámetro inválido")]
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

            return Ok(
                AbuseReportResourceFromEntityAssembler
                    .ToResourceList(reports)
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}