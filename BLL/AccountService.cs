using BLL.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL;

public class AccountService
{
    private readonly IAccountData _accountData;

    public AccountService(IAccountData accountData)
    {
        _accountData = accountData;
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
        var result = await _accountData.CreateUserAsync(user, model.Password);
        if (result.Succeeded)
        {
            return "Registration successful";
        }
        return string.Join(", ", result.Errors.Select(e => e.Description));
    }

    public async Task<string> LoginAsync(LoginModel model)
    {
        Console.WriteLine("Login Service");
        var user = await _accountData.FindByUserNameAsync(model.UserName);
        if (user == null)
        {
            Console.WriteLine("Login Service null");
            return "Invalid login attempt";
        }

        var result = await _accountData.LoginAsync(model);
        if (!result.Succeeded)
        {
            Console.WriteLine($"Login Service failed for user: {model.UserName}");
            return "Invalid login attempt";
        }

        Console.WriteLine("Login Service JWT");
        // JWT-token genereren
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyForJWT2024!ExtraLongKey123"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "MyWeightPalApi",
            audience: "MyWeightPalClient",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        Console.WriteLine($"Login Service successful for user: {model.UserName}");
        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    public async Task LogoutAsync()
    {
        await _accountData.LogoutAsync();
    }
}