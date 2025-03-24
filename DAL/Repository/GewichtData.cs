using System.Text.Json;
using BLL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class GewichtData :IGewichtData
{
    private readonly AppDbContext _context;

    public GewichtData(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<GewichtDetails>> GetGewicht()
    {
        return await _context.Gewichten
            .Select(gm => new GewichtDetails(gm.id, gm.gewicht, gm.datumToegevoegd))
            .ToListAsync();
    }

    public async Task SetGewicht(double gewicht)
    {
        var Gewicht = new DAL.Models.GewichtModel { gewicht = gewicht, datumToegevoegd = DateTime.Now };
        _context.Gewichten.Add(Gewicht);
        await _context.SaveChangesAsync();
    }
    
    public async Task EditGewicht(int idGewicht, double gewicht)
    {
        var Gewicht = await _context.Gewichten.FirstOrDefaultAsync(gm => gm.id == idGewicht);
        if (Gewicht != null)
        {
            Gewicht.gewicht = gewicht;
            _context.Gewichten.Update(Gewicht);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task DeleteGewicht(int idGewicht)
    {
        var Gewicht = await _context.Gewichten.FindAsync(idGewicht);
        if (Gewicht != null)
        {
            _context.Gewichten.Remove(Gewicht);
            await _context.SaveChangesAsync();
        }
    }

    
/*
    public async Task<DoelGewichtDetails> GetDoelGewicht()
    {
        
    }
    
    public async Task SetDoelGewicht(double doelgewicht)
    {
        return;
    }

    public async Task EditDoelGewicht(double doelgewicht)
    {
        return;
    }
    

    public async Task DeleteDoelGewicht(int id)
    {
        
    }
    */
}