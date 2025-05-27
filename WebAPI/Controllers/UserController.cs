using BLL;
using BLL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("me")]
    public async Task<ActionResult<UserModel>> GetCurrentUser()
    {
        try
        {
            var user = await _userService.GetCurrentUser();
            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult> AddUser([FromBody] AddUserModel model)
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
            await _userService.CreateUser(model.UserName, model.ProfileImageUrl);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating User: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    
    [HttpPut]
    public async Task<ActionResult> EditUser([FromBody] EditUserModel model)
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
            await _userService.EditUser(model.UserName, model.ProfileImageUrl);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error editing gewicht: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
