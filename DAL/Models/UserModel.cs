using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

[Table("UserModel")]
public class UserModel
{
    [Key]
    public string UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    public string? ProfileImageUrl { get; set; }
}