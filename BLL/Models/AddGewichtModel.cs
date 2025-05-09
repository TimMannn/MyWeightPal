using System.ComponentModel.DataAnnotations;

namespace BLL.Models;

public class AddGewichtModel
{
    [Required]
    [Range(0, 300, ErrorMessage = "Ingevoerde gewicht is niet realistisch (onder 0 of boven 300kg)")]
    public double Gewicht { get; set; }
}