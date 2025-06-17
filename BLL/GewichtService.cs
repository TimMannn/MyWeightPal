using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace BLL;

public class GewichtService
{
    private readonly IGewichtData _gewichtData;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GewichtService(IGewichtData gewichtdata, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _gewichtData = gewichtdata;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    // Gewicht-methodes
    public async Task<List<GewichtDetails>> GetGewicht()
    {
        var userId = GetUserIdFromContext();
        return await _gewichtData.GetGewicht(userId);
    }

    public async Task<GewichtDetails> GetGewicht(int idGewicht)
    {
        var userId = GetUserIdFromContext();
        return await _gewichtData.GetGewicht(idGewicht, userId);
    }

    public async Task SetGewicht(double gewicht)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.SetGewicht(gewicht, userId);
        
    }

    public async Task EditGewicht(int idGewicht, double gewicht)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.EditGewicht(idGewicht, gewicht, userId);
    }

    public async Task DeleteGewicht(int idGewicht)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.DeleteGewicht(idGewicht, userId);
    }

    // Doelgewicht-methodes
    public async Task<List<DoelGewichtDetails>> GetDoelGewicht()
    {
        var userId = GetUserIdFromContext();
        return await _gewichtData.GetDoelGewicht(userId);
    }

    public async Task SetDoelGewicht(double doelgewicht)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.SetDoelGewicht(doelgewicht, userId);
    }

    public async Task EditDoelGewicht(int idDoelGewicht, double? doelgewicht, DateTime? datumBehaald)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.EditDoelGewicht(idDoelGewicht, doelgewicht, datumBehaald, userId);
    }

    public async Task DeleteDoelGewicht(int idDoelGewicht)
    {
        var userId = GetUserIdFromContext();
        await _gewichtData.DeleteDoelGewicht(idDoelGewicht, userId);
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
