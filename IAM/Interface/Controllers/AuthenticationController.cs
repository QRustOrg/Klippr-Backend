using Application.OutboundServices.Hashing;
using Application.OutboundServices.Tokens;
using Domain.Services;
using Interface.Assemblers;
using Interface.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly ITokenService _tokenService;
    private readonly IHashingService _hashingService;

    public AuthenticationController(
        IUserCommandService userCommandService,
        ITokenService tokenService,
        IHashingService hashingService)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = SignInCommandFromResourceAssembler.Assemble(resource);
            var user = await _userCommandService.SignInAsync(command, cancellationToken);

            var token = _tokenService.GenerateToken(user.Id, user.Email.Value, user.Role.Value);
            var response = AuthenticatedUserResourceFromEntityAssembler.Assemble(user, token);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during sign in." });
        }
    }

    [HttpPost("sign-up/consumer")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpConsumer([FromBody] SignUpConsumerResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = SignUpConsumerCommandFromResourceAssembler.Assemble(resource);
            var user = await _userCommandService.SignUpConsumerAsync(command, cancellationToken);

            var token = _tokenService.GenerateToken(user.Id, user.Email.Value, user.Role.Value);
            var response = AuthenticatedUserResourceFromEntityAssembler.Assemble(user, token);

            return CreatedAtAction(nameof(SignUpConsumer), response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during sign up." });
        }
    }

    [HttpPost("sign-up/business")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUpBusiness([FromBody] SignUpBusinessResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = SignUpBusinessCommandFromResourceAssembler.Assemble(resource);
            var user = await _userCommandService.SignUpBusinessAsync(command, cancellationToken);

            var token = _tokenService.GenerateToken(user.Id, user.Email.Value, user.Role.Value);
            var response = AuthenticatedUserResourceFromEntityAssembler.Assemble(user, token);

            return CreatedAtAction(nameof(SignUpBusiness), response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during sign up." });
        }
    }
}
