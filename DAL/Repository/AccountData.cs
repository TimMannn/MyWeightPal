using Microsoft.AspNetCore.Identity;
using BLL;
using BLL.Models;

namespace DAL.Repository;

public class AccountData : IAccountData
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountData(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<SignInResult> LoginAsync(LoginModel model)
    {
        Console.WriteLine("Login DAL");
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null)
        {
            Console.WriteLine("Login DAL fail");
            return SignInResult.Failed;
        }
        Console.WriteLine("Login DAL succes");
        return await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
    }


    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityUser> FindByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }
}