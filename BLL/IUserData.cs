namespace BLL;

public interface IUserData
{
    Task<List<UserDetails>> GetAllUsers();
    Task<UserDetails> GetCurrentUser(string userId);
    Task CreateUser(string userId, string UserName, string ProfileImageUrl);
    Task EditUser(string userId, string UserName, string ProfileImageUrl);
}

