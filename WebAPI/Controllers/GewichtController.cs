using System.Security.Claims;
using BLL;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;


namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GewichtController : ControllerBase
{
    private readonly GewichtService _gewichtService;
    private readonly IHubContext<GewichtHub> _hubContext;

    public GewichtController(GewichtService gewichtservice, IHubContext<GewichtHub> hubContext)
    {
        _gewichtService = gewichtservice;
        _hubContext = hubContext;
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
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
            
            var userId = GetUserIdFromContext();
            await _hubContext.Clients.User(userId).SendAsync("GewichtUpdated");
            
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting doelgewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    private string GetUserIdFromContext()
    {
        if (HttpContext == null)
            throw new InvalidOperationException("HttpContext is null");

        var user = HttpContext.User;

        if (user == null || !user.Identity.IsAuthenticated)
            throw new InvalidOperationException("User not authenticated");

        var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            throw new InvalidOperationException("User ID claim not found");

        return userId;
    }
}
