using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Klippr_Backend.Promotions.Interface.Assemblers;
using Klippr_Backend.Promotions.Interface.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Promotions.Interface.Controllers;

/// <summary>
/// Controlador REST para gestionar promociones.
/// </summary>
/// <author>Samuel Bonifacio</author>
/// <remarks>
/// Expone el ciclo de vida principal de promociones sin mover reglas de negocio fuera del dominio.
/// </remarks>
[ApiController]
[Route("api/promotions")]
public class PromotionController(
    IPromotionCommandService promotionCommandService,
    IPromotionQueryService promotionQueryService) : ControllerBase
{
    /// <summary>
    /// Crea una nueva promocion en estado borrador.
    /// </summary>
    /// <param name="resource">Datos de entrada de la promocion.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con el identificador de la promocion creada.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePromotionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = CreatePromotionCommandFromResourceAssembler.ToCommand(resource);
            var promotionId = await promotionCommandService
                .CreateAsync(command, cancellationToken)
                .ConfigureAwait(false);

            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { promotionId },
                new { promotionId });
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
    /// Obtiene una promocion por su identificador.
    /// </summary>
    /// <param name="promotionId">Identificador unico de la promocion.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con la promocion encontrada.</returns>
    [HttpGet("{promotionId:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        Guid promotionId,
        CancellationToken cancellationToken)
    {
        var promotion = await promotionQueryService
            .GetByIdAsync(new GetPromotionByIdQuery(promotionId), cancellationToken)
            .ConfigureAwait(false);

        if (promotion is null)
            return NotFound();

        return Ok(PromotionResourceFromEntityAssembler.ToResource(promotion));
    }

    /// <summary>
    /// Obtiene promociones activas disponibles para consumidores.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con promociones activas.</returns>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveAsync(CancellationToken cancellationToken)
    {
        var promotions = await promotionQueryService
            .GetActiveAsync(new GetActivePromotionsQuery(), cancellationToken)
            .ConfigureAwait(false);

        return Ok(PromotionResourceFromEntityAssembler.ToResources(promotions));
    }

    /// <summary>
    /// Obtiene promociones asociadas a un negocio.
    /// </summary>
    /// <param name="businessId">Identificador del negocio propietario.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP con promociones del negocio.</returns>
    [HttpGet("businesses/{businessId:guid}")]
    public async Task<IActionResult> GetByBusinessIdAsync(
        Guid businessId,
        CancellationToken cancellationToken)
    {
        var promotions = await promotionQueryService
            .GetByBusinessIdAsync(new GetPromotionsByBusinessIdQuery(businessId), cancellationToken)
            .ConfigureAwait(false);

        return Ok(PromotionResourceFromEntityAssembler.ToResources(promotions));
    }

    /// <summary>
    /// Actualiza una promocion existente en estado borrador.
    /// </summary>
    /// <param name="promotionId">Identificador unico de la promocion.</param>
    /// <param name="resource">Datos actualizados de la promocion.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP sin contenido cuando la actualizacion finaliza.</returns>
    [HttpPut("{promotionId:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid promotionId,
        [FromBody] UpdatePromotionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = UpdatePromotionCommandFromResourceAssembler.ToCommand(promotionId, resource);

            await promotionCommandService
                .UpdateAsync(command, cancellationToken)
                .ConfigureAwait(false);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
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
    /// Publica una promocion en borrador.
    /// </summary>
    /// <param name="promotionId">Identificador unico de la promocion.</param>
    /// <param name="resource">Datos temporales de verificacion del negocio.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP sin contenido cuando la publicacion finaliza.</returns>
    [HttpPost("{promotionId:guid}/publish")]
    public async Task<IActionResult> PublishAsync(
        Guid promotionId,
        [FromBody] PublishPromotionResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = PublishPromotionCommandFromResourceAssembler.ToCommand(promotionId, resource);

            await promotionCommandService
                .PublishAsync(command, cancellationToken)
                .ConfigureAwait(false);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
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
    /// Cancela una promocion en borrador o publicada.
    /// </summary>
    /// <param name="promotionId">Identificador unico de la promocion.</param>
    /// <param name="cancellationToken">Token para cancelar la operacion asincronica.</param>
    /// <returns>Resultado HTTP sin contenido cuando la cancelacion finaliza.</returns>
    [HttpPost("{promotionId:guid}/cancel")]
    public async Task<IActionResult> CancelAsync(
        Guid promotionId,
        CancellationToken cancellationToken)
    {
        try
        {
            await promotionCommandService
                .CancelAsync(new CancelPromotionCommand(promotionId), cancellationToken)
                .ConfigureAwait(false);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
