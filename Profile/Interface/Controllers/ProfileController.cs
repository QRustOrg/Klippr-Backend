using Application.QueryServices;
using Application.Services;
using Domain.Commands;
using Domain.Queries;
using Domain.Services;
using Interface.Assemblers;
using Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Interface.Controllers;

[ApiController]
[Route("api/profiles")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IConsumerProfileCommandService _consumerCommandService;
    private readonly IBusinessProfileCommandService _businessCommandService;
    private readonly IProfileQueryService _queryService;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(
        IConsumerProfileCommandService consumerCommandService,
        IBusinessProfileCommandService businessCommandService,
        IProfileQueryService queryService,
        ILogger<ProfileController> logger)
    {
        _consumerCommandService = consumerCommandService ?? throw new ArgumentNullException(nameof(consumerCommandService));
        _businessCommandService = businessCommandService ?? throw new ArgumentNullException(nameof(businessCommandService));
        _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("consumer")]
    public async Task<IActionResult> CreateConsumerProfile([FromBody] ConsumerProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token."));
            var command = CreateConsumerProfileCommandFromResourceAssembler.ToCommand(resource, userId);

            var profile = await _consumerCommandService.CreateProfileAsync(command, cancellationToken);
            var responseResource = ConsumerProfileResourceFromEntityAssembler.ToResource(profile);

            return CreatedAtAction(nameof(GetConsumerProfile), new { profileId = profile.Id }, responseResource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating consumer profile: {ex.Message}");
            return StatusCode(500, new { message = "Error creating profile" });
        }
    }

    [HttpGet("consumer/{profileId:guid}")]
    public async Task<IActionResult> GetConsumerProfile(Guid profileId, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetConsumerProfileByUserIdQuery { UserId = profileId };
            var profile = await _queryService.GetConsumerProfileByUserIdAsync(query, cancellationToken);

            if (profile == null)
                return NotFound();

            var resource = ConsumerProfileResourceFromEntityAssembler.ToResource(profile);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving consumer profile: {ex.Message}");
            return StatusCode(500, new { message = "Error retrieving profile" });
        }
    }

    [HttpPut("consumer")]
    public async Task<IActionResult> UpdateConsumerProfile([FromBody] UpdateConsumerProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateConsumerProfileCommand
            {
                ProfileId = resource.ProfileId,
                FirstName = resource.FirstName,
                LastName = resource.LastName,
                PhoneNumber = resource.PhoneNumber,
                Street = resource.Street,
                City = resource.City,
                State = resource.State,
                Country = resource.Country,
                ZipCode = resource.ZipCode
            };

            var profile = await _consumerCommandService.UpdateProfileAsync(command, cancellationToken);
            var responseResource = ConsumerProfileResourceFromEntityAssembler.ToResource(profile);

            return Ok(responseResource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating consumer profile: {ex.Message}");
            return StatusCode(500, new { message = "Error updating profile" });
        }
    }

    [HttpPost("business")]
    public async Task<IActionResult> CreateBusinessProfile([FromBody] BusinessProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("User ID not found in token."));

            var command = new CreateBusinessProfileCommand
            {
                UserId = userId,
                BusinessName = resource.BusinessName,
                TaxId = resource.TaxId,
                Category = resource.Category?.Name ?? string.Empty
            };

            var profile = await _businessCommandService.CreateProfileAsync(command, cancellationToken);
            var responseResource = BusinessProfileResourceFromEntityAssembler.ToResource(profile);

            return CreatedAtAction(nameof(GetBusinessProfile), new { profileId = profile.Id }, responseResource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating business profile: {ex.Message}");
            return StatusCode(500, new { message = "Error creating profile" });
        }
    }

    [HttpGet("business/{profileId:guid}")]
    public async Task<IActionResult> GetBusinessProfile(Guid profileId, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetBusinessProfileByUserIdQuery { UserId = profileId };
            var profile = await _queryService.GetBusinessProfileByUserIdAsync(query, cancellationToken);

            if (profile == null)
                return NotFound();

            var resource = BusinessProfileResourceFromEntityAssembler.ToResource(profile);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving business profile: {ex.Message}");
            return StatusCode(500, new { message = "Error retrieving profile" });
        }
    }

    [HttpPut("business")]
    public async Task<IActionResult> UpdateBusinessProfile([FromBody] UpdateBusinessProfileResource resource, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateBusinessProfileCommand
            {
                ProfileId = resource.ProfileId,
                BusinessName = resource.BusinessName,
                Category = resource.Category,
                Description = resource.Description,
                Street = resource.Street,
                City = resource.City,
                State = resource.State,
                Country = resource.Country,
                ZipCode = resource.ZipCode
            };

            var profile = await _businessCommandService.UpdateProfileAsync(command, cancellationToken);
            var responseResource = BusinessProfileResourceFromEntityAssembler.ToResource(profile);

            return Ok(responseResource);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating business profile: {ex.Message}");
            return StatusCode(500, new { message = "Error updating profile" });
        }
    }
}
