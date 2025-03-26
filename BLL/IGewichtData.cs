namespace BLL;

public interface IGewichtData
{
    Task<List<GewichtDetails>> GetGewicht();
    Task SetGewicht(double gewicht);
    Task EditGewicht(int idGewicht, double gewicht);
    Task DeleteGewicht(int idGewicht);

    
    Task<List<DoelGewichtDetails>> GetDoelGewicht();
    Task SetDoelGewicht(double gewicht);
    Task EditDoelGewicht(int idDoelGewicht, double gewicht);
    Task DeleteDoelGewicht(int idDoelGewicht);
}