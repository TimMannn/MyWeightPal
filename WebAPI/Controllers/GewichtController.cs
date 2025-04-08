using BLL;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GewichtController : ControllerBase
{
    private readonly GewichtService _gewichtService;

    public GewichtController(GewichtService gewichtservice)
    {
        _gewichtService = gewichtservice;
    }
    
    [HttpGet("gewicht")] 
    public async Task<ActionResult<IEnumerable<DAL.Models.GewichtModel>>> GetGewicht()
    {
        try
        {
            var Gewicht = await _gewichtService.GetGewicht();
            return Ok(Gewicht);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workouts: {ex.Message}");
            return StatusCode(500, "internal server error");
        }
    }
    
    [HttpGet("gewicht{id}")]
    public async Task<ActionResult<DAL.Models.GewichtModel>> GetGewicht(int id)
    {
        var gewicht = await _gewichtService.GetGewicht(id); 
        if (gewicht == null)
        {
            return NotFound();
        }

        return Ok(gewicht);
    }

    [HttpPost("gewicht")]
    public async Task<ActionResult<DAL.Models.GewichtModel>> SetGewicht([FromBody] AddGewichtModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        await _gewichtService.SetGewicht(model.Gewicht);
        return Ok();
    }

    [HttpPut("gewicht{id}")]
    public async Task<ActionResult<DAL.Models.GewichtModel>> EditGewicht(int id, [FromBody] EditGewichtModel model)
    {
        if (id != model.Id)
        {
            return BadRequest("EditGewichtModel id mismatch in GewichtController");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        await _gewichtService.EditGewicht(model.Id, model.Gewicht);
        return Ok();
    }

    [HttpDelete("gewicht{id}")]
    public async Task<ActionResult> DeleteGewicht(int id)
    {
        await _gewichtService.DeleteGewicht(id);
        return Ok();
    }
    
    
    
    
    [HttpGet("doelgewicht")]
    public async Task<ActionResult<IEnumerable<DAL.Models.DoelGewichtModel>>> GetDoelGewicht()
    {
        try
        {
            var DoelGewicht = await _gewichtService.GetDoelGewicht();
            return Ok(DoelGewicht);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting workouts: {ex.Message}");
            return StatusCode(500, "internal server error");
        }
    }

    [HttpPost("doelgewicht")]
    public async Task<ActionResult<DAL.Models.DoelGewichtModel>> SetGewicht([FromBody] AddDoelGewichtModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        await _gewichtService.SetDoelGewicht(model.Doelgewicht);
        return Ok();
    }

    [HttpPut("doelgewicht{id}")]
    public async Task<ActionResult<DAL.Models.DoelGewichtModel>> EditDoelGewicht(int id, [FromBody] EditDoelGewichtModel model)
    {
        if (id != model.Id)
        {
            return BadRequest("EditDoelGewichtModel id mismatch in GewichtController");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        await _gewichtService.EditDoelGewicht(model.Id, model.Doelgewicht, model.Datumbehaald);
        return Ok();
    }

    [HttpDelete("doelgewicht{id}")]
    public async Task<ActionResult> DeleteDoelGewicht(int id)
    {
        await _gewichtService.DeleteDoelGewicht(id);
        return Ok();
    }
}