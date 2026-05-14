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
    private readonly ILogger<AdminProfileController> _logger;

    public AdminProfileController(
        IProfileQueryService queryService,
        ILogger<AdminProfileController> logger)
    {
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
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
}
