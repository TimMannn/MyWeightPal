using System.Text.Json;
using BLL;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class GewichtData : IGewichtData
{
    private readonly AppDbContext _context;

    public GewichtData(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GewichtDetails>> GetGewicht(string userId)
    {
        return await _context.Gewichten
            .Where(g => g.UserId == userId)
            .Select(gm => new GewichtDetails(gm.id, gm.gewicht, gm.datumToegevoegd, gm.UserId))
            .ToListAsync();
    }

    public async Task<GewichtDetails> GetGewicht(int idGewicht, string userId)
    {
        var gewicht = await _context.Gewichten
            .FirstOrDefaultAsync(gm => gm.id == idGewicht && gm.UserId == userId);

        if (gewicht == null) return null;

        return new GewichtDetails(gewicht.id, gewicht.gewicht, gewicht.datumToegevoegd, gewicht.UserId);
    }

    public async Task SetGewicht(double gewicht, string userId)
    {
        var gewichtModel = new DAL.Models.GewichtModel
        {
            gewicht = gewicht,
            datumToegevoegd = DateTime.Now.Date,
            UserId = userId
        };
        _context.Gewichten.Add(gewichtModel);
        await _context.SaveChangesAsync();
    }

    public async Task EditGewicht(int idGewicht, double gewicht, string userId)
    {
        var gewichtModel = await _context.Gewichten
            .FirstOrDefaultAsync(gm => gm.id == idGewicht && gm.UserId == userId);

        if (gewichtModel != null)
        {
            gewichtModel.gewicht = gewicht;
            _context.Gewichten.Update(gewichtModel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteGewicht(int idGewicht, string userId)
    {
        var gewichtModel = await _context.Gewichten
            .FirstOrDefaultAsync(gm => gm.id == idGewicht && gm.UserId == userId);

        if (gewichtModel != null)
        {
            _context.Gewichten.Remove(gewichtModel);
            await _context.SaveChangesAsync();
        }
    }





public async Task<List<DoelGewichtDetails>> GetDoelGewicht(string userId)
    {
        return await _context.DoelGewichten
            .Where(dgm => dgm.UserId == userId)
            .Select(dgm => new DoelGewichtDetails(dgm.id, dgm.doelgewicht, dgm.datumToegevoegd, dgm.datumBehaald, dgm.UserId))
            .ToListAsync();
    }

    
    public async Task SetDoelGewicht(double doelgewicht, string userId)
    {
        var doelGewicht = new DAL.Models.DoelGewichtModel
        {
            doelgewicht = doelgewicht,
            datumToegevoegd = DateTime.Now.Date,
            UserId = userId
        };
        _context.DoelGewichten.Add(doelGewicht);
        await _context.SaveChangesAsync();
    }


    public async Task EditDoelGewicht(int idDoelGewicht, double? doelgewicht, DateTime? datumBehaald, string userId)
    {
        var doelGewicht = await _context.DoelGewichten
            .FirstOrDefaultAsync(dgm => dgm.id == idDoelGewicht && dgm.UserId == userId);
        
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

    public async Task DeleteDoelGewicht(int idDoelGewicht, string userId)
    {
        var DoelGewicht = await _context.DoelGewichten
            .FirstOrDefaultAsync(dgm => dgm.id == idDoelGewicht && dgm.UserId == userId);
        if (DoelGewicht != null)
        {
            _context.DoelGewichten.Remove(DoelGewicht);
            await _context.SaveChangesAsync();
        }
    }
}