using Microsoft.EntityFrameworkCore;
using System;
using Teslow_srv.Domain.Dto.User;
using Teslow_srv.Domain.Entities;
using Teslow_srv.Infrastructure.Data;
using Teslow_srv.service.User;

namespace Teslow_srv.Application.Users
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<GetUserDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Users
                .AsNoTracking()
                .Select(u => new GetUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
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
                CreatedAt = u.CreatedAt
            };
        }

        public async Task<GetUserDto> CreateAsync(CreateUserDto dto, CancellationToken ct = default)
        {
            var user = new User
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                UserName = (dto.UserName ?? string.Empty).Trim()
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(ct);

            return new GetUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<GetUserDto?> UpdateAsync(Guid id, UpdateUserDto dto, CancellationToken ct = default)
        {
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (u is null) return null;

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                u.UserName = dto.UserName.Trim();

            await _db.SaveChangesAsync(ct);

            return new GetUserDto
            {
                Id = u.Id,
                UserName = u.UserName,
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
    }
}
