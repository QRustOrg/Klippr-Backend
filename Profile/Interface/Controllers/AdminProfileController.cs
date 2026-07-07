using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Interface.Assemblers;
using Klippr_Backend.IAM.Interface.Resources;
using Klippr_Backend.Profile.Application.QueryServices;
using Klippr_Backend.Profile.Domain.Queries;
using Klippr_Backend.Profile.Domain.Services;
using Klippr_Backend.Profile.Interface.Assemblers;
using Klippr_Backend.Profile.Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Klippr_Backend.Profile.Interface.Controllers;

[ApiController]
[Route("api/admin/profiles")]
[Authorize(Roles = "ADMIN")]
public class AdminProfileController : ControllerBase
{
    private readonly IProfileQueryService _queryService;
    private readonly IBusinessProfileCommandService _businessProfileCommandService;
    private readonly IUserCommandService _userCommandService;
    private readonly ILogger<AdminProfileController> _logger;

    public AdminProfileController(
        IProfileQueryService queryService,
        IBusinessProfileCommandService businessProfileCommandService,
        IUserCommandService userCommandService,
        ILogger<AdminProfileController> logger)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        _businessProfileCommandService = businessProfileCommandService ?? throw new ArgumentNullException(nameof(businessProfileCommandService));
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("pending-verification")]
    public async Task<IActionResult> GetPendingVerification([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and page size must be greater than 0.");

            var query = new GetProfilesWithVerificationPendingQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var profiles = await _queryService.GetProfilesWithVerificationPendingAsync(query, cancellationToken);
            var resources = profiles.Select(BusinessProfileResourceFromEntityAssembler.ToResource).ToList();

            return Ok(new { total = resources.Count, pageNumber, pageSize, data = resources });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving pending verifications: {ex.Message}");
            return StatusCode(500, new { message = "Error retrieving pending verifications" });
        }
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetProfileByUser(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var consumerQuery = new GetConsumerProfileByUserIdQuery { UserId = userId };
            var consumerProfile = await _queryService.GetConsumerProfileByUserIdAsync(consumerQuery, cancellationToken);

            if (consumerProfile != null)
            {
                var resource = ConsumerProfileResourceFromEntityAssembler.ToResource(consumerProfile);
                return Ok(new { type = "consumer", data = resource });
            }

            var businessQuery = new GetBusinessProfileByUserIdQuery { UserId = userId };
            var businessProfile = await _queryService.GetBusinessProfileByUserIdAsync(businessQuery, cancellationToken);

            if (businessProfile != null)
            {
                var resource = BusinessProfileResourceFromEntityAssembler.ToResource(businessProfile);
                return Ok(new { type = "business", data = resource });
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user profiles: {ex.Message}");
            return StatusCode(500, new { message = "Error retrieving profiles" });
        }
    }

    // --- User administration (IAM context, exposed here per consolidation decision) ---

    [HttpPost("/api/admin/users")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAdmin([FromBody] SignUpAdminResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = SignUpAdminCommandFromResourceAssembler.Assemble(resource);
            var user = await _userCommandService.SignUpAdminAsync(command, cancellationToken);

            var response = UserResourceFromEntityAssembler.Assemble(user);

            return CreatedAtAction(nameof(CreateAdmin), response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the admin user." });
        }
    }

    [HttpPost("/api/admin/users/{userId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userCommandService.DeactivateUserAsync(userId, cancellationToken);
            return Ok(new { message = "User deactivated successfully", userId = user.Id, isActive = user.IsActive });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deactivating user: {ex.Message}");
            return StatusCode(500, new { message = "Error deactivating user" });
        }
    }

    [HttpPost("/api/admin/users/{userId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateUser(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userCommandService.ActivateUserAsync(userId, cancellationToken);
            return Ok(new { message = "User reactivated successfully", userId = user.Id, isActive = user.IsActive });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error reactivating user: {ex.Message}");
            return StatusCode(500, new { message = "Error reactivating user" });
        }
    }

    // --- Business profile administration ---

    [HttpPost("{profileId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateProfile(Guid profileId, CancellationToken cancellationToken)
    {
        try
        {
            await _businessProfileCommandService.DeactivateProfileAsync(profileId, cancellationToken);
            return Ok(new { message = "Business profile deactivated successfully", profileId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deactivating profile: {ex.Message}");
            return StatusCode(500, new { message = "Error deactivating profile" });
        }
    }

    [HttpPost("{profileId:guid}/reactivate")]
    public async Task<IActionResult> ReactivateProfile(Guid profileId, CancellationToken cancellationToken)
    {
        try
        {
            await _businessProfileCommandService.ActivateProfileAsync(profileId, cancellationToken);
            return Ok(new { message = "Business profile reactivated successfully", profileId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error reactivating profile: {ex.Message}");
            return StatusCode(500, new { message = "Error reactivating profile" });
        }
    }
}
