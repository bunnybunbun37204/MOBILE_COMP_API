using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ToDo.Models;
using ToDo.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Post([FromBody] Register data)
    {
        var db = new ToDoDbContext();

        var user = (from x in db.User
                    where x.Nationalid == data.NationalId
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
            Nationalid = data.NationalId,
            Title = data.Title,
            Password = hash,
            Firstname = data.FirstName,
            Lastname = data.LastName,
            Salt = Convert.ToBase64String(s)
        });
        db.SaveChanges();

        return Created();

    }

    // GET Myself get the current user by token
    [HttpGet]
    [Authorize(Roles = "user")]
    public IActionResult Get()
    {
        var db = new ToDoDbContext();

        var user = (from x in db.User
                    where x.Id == Convert.ToInt32(User.Identity.Name)
                    select x).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            NationalId = user.Id,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            Title = user.Title
        });
    }

}
