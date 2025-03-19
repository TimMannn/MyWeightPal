using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

[Table("DoelGewichtModel")]
public class DoelGewichtModel
{
    [Key]
    public int id { get; set; }

    [Required]
    public double doelgewicht { get; set; }

    [Required]
    public DateTime datumToegevoegd { get; set; } = DateTime.Now;
}