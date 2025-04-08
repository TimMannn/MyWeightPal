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

    public async Task<GewichtDetails> GetGewicht(int idGewicht)
    {
        var gewicht = _context.Gewichten.FirstOrDefault(gm => gm.id == idGewicht);
        if (gewicht == null)
        {
            return null;
        }

        return new GewichtDetails(gewicht.id, gewicht.gewicht, gewicht.datumToegevoegd);
    }

    public async Task SetGewicht(double gewicht)
    {
        var Gewicht = new DAL.Models.GewichtModel { gewicht = gewicht, datumToegevoegd = DateTime.Now.Date };
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

    
    public async Task<List<DoelGewichtDetails>> GetDoelGewicht()
    {
        return await _context.DoelGewichten
            .Select(dgm => new DoelGewichtDetails(dgm.id, dgm.doelgewicht, dgm.datumToegevoegd, dgm.datumBehaald))
            .ToListAsync();
    }
    
    public async Task SetDoelGewicht(double doelgewicht)
    {
        var doelGewicht = new DAL.Models.DoelGewichtModel { doelgewicht = doelgewicht, datumToegevoegd = DateTime.Now.Date };
        _context.DoelGewichten.Add(doelGewicht);
        await _context.SaveChangesAsync();
    }

    public async Task EditDoelGewicht(int idDoelGewicht, double? doelgewicht, DateTime? datumBehaald)
    {
        var doelGewicht = await _context.DoelGewichten.FirstOrDefaultAsync(dgm => dgm.id == idDoelGewicht);
        if (doelGewicht != null)
        {
            if (doelgewicht.HasValue)
                doelGewicht.doelgewicht = doelgewicht.Value;

            if (datumBehaald.HasValue)
                doelGewicht.datumBehaald = datumBehaald.Value;

            _context.DoelGewichten.Update(doelGewicht);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteDoelGewicht(int idDoelGewicht)
    {
        var DoelGewicht = await _context.DoelGewichten.FindAsync(idDoelGewicht);
        if (DoelGewicht != null)
        {
            _context.DoelGewichten.Remove(DoelGewicht);
            await _context.SaveChangesAsync();
        }
    }
}