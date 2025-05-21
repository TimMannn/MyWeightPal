namespace BLL;

public interface IGewichtData
{
    Task<List<GewichtDetails>> GetGewicht(string userId);
    Task<GewichtDetails> GetGewicht(int idGewicht, string userId);
    Task SetGewicht(double gewicht, string userId);
    Task EditGewicht(int idGewicht, double gewicht, string userId);
    Task DeleteGewicht(int idGewicht, string userId);

    
    Task<List<DoelGewichtDetails>> GetDoelGewicht(string userId);
    Task SetDoelGewicht(double gewicht, string userId);
    Task EditDoelGewicht(int idDoelGewicht, double? gewicht, DateTime? datumBehaald, string userId);
    Task DeleteDoelGewicht(int idDoelGewicht, string userId);
}