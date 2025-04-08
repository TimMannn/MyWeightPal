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
    
    public async Task<GewichtDetails> GetGewicht(int id)
    {
        return await _gewichtData.GetGewicht(id);
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

    

    public async Task<List<DoelGewichtDetails>> GetDoelGewicht()
    {
        return await _gewichtData.GetDoelGewicht();
    }  
    
    public async Task SetDoelGewicht(double doelgewicht)
    {
        await _gewichtData.SetDoelGewicht(doelgewicht);
    }

    public async Task EditDoelGewicht(int idDoelGewicht, double? doelgewicht, DateTime? datumBehaald)
    {
        await _gewichtData.EditDoelGewicht(idDoelGewicht, doelgewicht, datumBehaald);
    }
    
    public async Task DeleteDoelGewicht(int idDoelGewicht)
    {
        await _gewichtData.DeleteDoelGewicht(idDoelGewicht);
    }
}