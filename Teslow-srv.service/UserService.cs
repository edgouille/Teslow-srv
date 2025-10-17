using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Teslow_srv.Domain.Dto.Auth;
using Teslow_srv.Domain.Dto.User;
using Teslow_srv.Domain.Entities;
using Teslow_srv.Infrastructure.Data;
using Teslow_srv.Service.Interface;

namespace Teslow_srv.Service
{
    public class UserService : IUserService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<GetUserDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .Select(u => new GetUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    CanonicalName = u.CanonicalName,
                    Age = u.Age,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<GetUserDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var u = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (u is null) return null;

            return new GetUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                CanonicalName = u.CanonicalName,
                Age = u.Age,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            };
        }

        public async Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            var userName = (dto.UserName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name is required.");
            }

            var password = dto.Password ?? string.Empty;
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Password is required.");
            }

            if (await _db.Users.AnyAsync(u => u.UserName == userName, ct))
            {
                throw new ArgumentException("A user with the same user name already exists.");
            }

            var displayName = string.IsNullOrWhiteSpace(dto.DisplayName)
                ? userName
                : dto.DisplayName.Trim();

            var canonicalName = string.IsNullOrWhiteSpace(dto.CanonicalName)
                ? BuildCanonicalName(displayName)
                : BuildCanonicalName(dto.CanonicalName);

            var user = new User
            {
                Id = dto.Id.HasValue && dto.Id.Value != Guid.Empty
                    ? dto.Id.Value
                    : Guid.NewGuid(),
                UserName = userName,
                DisplayName = displayName,
                CanonicalName = canonicalName,
                Age = dto.Age,
                PasswordHash = HashPassword(password),
                Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role.Trim()
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            return new GetUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                CanonicalName = user.CanonicalName,
                Age = user.Age,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<GetUserDto?> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default)
        {
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (u is null) return null;

            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                var normalized = dto.UserName.Trim();
                if (await _db.Users.AnyAsync(x => x.Id != id && x.UserName == normalized, ct))
                {
                    throw new ArgumentException("A user with the same user name already exists.");
                }

                u.UserName = normalized;
            }

            if (dto.DisplayName is not null)
            {
                var normalized = dto.DisplayName.Trim();
                u.DisplayName = string.IsNullOrWhiteSpace(normalized) ? null : normalized;
            }

            if (dto.CanonicalName is not null)
            {
                var normalized = dto.CanonicalName.Trim();
                u.CanonicalName = string.IsNullOrWhiteSpace(normalized)
                    ? null
                    : BuildCanonicalName(normalized);
            }

            if (dto.Age.HasValue)
            {
                u.Age = dto.Age.Value;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                u.PasswordHash = HashPassword(dto.Password);
            }

            if (!string.IsNullOrWhiteSpace(dto.Role))
            {
                u.Role = dto.Role.Trim();
            }

            await _db.SaveChangesAsync(ct);

            return new GetUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                CanonicalName = u.CanonicalName,
                Age = u.Age,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var u = await _db.Users.FindAsync([id], ct);
            if (u is null) return false;

            _db.Users.Remove(u);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<AuthenticatedUserDto?> ValidateCredentialsAsync(string userName, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var normalized = userName.Trim();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == normalized, ct);
            if (user is null)
            {
                return null;
            }

            return VerifyPassword(password, user.PasswordHash)
                ? new AuthenticatedUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Role = user.Role
                }
                : null;
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);
            var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static string? BuildCanonicalName(string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            var trimmed = source.Trim().ToLowerInvariant();
            var filtered = trimmed.Where(char.IsLetterOrDigit).ToArray();
            return new string(filtered);
        }
    }
}
