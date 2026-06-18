using Klippr_Backend.Community.Domain.Model.Aggregate;
using Klippr_Backend.Community.Domain.Model.Commands;
using Klippr_Backend.Community.Domain.Repositories;
using Klippr_Backend.Community.Domain.Services;
using Klippr_Backend.Shared.Domain.Repositories;

namespace Klippr_Backend.Community.Application.Internal.CommandServices;

public class ReviewCommandService(IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork) 
    : IReviewCommandService 
{
    public async Task<Review?> Handle(CreateReviewCommand command)
    {
        var review= new Review(command);
        try
        {
            await reviewRepository.AddAsync(review);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL CREAR REVIEW: {ex.Message}");
            Console.WriteLine(ex.InnerException?.Message);
            throw;
        }
        return review;
    }
    
    public async Task<bool> Handle(DeleteReviewByIdCommand command)
    {
        var review = await reviewRepository.FindByIdAsync(command.Id);
        if (review is null) return false;
        try
        {
            reviewRepository.Remove(review);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL ELIMINAR REVIEW: {ex.Message}");
            return false;
        }
    }
    
    public async Task<Review?> Handle(UpdateReviewCommand command)
    {
        var review = await reviewRepository.FindByIdAsync(command.Id);
        if (review is null) return null;
        try
        {
            review.UpdateReview(command);
            reviewRepository.Update(review);
            await unitOfWork.CompleteAsync();
            return review;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR AL ACTUALIZAR REVIEW: {ex.Message}");
            return null;
        }
    }
}