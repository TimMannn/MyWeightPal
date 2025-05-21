using System.ComponentModel.DataAnnotations;

namespace BLL;

public class GewichtDetails
{

    public GewichtDetails() {}

    public GewichtDetails(int idGewicht, double gewicht, DateTime datum, string userId)
    {
        Id = idGewicht;
        Gewicht = gewicht;
        Datum = datum;
        UserId = userId;
    }
    
    [Required]
    public int Id { get; set; }
    
    [Required]
    [Range(0, 300, ErrorMessage = "Ingevoerde gewicht is niet realistisch (onder 0 of boven 300kg)")]
    public double Gewicht { get; set; }

    [Required] 
    public DateTime Datum { get; set; }
    
    public string UserId { get; set; }
}