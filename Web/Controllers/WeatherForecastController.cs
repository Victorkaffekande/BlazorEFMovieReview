using Entities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "GetAllMovies")]
    public IEnumerable<Movie> GetMovies()
    {
        var rep = new Repository();
        return rep.GetMovies();
    }
}