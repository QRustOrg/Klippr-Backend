using Klippr_Backend.Redemption.Domain.Queries;
using Klippr_Backend.Redemption.Domain.Services;
using Klippr_Backend.Redemption.Interface.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Redemption.Interface.REST;

/// <summary>
/// Controlador REST para gestionar canjes de promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Expone el ciclo de vida de canjes sin mover reglas de negocio fuera del dominio.
/// </remarks>
[ApiController]
[Route("api/redemptions")]
public class RedemptionController(
    IRedemptionCommandService redemptionCommandService,
    IRedemptionQueryService redemptionQueryService) : ControllerBase
{
    private const string GetRedemptionByIdRouteName = "GetRedemptionById";

    /// <summary>
    /// Genera un nuevo canje para una promocion.
    /// </summary>
    /// <param name="resource">Datos de entrada del canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el canje generado.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Generar canje",
        Description = "Crea un nuevo canje para una promocion activa. Genera un codigo unico y un token antifraude asociados al consumidor y negocio indicados. El canje queda en estado generado hasta ser confirmado.",
        OperationId = "RedeemPromotion")]
    public async Task<IActionResult> RedeemAsync(
        [FromBody] RedeemPromotionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RedeemPromotionCommandFromResourceAssembler.ToCommand(resource);
            var redemption = await redemptionCommandService
                .Handle(command)
                .ConfigureAwait(false);

            if (redemption is null)
                return BadRequest("No se pudo generar el canje.");

            return CreatedAtRoute(
                GetRedemptionByIdRouteName,
                new { redemptionId = redemption.Id },
                RedemptionResourceFromEntityAssembler.ToResource(redemption));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    /// <summary>
    /// Confirma el uso de un canje existente.
    /// </summary>
    /// <param name="redemptionId">Identificador del canje que se desea confirmar.</param>
    /// <param name="resource">Datos de confirmacion del canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el canje confirmado.</returns>
    [HttpPost("{redemptionId:int}/confirm")]
    [SwaggerOperation(
        Summary = "Confirmar canje",
        Description = "Registra el uso efectivo de un canje por parte del negocio, cambiando su estado a canjeado. Valida que el canje pertenezca al negocio indicado y que no haya expirado.",
        OperationId = "ConfirmRedemption")]
    public async Task<IActionResult> ConfirmAsync(
        int redemptionId,
        [FromBody] ConfirmRedemptionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = ConfirmRedemptionCommandFromResourceAssembler.ToCommand(redemptionId, resource);
            var redemption = await redemptionCommandService
                .Handle(command)
                .ConfigureAwait(false);

            if (redemption is null)
                return NotFound();

            return Ok(RedemptionResourceFromEntityAssembler.ToResource(redemption));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    /// <summary>
    /// Obtiene un canje por su identificador.
    /// </summary>
    /// <param name="redemptionId">Identificador unico del canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el canje encontrado.</returns>
    [HttpGet("{redemptionId:int}", Name = GetRedemptionByIdRouteName)]
    [SwaggerOperation(
        Summary = "Obtener canje por ID",
        Description = "Retorna los detalles completos de un canje dado su identificador interno, incluyendo su estado actual, codigo, token antifraude y fechas relevantes. Retorna 404 si no existe.",
        OperationId = "GetRedemptionById")]
    public async Task<IActionResult> GetByIdAsync(
        int redemptionId,
        CancellationToken cancellationToken)
    {
        var redemption = await redemptionQueryService
            .Handle(new GetRedemptionByIdQuery(redemptionId))
            .ConfigureAwait(false);

        if (redemption is null)
            return NotFound();

        return Ok(RedemptionResourceFromEntityAssembler.ToResource(redemption));
    }

    /// <summary>
    /// Obtiene los canjes asociados a un consumidor.
    /// </summary>
    /// <param name="consumerId">Identificador del consumidor.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con los canjes del consumidor.</returns>
    [HttpGet("consumers/{consumerId:guid}")]
    [SwaggerOperation(
        Summary = "Listar canjes por consumidor",
        Description = "Retorna todos los canjes generados por un consumidor, ordenados del mas reciente al mas antiguo. Incluye canjes en cualquier estado: generado, canjeado, bloqueado o expirado.",
        OperationId = "GetRedemptionsByConsumerId")]
    public async Task<IActionResult> GetByConsumerIdAsync(
        Guid consumerId,
        CancellationToken cancellationToken)
    {
        var redemptions = await redemptionQueryService
            .Handle(new GetRedemptionsByConsumerIdQuery(consumerId))
            .ConfigureAwait(false);

        return Ok(RedemptionResourceFromEntityAssembler.ToResources(redemptions));
    }

    /// <summary>
    /// Obtiene los canjes asociados a un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con los canjes del negocio.</returns>
    [HttpGet("businesses/{businessId:guid}")]
    [SwaggerOperation(
        Summary = "Listar canjes por negocio",
        Description = "Retorna todos los canjes asociados a un negocio, ordenados del mas reciente al mas antiguo. Util para el historial y reportes internos del negocio.",
        OperationId = "GetRedemptionsByBusinessId")]
    public async Task<IActionResult> GetByBusinessIdAsync(
        Guid businessId,
        CancellationToken cancellationToken)
    {
        var redemptions = await redemptionQueryService
            .Handle(new GetRedemptionsByBusinessIdQuery(businessId))
            .ConfigureAwait(false);

        return Ok(RedemptionResourceFromEntityAssembler.ToResources(redemptions));
    }
}
