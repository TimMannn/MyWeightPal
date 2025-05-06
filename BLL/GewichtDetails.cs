using System.ComponentModel.DataAnnotations;

namespace BLL;

public class GewichtDetails
{

    public GewichtDetails() {}

    public GewichtDetails(int idGewicht, double gewicht, DateTime datum)
    {
        Id = idGewicht;
        Gewicht = gewicht;
        Datum = datum;
    }
    
    [Required]
    public int Id { get; set; }
    
    [Required]
    [Range(0, 300, ErrorMessage = "Ingevoerde gewicht is niet realistisch (onder 0 of boven 300kg)")]
    public double Gewicht { get; set; }

    [Required] public DateTime Datum { get; set; }
}