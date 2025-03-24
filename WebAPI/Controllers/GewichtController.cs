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
    
    [HttpGet]
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

    [HttpPost]
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

    [HttpPut("{id}")]
    public async Task<ActionResult<DAL.Models.GewichtModel>> EditGewicht(int id, [FromBody] EditGewichtModel model)
    {
        if (id != model.id)
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

        await _gewichtService.EditGewicht(model.id, model.Gewicht);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGewicht(int id)
    {
        await _gewichtService.DeleteGewicht(id);
        return Ok();
    }
}