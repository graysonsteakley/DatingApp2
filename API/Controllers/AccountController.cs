using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly DataContext _ctx;
    public AccountController(DataContext ctx)
    {
      _ctx = ctx;
    }

    [HttpPost("register")] // /api/account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.Username)) return BadRequest("Username is taken.");

      using var hmac = new HMACSHA512();

      var user = new AppUser()
      {
        UserName = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
      };

      _ctx.Users.Add(user);
      await _ctx.SaveChangesAsync();

      return user;
    }

    private async Task<bool> UserExists(string username)
    {
      return await _ctx.Users.AnyAsync(user => user.UserName == username.ToLower());
    }
  }
}
