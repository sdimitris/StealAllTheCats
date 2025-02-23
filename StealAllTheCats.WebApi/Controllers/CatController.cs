using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Application.Interfaces;

namespace StealAllTheCats.Controllers;

[Route("api/cats")]
[ApiController]
public class CatController : ControllerBase
{
    private readonly ICatManager _catManager;
    private readonly ICatService _catService;
    private readonly ILogger<CatController> _logger;

    public CatController(ILogger<CatController> logger, ICatManager catManager, ICatService catService)
    {
        ArgumentNullException.ThrowIfNull(_catManager = catManager);
        ArgumentNullException.ThrowIfNull(_logger = logger);
        ArgumentNullException.ThrowIfNull(_catService = catService);
    }

    [HttpPost("fetch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FetchCats()
    {
        var result = await _catManager.FetchCatsAsync();

        if (result.IsFailure)
        {
            _logger.LogError(result.Error.GetError());
            return Problem(title: result.Error.Message);
        }

        _logger.LogInformation("Cats fetched and stored successfully.");

        return Ok(new { message = "Ok" });
    }

    [HttpGet("{catId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCatById(string catId)
    {
        var catResult = await _catService.GetCatByCatIdAsync(catId);

        if (catResult.IsFailure)
        {
            _logger.LogError(catResult.Error.GetError());
            return Problem(title: catResult.Error.Message);
        }

        return catResult.Value == null ? NotFound() : Ok(catResult.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var catResult = await _catService.GetCatsAsync(page, pageSize);

        if (catResult.IsFailure)
        {
            _logger.LogError(catResult.Error.GetError());
            return Problem(title: catResult.Error.Message);
        }

        return Ok(catResult.Value);
    }

    [HttpGet("by-tag")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCatsByTag([FromQuery] string tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var catResult = await _catService.GetCatsByTagAsync(tag, page, pageSize);

        if (catResult.IsFailure)
        {
            _logger.LogError(catResult.Error.GetError());
            return Problem(title: catResult.Error.Message);
        }

        return Ok(catResult.Value);
    }
}