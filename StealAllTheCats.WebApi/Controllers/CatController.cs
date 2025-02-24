using Microsoft.AspNetCore.Mvc;
using StealAllTheCats.Application.Interfaces;
using StealAllTheCats.Domain.Dtos;
using StealAllTheCats.Domain.Entities;

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

    /// <summary>
    /// Fetching cats from the Cats API.
    /// </summary>
    /// <returns></returns>
    [HttpPost("fetch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> FetchCats()
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

    /// <summary>
    /// Gets a specific cat by ID.
    /// </summary>
    /// <param name="catId">The ID of the cat.</param>
    /// <returns>The requested cat.</returns>
    [HttpGet("{catId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CatDto>> GetCatById(string catId)
    {
        var catResult = await _catService.GetCatByCatIdAsync(catId);

        if (catResult.IsFailure)
        {
            _logger.LogError(catResult.Error.GetError());
            return Problem(title: catResult.Error.Message);
        }

        return catResult.Value == null ? NotFound() : Ok(catResult.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="page"> Used for pagination </param>
    /// <param name="pageSize">Page size used for pagination </param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CatDto>>> GetCats([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var catResult = await _catService.GetCatsAsync(page, pageSize);

        if (catResult.IsFailure)
        {
            _logger.LogError(catResult.Error.GetError());
            return Problem(title: catResult.Error.Message);
        }

        return Ok(catResult.Value);
    }

    /// <summary>
    /// Fetches the cats by given tag
    /// </summary>
    /// <param name="tag">Tag name</param>
    /// <param name="page"> Used for pagination </param>
    /// <param name="pageSize">Page size used for pagination </param>
    /// <returns>A list of cats with the given tag</returns>
    [HttpGet("by-tag")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<CatDto>>> GetCatsByTag([FromQuery] string tag, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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