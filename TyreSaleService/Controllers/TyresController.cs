using Microsoft.AspNetCore.Mvc;
using TyreSaleService.Common;
using TyreSaleService.Models;
using TyreSaleService.Services;

/// <summary>
/// Controller for managing tyre data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TyresController : ControllerBase
{
    private readonly ITyreService tyreService;
    private readonly ILogger<TyresController> log;

    /// <summary>
    /// Constructor for TyresController.
    /// </summary>
    /// <param name="serviceTyre"></param>
    /// <param name="logger"></param>
    public TyresController(ITyreService serviceTyre, ILogger<TyresController> logger)
    {
        tyreService = serviceTyre;
        log = logger;
    }

    /// <summary>
    /// Retrieves all tyres.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tyre>>> GetAll()
    {
        try
        {
            var tyres = await tyreService.GetAllAsync();
            return Ok(tyres);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, "Error retrieving tyres");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error retrieving tyres");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Gets the tyre corresponding to the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Tyre>> Get(int id)
    {
        try
        {
            var tyre = await tyreService.GetByIdAsync(id);
            return Ok(tyre);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning(keyex, $"Tyre {id} not found");
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, $"Error retrieving tyre {id}");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Unexpected error retrieving tyre {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Create Tyre
    /// </summary>
    /// <param name="tyre"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Tyre>> Create(Tyre tyre)
    {
        try
        {
            var created = await tyreService.CreateAsync(tyre);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, "Error creating tyre");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error creating tyre");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update Tyre
    /// </summary>
    /// <param name="tyre"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update(Tyre tyre)
    {
        try
        {
            await tyreService.UpdateAsync(tyre);
            return Ok(tyre);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"Tyre {tyre.Id} not found for update", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, $"Error updating tyre {tyre.Id}");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error updating tyre {tyre.Id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Delete Tyre
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await tyreService.DeleteAsync(id);
            return Ok(new { message = $"Tyre {id} deleted successfully." });
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning(keyex, $"Tyre {id} not found for delete");
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, $"Error deleting tyre {id}");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Unexpected error deleting tyre {id}");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}

