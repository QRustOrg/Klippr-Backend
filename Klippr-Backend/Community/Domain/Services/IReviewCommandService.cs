using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Domain.Model.Commands;
using Klippr_Backend.Community.Domain.Model.Queries;

namespace Klippr_Backend.Community.Domain.Services;

public interface IReviewCommandService
{
    Task<Review?> Handle(CreateReviewCommand command);
    Task<bool> Handle(DeleteReviewByIdCommand command);
    Task<Review?> Handle(UpdateReviewCommand command); 
}