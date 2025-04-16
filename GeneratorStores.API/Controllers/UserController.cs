using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userManager.Users.Select(u => new
        {
            u.Id,
            u.UserName,
            u.Email,
            u.FullName
        }).ToList();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.FullName
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] ApplicationUser user)
    {
        var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Replace with proper password handling
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] ApplicationUser updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = updatedUser.FullName;
        user.Email = updatedUser.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}

