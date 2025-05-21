using BLL;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<GewichtModel>>> GetGewicht()
    {
        try
        {
            var gewicht = await _gewichtService.GetGewicht();
            return Ok(gewicht);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting gewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("gewicht/{id}")]
    public async Task<ActionResult<GewichtModel>> GetGewicht(int id)
    {
        try
        {
            var gewicht = await _gewichtService.GetGewicht(id);
            if (gewicht == null)
            {
                return NotFound();
            }
            return Ok(gewicht);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting gewicht by id: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("gewicht")]
    public async Task<ActionResult> SetGewicht([FromBody] AddGewichtModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            await _gewichtService.SetGewicht(model.Gewicht);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting gewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("gewicht/{id}")]
    public async Task<ActionResult> EditGewicht(int id, [FromBody] EditGewichtModel model)
    {
        if (id != model.Id)
        {
            return BadRequest("Mismatched id");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            await _gewichtService.EditGewicht(id, model.Gewicht);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing gewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("gewicht/{id}")]
    public async Task<ActionResult> DeleteGewicht(int id)
    {
        try
        {
            await _gewichtService.DeleteGewicht(id);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting gewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }



    [HttpGet("doelgewicht")]
    public async Task<ActionResult<IEnumerable<DoelGewichtModel>>> GetDoelGewicht()
    {
        try
        {
            var doelGewicht = await _gewichtService.GetDoelGewicht();
            return Ok(doelGewicht);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting doelgewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("doelgewicht")]
    public async Task<ActionResult> SetDoelGewicht([FromBody] AddDoelGewichtModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Ongeldige invoer.",
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        try
        {
            await _gewichtService.SetDoelGewicht(model.Doelgewicht);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting doelgewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("doelgewicht/{id}")]
    public async Task<ActionResult> EditDoelGewicht(int id, [FromBody] EditDoelGewichtModel model)
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

        try
        {
            await _gewichtService.EditDoelGewicht(model.Id, model.Doelgewicht, model.Datumbehaald);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing doelgewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("doelgewicht/{id}")]
    public async Task<ActionResult> DeleteDoelGewicht(int id)
    {
        try
        {
            await _gewichtService.DeleteDoelGewicht(id);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting doelgewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
