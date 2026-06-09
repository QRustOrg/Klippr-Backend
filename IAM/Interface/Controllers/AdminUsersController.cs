using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Interface.Assemblers;
using Klippr_Backend.IAM.Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.IAM.Interface.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "ADMIN")]
public class AdminUsersController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;

    public AdminUsersController(IUserCommandService userCommandService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
    }

    [HttpPost]
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
}
