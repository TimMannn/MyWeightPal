using System.ComponentModel.DataAnnotations;

namespace BLL;

public class DoelGewichtDetails(int idDoelGewicht, double doelgewicht, DateTime datum)
{
    [Required] 
    public int Id { get; } = idDoelGewicht;
    
    [Required]
    [Range(0, 200, ErrorMessage = "Doel gewicht is niet realistisch (onder 0 of boven 200kg)")]
    public double Doelgewicht { get; } = doelgewicht;
    
    [Required]
    public DateTime Datum { get; } = datum;
}