using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore.CreatedUpdatedDate.Contracts;
using Klippr_Backend.Notifications.Domain.Commands;
using Klippr_Backend.Notifications.Domain.ValueObjects;

namespace Klippr_Backend.Notifications.Domain.Aggregates;

public class NotificationItem : IEntityWithCreatedUpdatedDate
{
    private NotificationItem()
    {
        NotificationId = string.Empty;
        UserId         = string.Empty;
        Title          = string.Empty;
        Message        = string.Empty;
    }

    public NotificationItem(CreateNotificationCommand command)
    {
        NotificationId = Guid.NewGuid().ToString();
        UserId         = command.UserId;
        Type           = command.Type;
        Title          = command.Title;
        Message        = command.Message;
        RelatedId      = command.RelatedId;
        IsRead         = false;
    }

    public bool BelongsToUser(string userId) =>
        string.Equals(UserId, userId, StringComparison.Ordinal);

    public void MarkAsRead() => IsRead = true;

    public int              Id             { get; private set; }
    public string           NotificationId { get; private set; }
    public string           UserId         { get; private set; }
    public NotificationType Type           { get; private set; }
    public string           Title          { get; private set; }
    public string           Message        { get; private set; }
    public string?          RelatedId      { get; private set; }
    public bool             IsRead         { get; private set; }

    [Column("CreatedAt")] public DateTimeOffset? CreatedDate { get; set; }
    [Column("UpdatedAt")] public DateTimeOffset? UpdatedDate { get; set; }
}