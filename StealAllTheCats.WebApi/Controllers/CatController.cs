using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Application.Interfaces;

namespace StealAllTheCats.Controllers;

[Route("api/cats")]
[ApiController]
public class CatController : ControllerBase
{
    private readonly ICatService _catService;

    public CatController(ICatService catService)
    {
        _catService = catService;
    }

    [HttpPost("fetch")]
    public async Task<IActionResult> FetchCats()
    {
        await _catService.FetchAndStoreCatsAsync();
        return Ok(new { message = "Fetched 25 cats successfully!" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCatById(int id)
    {
        var cat = await _catService.GetCatByIdAsync(id);
        return cat == null ? NotFound() : Ok(cat);
    }
}