using Klippr_Backend.IAM.Domain.Queries;
using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Interface.Assemblers;
using Klippr_Backend.IAM.Interface.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.IAM.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserQueryService _userQueryService;

    public UsersController(IUserQueryService userQueryService)
    {
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        if (!HasAuthorization(out var userIdClaim))
            return Unauthorized(new { message = "Unauthorized: Token not found or invalid." });

        if (userId == Guid.Empty)
            return BadRequest(new { message = "User ID cannot be empty." });

        try
        {
            var query = new GetUserByIdQuery { UserId = userId };
            var user = await _userQueryService.GetUserByIdAsync(query, cancellationToken);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var response = UserResourceFromEntityAssembler.Assemble(user);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred." });
        }
    }

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        if (!HasAuthorization(out var userIdClaim))
            return Unauthorized(new { message = "Unauthorized: Token not found or invalid." });

        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { message = "Email is required." });

        try
        {
            var query = new GetUserByEmailQuery { Email = email };
            var user = await _userQueryService.GetUserByEmailAsync(query, cancellationToken);

            if (user == null)
                return NotFound(new { message = "User not found." });

            var response = UserResourceFromEntityAssembler.Assemble(user);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred." });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!HasAuthorization(out var userIdClaim))
            return Unauthorized(new { message = "Unauthorized: Token not found or invalid." });

        if (pageNumber < 1 || pageSize < 1)
            return BadRequest(new { message = "Page number and page size must be greater than 0." });

        try
        {
            var query = new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize };
            var users = await _userQueryService.GetAllUsersAsync(query, cancellationToken);

            var response = UserResourceFromEntityAssembler.AssembleMany(users);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred." });
        }
    }

    [HttpGet("role/{role}")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUsersByRole(
        [FromRoute] string role,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!HasAuthorization(out var userIdClaim))
            return Unauthorized(new { message = "Unauthorized: Token not found or invalid." });

        if (string.IsNullOrWhiteSpace(role))
            return BadRequest(new { message = "Role is required." });

        if (pageNumber < 1 || pageSize < 1)
            return BadRequest(new { message = "Page number and page size must be greater than 0." });

        try
        {
            var query = new GetUsersByRoleQuery { Role = role, PageNumber = pageNumber, PageSize = pageSize };
            var users = await _userQueryService.GetUsersByRoleAsync(query, cancellationToken);

            var response = UserResourceFromEntityAssembler.AssembleMany(users);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred." });
        }
    }

    private bool HasAuthorization(out Guid userId)
    {
        userId = Guid.Empty;

        if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is Guid id)
        {
            userId = id;
            return true;
        }

        return false;
    }
}
