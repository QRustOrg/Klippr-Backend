using Klippr_Backend.Profile.Domain.Commands;
using Klippr_Backend.Profile.Domain.Services;
using Klippr_Backend.Profile.Interface.Assemblers;
using Klippr_Backend.Profile.Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Profile.Interface.Controllers;

[ApiController]
[Route("api/verification")]
[Authorize]
public class VerificationController : ControllerBase
{
    private readonly IBusinessProfileCommandService _commandService;
    private readonly ILogger<VerificationController> _logger;

    public VerificationController(
        IBusinessProfileCommandService commandService,
        ILogger<VerificationController> logger)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitVerification([FromBody] VerificationDocumentResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = VerificationDocumentCommandFromResourceAssembler.ToSubmitCommand(resource);
            var profile = await _commandService.SubmitVerificationAsync(command, cancellationToken);

            return Ok(new { message = "Verification document submitted successfully", profileId = profile.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error submitting verification: {ex.Message}");
            return StatusCode(500, new { message = "Error submitting verification" });
        }
    }

    [HttpPost("approve")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> ApproveVerification([FromBody] VerificationDocumentResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = VerificationDocumentCommandFromResourceAssembler.ToApproveCommand(resource.ProfileId);
            var profile = await _commandService.ApproveVerificationAsync(command, cancellationToken);

            return Ok(new { message = "Verification approved successfully", profileId = profile.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error approving verification: {ex.Message}");
            return StatusCode(500, new { message = "Error approving verification" });
        }
    }
}
