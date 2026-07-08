using System.Security.Claims;
using Klippr_Backend.Profile.Interface.Facade;
using Klippr_Backend.Redemption.Application.OutboundServices.Signing;
using Klippr_Backend.Redemption.Domain.Queries;
using Klippr_Backend.Redemption.Domain.Exceptions;
using Klippr_Backend.Redemption.Domain.Services;
using Klippr_Backend.Redemption.Interface.Transform;
using Microsoft.AspNetCore.Authorization;
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
[Authorize]
public class RedemptionController(
    IRedemptionCommandService redemptionCommandService,
    IRedemptionQueryService redemptionQueryService,
    ProfileContextFacade profileContextFacade,
    IRedemptionTokenSigner redemptionTokenSigner) : ControllerBase
{
    private const string GetRedemptionByIdRouteName = "GetRedemptionById";

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token."));

    /// <summary>
    /// Genera un nuevo canje para una promocion.
    /// </summary>
    /// <param name="resource">Datos de entrada del canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el canje generado.</returns>
    [HttpPost]
    [Authorize(Roles = "CONSUMER")]
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
            var command = RedeemPromotionCommandFromResourceAssembler.ToCommand(resource) with { ConsumerId = GetUserId() };
            var redemption = await redemptionCommandService
                .Handle(command)
                .ConfigureAwait(false);

            if (redemption is null)
                return BadRequest("No se pudo generar el canje.");

            var resourceResult = RedemptionResourceFromEntityAssembler.ToResource(redemption.Redemption, redemptionTokenSigner);

            return redemption.Created
                ? CreatedAtRoute(
                    GetRedemptionByIdRouteName,
                    new { redemptionId = redemption.Redemption.Id },
                    resourceResult)
                : Ok(resourceResult);
        }
        catch (RedemptionConflictException exception)
        {
            return Conflict(new { message = exception.Message });
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
    /// Confirma el uso de un canje existente desde el token opaco del QR.
    /// </summary>
    /// <param name="uniqueToken">Token unico contenido en el QR.</param>
    /// <param name="resource">Datos de confirmacion del canje.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el canje confirmado.</returns>
    [HttpPost("tokens/{uniqueToken:guid}/confirm")]
    [Authorize(Roles = "BUSINESS")]
    [SwaggerOperation(
        Summary = "Confirmar canje por token",
        Description = "Registra el uso efectivo de un canje a partir del token opaco contenido en el QR. Valida negocio, vigencia y estado antes de bloquear el canje.",
        OperationId = "ConfirmRedemptionByToken")]
    public async Task<IActionResult> ConfirmByTokenAsync(
        Guid uniqueToken,
        [FromBody] ConfirmRedemptionResource resource,
        CancellationToken cancellationToken)
    {
        var businessProfile = await profileContextFacade
            .GetBusinessProfileByUserIdAsync(GetUserId(), cancellationToken)
            .ConfigureAwait(false);

        if (businessProfile is null)
            return Forbid();

        if (!redemptionTokenSigner.Verify(uniqueToken, resource.Signature ?? string.Empty))
            return BadRequest(new { message = "Firma de token inválida." });

        try
        {
            var command = ConfirmRedemptionByTokenCommandFromResourceAssembler.ToCommand(uniqueToken, resource) with { BusinessId = businessProfile.Id };
            var redemption = await redemptionCommandService
                .Handle(command, cancellationToken)
                .ConfigureAwait(false);

            if (redemption is null)
                return NotFound();

            return Ok(RedemptionResourceFromEntityAssembler.ToResource(redemption, redemptionTokenSigner));
        }
        catch (RedemptionConflictException exception)
        {
            return Conflict(new { message = exception.Message });
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
    [Authorize(Roles = "BUSINESS,CONSUMER")]
    [SwaggerOperation(
        Summary = "Confirmar canje",
        Description = "Registra el uso efectivo de un canje. Para negocios, valida que el canje pertenezca al negocio indicado. Para consumidores, valida que el canje les pertenezca. Valida vigencia y estado antes de confirmar.",
        OperationId = "ConfirmRedemption")]
    public async Task<IActionResult> ConfirmAsync(
        int redemptionId,
        [FromBody] ConfirmRedemptionResource resource,
        CancellationToken cancellationToken)
    {
        var isConsumer = User.IsInRole("CONSUMER");
        
        if (isConsumer)
        {
            var consumerId = GetUserId();
            var redemption = await redemptionQueryService
                .Handle(new GetRedemptionByIdQuery(redemptionId))
                .ConfigureAwait(false);

            if (redemption is null)
                return NotFound();

            if (redemption.ConsumerId != consumerId)
                return Forbid();

            var command = new Klippr_Backend.Redemption.Domain.Commands.ConfirmRedemptionCommand(
                redemptionId,
                redemption.BusinessId,
                Klippr_Backend.Redemption.Domain.ValueObjects.RedemptionValidationMethod.ManualCode,
                resource.ConfirmedAt
            );
            
            try
            {
                var confirmedRedemption = await redemptionCommandService
                    .Handle(command, cancellationToken)
                    .ConfigureAwait(false);

                if (confirmedRedemption is null)
                    return NotFound();

                return Ok(RedemptionResourceFromEntityAssembler.ToResource(confirmedRedemption, redemptionTokenSigner));
            }
            catch (RedemptionConflictException exception)
            {
                return Conflict(new { message = exception.Message });
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
        else
        {
            var businessProfile = await profileContextFacade
                .GetBusinessProfileByUserIdAsync(GetUserId(), cancellationToken)
                .ConfigureAwait(false);

            if (businessProfile is null)
                return Forbid();

            try
            {
                var command = ConfirmRedemptionCommandFromResourceAssembler.ToCommand(redemptionId, resource) with { BusinessId = businessProfile.Id };
                var redemption = await redemptionCommandService
                    .Handle(command, cancellationToken)
                    .ConfigureAwait(false);

                if (redemption is null)
                    return NotFound();

                return Ok(RedemptionResourceFromEntityAssembler.ToResource(redemption, redemptionTokenSigner));
            }
            catch (RedemptionConflictException exception)
            {
                return Conflict(new { message = exception.Message });
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

        return Ok(RedemptionResourceFromEntityAssembler.ToResource(redemption, redemptionTokenSigner));
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

        return Ok(RedemptionResourceFromEntityAssembler.ToResources(redemptions, redemptionTokenSigner));
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

        return Ok(RedemptionResourceFromEntityAssembler.ToResources(redemptions, redemptionTokenSigner));
    }
}
