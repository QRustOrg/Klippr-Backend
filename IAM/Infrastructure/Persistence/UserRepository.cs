using Klippr_Backend.IAM.Domain.Aggregates;
using Klippr_Backend.IAM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Klippr_Backend.IAM.Domain.ValueObjects;

namespace Klippr_Backend.IAM.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly IamDbContext _context;

    public UserRepository(IamDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        var emailVo = Email.Create(email);
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == emailVo, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        return await _context.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role cannot be null or empty.", nameof(role));

        if (pageNumber < 1)
            throw new ArgumentException("Page number must be greater than 0.", nameof(pageNumber));

        if (pageSize < 1)
            throw new ArgumentException("Page size must be greater than 0.", nameof(pageSize));

        var roleVo = Role.Create(role);
        return await _context.Users
            .Where(u => u.Role == roleVo)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        var emailVo = Email.Create(email);
        return await _context.Users
            .AnyAsync(u => u.Email == emailVo, cancellationToken);
    }
}
