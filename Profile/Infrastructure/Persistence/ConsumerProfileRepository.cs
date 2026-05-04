using Domain.Aggregates;
using Domain.Repositories;

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

        return _context.ConsumerProfiles.FirstOrDefault(cp => cp.UserId == userId);
    }

    public async Task<IEnumerable<ConsumerProfile>> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        return _context.ConsumerProfiles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable();
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
}
