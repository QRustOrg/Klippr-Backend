namespace Klippr_Backend.Reviews.Domain.Commands;

public record ToggleLikeCommand(
    Guid ReviewId,
    Guid UserId
);
