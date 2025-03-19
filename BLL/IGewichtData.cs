namespace BLL;

public interface IGewichtData
{
    Task<List<GewichtDetails>> GetGewicht();
    Task SetGewicht(string gewicht);
    Task EditGewicht(string gewicht);
    Task DeleteGewicht(int id);

    //Task<DoelGewichtDetails> GetDoelGewicht();
    Task SetDoelGewicht(string gewicht);
    Task EditDoelGewicht(string gewicht);
    Task DeleteDoelGewicht(int id);
}