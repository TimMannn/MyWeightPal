using BLL;
using BLL.Models;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class UserData : IUserData
{
    private readonly AppDbContext _context;

    public UserData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDetails>> GetAllUsers()
    {
        return await _context.Users
            .Select(u => new UserDetails(u.UserId, u.UserName, u.ProfileImageUrl))
            .ToListAsync();
    }

    public async Task<UserDetails> GetCurrentUser(string userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null) return null;

        return new UserDetails(user.UserId, user.UserName, user.ProfileImageUrl);
    }

    public async Task CreateUser(string userId, string userName, string profileImageUrl)
    {
        var existing = await _context.Users.AnyAsync(u => u.UserId == userId);
        if (existing)
            throw new InvalidOperationException("Gebruiker bestaat al.");

        var newUser = new UserModel
        {
            UserId = userId,
            UserName = userName,
            ProfileImageUrl = profileImageUrl
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task EditUser(string userId, string userName, string profileImageUrl)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            throw new InvalidOperationException("Gebruiker niet gevonden.");

        user.UserName = userName;
        user.ProfileImageUrl = profileImageUrl;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}