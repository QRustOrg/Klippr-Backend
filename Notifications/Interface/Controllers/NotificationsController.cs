using System.Net.Mime;
using System.Security.Claims;
using Klippr_Backend.Notifications.Domain.Commands;
using Klippr_Backend.Notifications.Domain.Queries;
using Klippr_Backend.Notifications.Domain.Services;
using Klippr_Backend.Notifications.Interface.Assemblers;
using Klippr_Backend.Notifications.Interface.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klippr_Backend.Notifications.Interface.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
public class NotificationsController(
    INotificationCommandService commandService,
    INotificationQueryService   queryService) : ControllerBase
{
    private string GetUserId() =>
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetNotificationsByUser(
        string userId,
        [FromQuery] bool unreadOnly = false)
    {
        if (string.IsNullOrWhiteSpace(userId)) return BadRequest();
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();

        var items = (await queryService.Handle(new GetUserNotificationsQuery(userId, unreadOnly)))
            .Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        var unreadCount = await queryService.HandleUnreadCount(userId);

        return Ok(new NotificationListResource(userId, items.Count, unreadCount, items.AsReadOnly()));
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationResource resource)
    {
        var command      = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var notification = await commandService.Handle(command);
        if (notification is null) return BadRequest(new { message = "Could not create notification." });

        var result = NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
        return Created($"api/v1/Notifications/{notification.NotificationId}", result);
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(string notificationId)
    {
        var userId = GetUserId();
        var ok     = await commandService.Handle(new MarkNotificationAsReadCommand(userId, notificationId));
        return ok ? NoContent() : NotFound();
    }

    [HttpPatch("user/{userId}/read-all")]
    public async Task<IActionResult> MarkAllAsRead(string userId)
    {
        if (!string.Equals(userId, GetUserId(), StringComparison.Ordinal)) return Forbid();
        var ok = await commandService.Handle(new MarkAllNotificationsAsReadCommand(userId));
        return ok ? NoContent() : BadRequest();
    }

    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(string notificationId)
    {
        var userId = GetUserId();
        var ok     = await commandService.Handle(new DeleteNotificationCommand(userId, notificationId));
        return ok ? NoContent() : NotFound();
    }
    
}