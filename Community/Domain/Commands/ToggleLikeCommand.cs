namespace Klippr_Backend.Community.Domain.Commands;

public record ToggleLikeCommand(
    Guid ReviewId,
    Guid UserId
);
