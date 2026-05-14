using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ConsumerProfileRepository : IConsumerProfileRepository
{
    private readonly ProfileDbContext _context;

    public ConsumerProfileRepository(ProfileDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ConsumerProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        return await _context.ConsumerProfiles.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<ConsumerProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        return await _context.ConsumerProfiles.FirstOrDefaultAsync(cp => cp.UserId == userId, cancellationToken);
    }

    public async Task<ConsumerProfile> AddAsync(ConsumerProfile aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _context.ConsumerProfiles.Add(aggregate);
        await _context.SaveChangesAsync(cancellationToken);

        return aggregate;
    }

    public async Task<ConsumerProfile> UpdateAsync(ConsumerProfile aggregate, CancellationToken cancellationToken = default)
    {
        if (aggregate == null)
            throw new ArgumentNullException(nameof(aggregate));

        _context.ConsumerProfiles.Update(aggregate);
        await _context.SaveChangesAsync(cancellationToken);

        return aggregate;
    }

    public async Task<bool> DeleteAsync(Guid profileId, CancellationToken cancellationToken = default)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID cannot be empty.", nameof(profileId));

        var profile = await _context.ConsumerProfiles.FindAsync(new object[] { profileId }, cancellationToken: cancellationToken);
        if (profile == null)
            return false;

        _context.ConsumerProfiles.Remove(profile);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
