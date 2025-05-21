using System.ComponentModel.DataAnnotations;

namespace BLL;

public class DoelGewichtDetails
{
    public DoelGewichtDetails() {}

    public DoelGewichtDetails(int idDoelGewicht, double doelgewicht, DateTime doeldatum, DateTime? doeldatumbehaald, string userId)
    {
        Id = idDoelGewicht;
        Doelgewicht = doelgewicht;
        Datum = doeldatum;
        Datumbehaald = doeldatumbehaald;
        UserId = userId;
    }
    
    [Required] 
    public int Id { get; set; }
    
    [Required]
    [Range(0, 200, ErrorMessage = "Doel gewicht is niet realistisch (onder 0 of boven 200kg)")]
    public double Doelgewicht { get; set; }
    
    [Required]
    public DateTime Datum { get; set; }
    
    public DateTime? Datumbehaald { get; set; }
    
    public string UserId { get; set; }
}