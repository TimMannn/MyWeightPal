namespace BLL;

public class GewichtService
{
    private readonly IGewichtData _gewichtData;

    public GewichtService(IGewichtData gewichtdata)
    {
        _gewichtData = gewichtdata;
    }
    
    public async Task<List<GewichtDetails>> GetGewicht()
    {
        return await _gewichtData.GetGewicht();
    }

    public async Task SetGewicht(double gewicht)
    {
        await _gewichtData.SetGewicht(gewicht);
    }
    
    public async Task EditGewicht(int idGewicht, double gewicht)
    {
        await _gewichtData.EditGewicht(idGewicht, gewicht);
    }
    
    public async Task DeleteGewicht(int idGewicht)
    {
        await _gewichtData.DeleteGewicht(idGewicht);
    }

    
/*
    public async Task<DoelGewichtDetails> GetDoelGewicht()
    {
        return new DoelGewichtDetails();
    }  
    
    public async Task SetDoelGewicht(double doelgewicht)
    {
        return;
    }

    public async Task EditDoelGewicht(double doelgewicht)
    {
        return;
    }
    

    public async Task DeleteDoelGewicht(int idDoelGewicht)
    {
        return;
    }
    */
}