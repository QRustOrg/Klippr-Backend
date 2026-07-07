using Klippr_Backend.Promotions.Domain.Commands;
using Klippr_Backend.Promotions.Domain.Queries;
using Klippr_Backend.Promotions.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Klippr_Backend.Promotions.Interface.REST;

/// <summary>
/// Controlador REST para moderación administrativa de promociones.
/// </summary>
/// <remarks>
/// Reutiliza el ciclo de vida existente (cancelar / eliminar) restringido a administradores.
/// </remarks>
[ApiController]
[Route("api/admin/promotions")]
[Authorize(Roles = "ADMIN")]
public class AdminPromotionsController(
    IPromotionCommandService promotionCommandService,
    IPromotionQueryService promotionQueryService) : ControllerBase
{
    /// <summary>
    /// Da de baja (cancela) una promoción por moderación.
    /// </summary>
    [HttpPost("{promotionId:guid}/takedown")]
    [SwaggerOperation(
        Summary = "Takedown de promoción (admin)",
        Description = "Cancela una promoción en borrador o publicada por moderación. No elimina canjes ya generados.",
        OperationId = "AdminTakedownPromotion")]
    public async Task<IActionResult> TakedownAsync(
        Guid promotionId,
        CancellationToken cancellationToken)
    {
        try
        {
            var promotion = await promotionQueryService
                .GetByIdAsync(new GetPromotionByIdQuery(promotionId), cancellationToken)
                .ConfigureAwait(false);

            if (promotion is null)
                return NotFound();

            await promotionCommandService
                .CancelAsync(new CancelPromotionCommand(promotionId, promotion.BusinessId), cancellationToken)
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

    /// <summary>
    /// Elimina permanentemente una promoción por moderación.
    /// </summary>
    [HttpDelete("{promotionId:guid}")]
    [SwaggerOperation(
        Summary = "Eliminar promoción (admin)",
        Description = "Elimina permanentemente una promoción del sistema por moderación.",
        OperationId = "AdminDeletePromotion")]
    public async Task<IActionResult> DeleteAsync(
        Guid promotionId,
        CancellationToken cancellationToken)
    {
        try
        {
            var promotion = await promotionQueryService
                .GetByIdAsync(new GetPromotionByIdQuery(promotionId), cancellationToken)
                .ConfigureAwait(false);

            if (promotion is null)
                return NotFound();

            await promotionCommandService
                .DeleteAsync(new DeletePromotionCommand(promotionId, promotion.BusinessId), cancellationToken)
                .ConfigureAwait(false);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
