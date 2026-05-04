using Domain.Aggregates;
using Interface.Resources;

namespace Interface.Assemblers;

public class BusinessProfileResourceFromEntityAssembler
{
    public static BusinessProfileResource ToResource(BusinessProfile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));

        var resource = new BusinessProfileResource
        {
            Id = profile.Id,
            UserId = profile.UserId,
            BusinessName = profile.BusinessName,
            TaxId = profile.TaxId,
            Description = profile.Description,
            VerificationStatus = profile.VerificationStatus.Value,
            DocumentUrl = profile.DocumentUrl,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,
            IsActive = profile.IsActive
        };

        if (profile.Category != null)
        {
            resource.Category = new BusinessCategoryResource
            {
                Name = profile.Category.Name,
                Description = profile.Category.Description
            };
        }

        if (profile.Location != null)
        {
            resource.Location = new LocationResource
            {
                Street = profile.Location.Street,
                City = profile.Location.City,
                State = profile.Location.State,
                Country = profile.Location.Country,
                PostalCode = profile.Location.PostalCode
            };
        }

        if (profile.Rating != null)
        {
            resource.Rating = new BusinessRatingResource
            {
                AverageRating = profile.Rating.AverageRating,
                TotalReviews = profile.Rating.TotalReviews
            };
        }

        return resource;
    }
}
