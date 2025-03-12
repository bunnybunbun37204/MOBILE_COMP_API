using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ToDo.Models;
using ToDo.DTOs;
using System.Security.Cryptography;

namespace ToDo.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<TokensController> _logger;

    public UsersController(ILogger<TokensController> logger)
    {
        _logger = logger;
    }

    // POST /Users register a new user
    [HttpPost]
    public IActionResult Post([FromBody] Login data)
    {
        var db = new ToDoDbContext();

        var user = (from x in db.User
                    where x.Id == data.NationalId
                    select x).FirstOrDefault();
        if (user != null)
        {
            return BadRequest("User already exists.");
        }

        byte[] s = new byte[16];
        RandomNumberGenerator.Create().GetBytes(s);


        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: data.Password,
                salt: s,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
            )
        );

        db.User.Add(new User
        {
            Id = data.NationalId,
            Password = hash,
            Salt = Convert.ToBase64String(s)
        });
        db.SaveChanges();

        return Ok();

    }

    // GET Myself get the current user by token
    [HttpGet]
    public IActionResult Get()
    {
        var db = new ToDoDbContext();

        var user = (from x in db.User
                    where x.Id == User.Identity.Name
                    select x).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            NationalId = user.Id
        });
    }

}
