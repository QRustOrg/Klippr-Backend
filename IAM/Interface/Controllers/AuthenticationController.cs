using Klippr_Backend.IAM.Application.OutboundServices.Hashing;
using Klippr_Backend.IAM.Application.OutboundServices.Tokens;
using Klippr_Backend.IAM.Domain.Services;
using Klippr_Backend.IAM.Interface.Assemblers;
using Klippr_Backend.IAM.Interface.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Klippr_Backend.IAM.Interface.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserCommandService _userCommandService;
    private readonly ITokenService _tokenService;
    private readonly IHashingService _hashingService;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IUserCommandService userCommandService,
        ITokenService tokenService,
        IHashingService hashingService,
        IWebHostEnvironment environment,
        ILogger<AuthenticationController> logger)
    {
        _userCommandService = userCommandService ?? throw new ArgumentNullException(nameof(userCommandService));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _hashingService = hashingService ?? throw new ArgumentNullException(nameof(hashingService));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        catch (ArgumentException)
        {
            // Generic response to avoid user enumeration / credential probing.
            return Unauthorized(new { message = "Invalid email or password." });
        }
        catch (InvalidOperationException)
        {
            // Generic response to avoid user enumeration / credential probing.
            return Unauthorized(new { message = "Invalid email or password." });
        }
        catch (Exception)
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
        catch (Exception)
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during sign up." });
        }
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = ForgotPasswordCommandFromResourceAssembler.Assemble(resource);
            var result = await _userCommandService.RequestPasswordResetAsync(command, cancellationToken);

            if (!result.UserExists)
                return NotFound(new { message = "Email not found." });

            _logger.LogInformation("Password reset code generated for {Email}: {Code}", resource.Email, result.Code);

            if (_environment.IsDevelopment())
                return Ok(new { message = "Reset code generated.", code = result.Code });

            return Ok(new { message = "If the email exists, a reset code has been sent." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while verifying the email." });
        }
    }

    [HttpPut("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordResource resource, CancellationToken cancellationToken)
    {
        if (resource == null)
            return BadRequest(new { message = "Request body is required." });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var command = ResetPasswordCommandFromResourceAssembler.Assemble(resource);
            await _userCommandService.ResetPasswordAsync(command, cancellationToken);

            return Ok(new { message = "Password updated successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException)
        {
            // Generic response to avoid user enumeration / code-guessing feedback.
            return BadRequest(new { message = "Invalid or expired reset code." });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while resetting the password." });
        }
    }
}
