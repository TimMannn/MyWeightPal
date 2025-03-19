using System.ComponentModel.DataAnnotations;

namespace BLL;

public class DoelGewichtDetails(int idDoelGewicht, double doelgewicht, DateTime datum)
{
    [Required] 
    public int Id { get; } = idDoelGewicht;
    
    [Required]
    public double Doelgewicht { get; } = doelgewicht;
    
    [Required]
    public DateTime Datum { get; } = datum;
}