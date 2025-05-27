using System.Security.Claims;
using BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BLL;

public class UserService
{
    private readonly IUserData _userData;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUserData userData, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userData = userData;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<List<UserDetails>> GetAllUsers()
    {
        return await _userData.GetAllUsers();
    }

    public async Task<UserDetails> GetCurrentUser()
    {
        var userId = GetUserIdFromContext();
        return await _userData.GetCurrentUser(userId);
    }

    public async Task CreateUser(string UserName, string ProfileImageUrl)
    {
        var userId = GetUserIdFromContext();
        await _userData.CreateUser(userId, UserName, ProfileImageUrl);
    }

    public async Task EditUser(string UserName, string ProfileImageUrl)
    {
        var userId = GetUserIdFromContext();
        await _userData.EditUser(userId, UserName, ProfileImageUrl);
    }

    private string GetUserIdFromContext()
    {
        if (_httpContextAccessor.HttpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        var user = _httpContextAccessor.HttpContext.User;

        if (user == null || !user.Identity.IsAuthenticated)
            throw new InvalidOperationException("User not authenticated");

        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new InvalidOperationException("User ID claim not found");

        return userId;
    }
}