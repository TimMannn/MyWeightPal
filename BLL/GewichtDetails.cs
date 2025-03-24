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
    [Range(0, 500, ErrorMessage = "Gewicht moet tussen 0 en 500 kg zijn.")]
    public double Gewicht { get; set; }

    [Required] public DateTime Datum { get; set; }
}