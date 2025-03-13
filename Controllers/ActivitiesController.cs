using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models;

namespace ToDo.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ILogger<TokensController> _logger;

    public ActivitiesController(ILogger<TokensController> logger)
    {
        _logger = logger;
    }

    // POST /Activities create a new activity
    [HttpPost]
    [Authorize(Roles = "user")]
    public IActionResult Post([FromBody] DTOs.Activity data)
    {
        var db = new ToDoDbContext();

        db.Activity.Add(new Models.Activity
        {
            Name = data.Name,
            When = data.When
        });
        db.SaveChanges();

        return Ok();
    }

    // GET /Activities get all activities
    [HttpGet]
    [Authorize(Roles = "user")]

    public IActionResult Get()
    {
        var db = new ToDoDbContext();

        return Ok(from x in db.Activity select x);
    }

    // GET /Activities/{id} get an activity by id
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var db = new ToDoDbContext();

        var activity = (from x in db.Activity
                        where x.Id == id
                        select x).FirstOrDefault();
        if (activity == null)
        {
            return NotFound();
        }

        return Ok(activity);
    }

    // PUT /Activities/{id} update an activity by id
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] DTOs.Activity data)
    {
        var db = new ToDoDbContext();

        var activity = (from x in db.Activity
                        where x.Id == id
                        select x).FirstOrDefault();
        if (activity == null)
        {
            return NotFound();
        }

        activity.Name = data.Name;
        activity.When = data.When;
        db.SaveChanges();

        return Ok();
    }

    // DELETE /Activities/{id} delete an activity by id
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var db = new ToDoDbContext();

        var activity = (from x in db.Activity
                        where x.Id == id
                        select x).FirstOrDefault();
        if (activity == null)
        {
            return NotFound();
        }

        db.Activity.Remove(activity);
        db.SaveChanges();

        return Ok();
    }
}
