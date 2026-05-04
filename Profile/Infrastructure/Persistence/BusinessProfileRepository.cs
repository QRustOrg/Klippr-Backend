using Domain.Aggregates;
using Domain.Repositories;

namespace Infrastructure.Persistence;

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

        return _context.BusinessProfiles.FirstOrDefault(bp => bp.UserId == userId);
    }

    public async Task<IEnumerable<BusinessProfile>> GetAllAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        return _context.BusinessProfiles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable();
    }

    public async Task<IEnumerable<BusinessProfile>> GetByVerificationStatusAsync(string status, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.", nameof(status));
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));
        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        return _context.BusinessProfiles
            .Where(bp => bp.VerificationStatus.Value == status)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsEnumerable();
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
}
