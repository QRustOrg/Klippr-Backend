namespace Klippr_Backend.Reviews.Domain.Commands;

public record AddCommentCommand(
    Guid ReviewId,
    Guid UserId,
    string Comment
);
