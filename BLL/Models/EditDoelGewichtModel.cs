using System.ComponentModel.DataAnnotations;

namespace BLL.Models;

public class EditDoelGewichtModel
{
    [Required] 
    public int Id { get; set; }
    
    [Required]
    [Range(0, 200, ErrorMessage = "Doel gewicht is niet realistisch (onder 0 of boven 200kg)")]
    public double? Doelgewicht { get; set; }
    
    public DateTime? Datumbehaald { get; set; }
}