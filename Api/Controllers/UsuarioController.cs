

using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
public class UsuarioController : ApiBaseController
{
    private readonly IUserService _userService;
    public UsuarioController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterDto model)
    {
        var result= await _userService.RegisterAsync(model);
        return Ok(result); 
    }

    [HttpPost("token")]

    public async Task<IActionResult> GetTokenAsync(LoginDto model)
    {
        var result= await _userService.GetTokenAsync(model);
        return Ok(result);
    }    

    [HttpPost("addrole")]

    public async Task<IActionResult> AddRoleAsync(AddRoleDto model)
    {
        var result= await _userService.AddRoleAsync(model);
        return Ok(result);
    }
}

