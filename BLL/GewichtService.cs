namespace BLL;

public class GewichtService
{
    public async Task<List<GewichtDetails>> GetGewicht()
    {
        return new List<GewichtDetails>();
    }

    public async Task SetGewicht(string gewicht)
    {
        return;
    }
    
    public async Task EditGewicht(string gewicht)
    {
        return;
    }
    
    public async Task DeleteGewicht(int idGewicht)
    {
        return;
    }

    
/*
    public async Task<DoelGewichtDetails> GetDoelGewicht()
    {
        return new DoelGewichtDetails();
    }
    */
    
    public async Task SetDoelGewicht(string doelgewicht)
    {
        return;
    }

    public async Task EditDoelGewicht(string doelgewicht)
    {
        return;
    }
    

    public async Task DeleteDoelGewicht(int idDoelGewicht)
    {
        return;
    }
}