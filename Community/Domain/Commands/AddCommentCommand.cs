namespace Klippr_Backend.Community.Domain.Commands;

public record AddCommentCommand(
    Guid ReviewId,
    Guid UserId,
    string Comment
);
