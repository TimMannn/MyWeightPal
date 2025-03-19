using System.ComponentModel.DataAnnotations;

namespace BLL;

public class GewichtDetails(int idGewicht, double gewicht, DateTime datum)
{
    [Required]
    public int Id { get; } = idGewicht;
    
    [Required]
    public double Gewicht { get; } = gewicht;

    [Required]
    public DateTime Datum { get; } = datum;
}