using Klippr_Backend.Profile.Domain.Aggregates;
using Klippr_Backend.Profile.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Klippr_Backend.Profile.Infrastructure.Persistence;

public class BusinessProfileRepository : IBusinessProfileRepository
{
    private readonly ProfileDbContext _context;

    public BusinessProfileRepository(ProfileDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<BusinessProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        return await _context.BusinessProfiles.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<BusinessProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        return await _context.BusinessProfiles.FirstOrDefaultAsync(bp => bp.UserId == userId, cancellationToken);
    }

    public async Task<IEnumerable<BusinessProfile>> GetByVerificationStatusAsync(string status, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.", nameof(status));
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        return await _context.BusinessProfiles
            .Where(bp => bp.VerificationStatus.Value == status)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<BusinessProfile> AddAsync(BusinessProfile aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _context.BusinessProfiles.Add(aggregate);
        await _context.SaveChangesAsync(cancellationToken);

        return aggregate;
    }

    public async Task<BusinessProfile> UpdateAsync(BusinessProfile aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _context.BusinessProfiles.Update(aggregate);
        await _context.SaveChangesAsync(cancellationToken);

        return aggregate;
    }

    public async Task<bool> DeleteAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(profileId));

        var profile = await _context.BusinessProfiles.FindAsync(new object[] { profileId }, cancellationToken: cancellationToken);
        if (profile == null)
            return false;

        _context.BusinessProfiles.Remove(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
