using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

[Table("GewichtModel")]
public class GewichtModel
{
    [Key]
    public int id { get; set; }

    [Required]
    public double gewicht { get; set; }

    [Required]
    public DateTime datumToegevoegd { get; set; } = DateTime.Now.Date;
}