namespace BLL;

public class UserDetails
{
    public UserDetails() {}

    public UserDetails(string id, string userName, string profileImageUrl)
    {
        Id = id;
        UserName = userName;
        ProfileImageUrl = profileImageUrl;
    }
    public string Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? ProfileImageUrl { get; set; }
}