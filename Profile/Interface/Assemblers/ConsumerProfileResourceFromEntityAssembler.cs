using Domain.Aggregates;
using Interface.Resources;

namespace Interface.Assemblers;

public class ConsumerProfileResourceFromEntityAssembler
{
    public static ConsumerProfileResource ToResource(ConsumerProfile profile)
    {
        if (profile == null)
            throw new ArgumentNullException(nameof(profile));

        var resource = new ConsumerProfileResource
        {
            Id = profile.Id,
            UserId = profile.UserId,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            PhoneNumber = profile.PhoneNumber,
            CreatedAt = profile.CreatedAt,
            UpdatedAt = profile.UpdatedAt,
            IsActive = profile.IsActive
        };

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

        if (profile.SavingsStatistics != null)
        {
            resource.SavingsStatistics = new SavingsStatisticsResource
            {
                TotalSavings = profile.SavingsStatistics.TotalSavings,
                TransactionCount = profile.SavingsStatistics.TransactionCount,
                AverageTransactionValue = profile.SavingsStatistics.AverageTransactionValue
            };
        }

        return resource;
    }
}
