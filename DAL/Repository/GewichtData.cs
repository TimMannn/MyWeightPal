using BLL;

namespace DAL.Repository;

public class GewichtData :IGewichtData
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
    
    public async Task DeleteGewicht(int id)
    {
        return;
    }

    
/*
    public async Task<DoelGewichtDetails> GetDoelGewicht()
    {
        
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
    

    public async Task DeleteDoelGewicht(int id)
    {
        
    }
}