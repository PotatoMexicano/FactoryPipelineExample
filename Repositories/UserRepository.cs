using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pipeline.Empresa.Context;
using Pipeline.Empresa.Entities;

namespace Pipeline.Empresa.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserAsync(Int32 idUser, CancellationToken cancellation);
    Task<ICollection<User>> GetUsersAsync(CancellationToken cancellation);
}

public class UserRepository(ApplicationDbContext context, UserManager<User> userManager, ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<User?> GetUserAsync(Int32 idUser, CancellationToken cancellation)
    {
        return await userManager.FindByIdAsync(idUser.ToString());
    }

    public async Task<ICollection<User>> GetUsersAsync(CancellationToken cancellation)
    {
        return await context.Users.Select(x => new User
        {
            Id = x.Id,
            Email = x.Email,
            LastLogin = x.LastLogin,
            UserName = x.UserName,
        }).ToArrayAsync(cancellation);
    }
}
