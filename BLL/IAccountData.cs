using BLL.Models;
using Microsoft.AspNetCore.Identity;

namespace BLL;

public interface IAccountData
{
    Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
    Task<SignInResult> LoginAsync(LoginModel model);
    Task LogoutAsync();
    Task<IdentityUser> FindByUserNameAsync(string userName);
}