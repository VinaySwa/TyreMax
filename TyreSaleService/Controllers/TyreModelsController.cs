using Microsoft.AspNetCore.Mvc;
using TyreSaleService.Common;
using TyreSaleService.Models;
using TyreSaleService.Services;

/// <summary>
/// Controller for managing tyre Models data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TyreModelsController : ControllerBase
{
    private readonly ITyreModelService modelService;
    private readonly ILogger<TyreModelsController> log;

    /// <summary>
    /// Constructor for TyreModelsController.
    /// </summary>
    /// <param name="serviceModel"></param>
    /// <param name="logger"></param>
    public TyreModelsController(ITyreModelService serviceModel, ILogger<TyreModelsController> logger)
    {
        modelService = serviceModel;
        log = logger;
    }

    /// <summary>
    /// Retrieves all tyres models
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TyreModel>>> GetAll()
    {
        try
        {
            var models = await modelService.GetAllAsync();
            return Ok(models);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, "Error retrieving tyre models");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error retrieving tyre models");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Gets the tyre model corresponding to the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TyreModel>> Get(int id)
    {
        try
        {
            var model = await modelService.GetByIdAsync(id);
            return Ok(model);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"TyreModel {id} not found", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError($"Error retrieving tyre model {id}", dataex);
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error retrieving tyre model {id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Create TyreModel
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TyreModel>> Create(TyreModel model)
    {
        try
        {
            var created = await modelService.CreateAsync(model);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, "Error creating tyre model");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error creating tyre model");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Update TyreModel
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update(TyreModel model)
    {
        try
        {
            await modelService.UpdateAsync(model);
            return Ok(model);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning(keyex, $"TyreModel {model.Id} not found for update");
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError(dataex, $"Error updating tyre model {model.Id}");
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Unexpected error updating tyre model {model.Id}" );
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Delete TyreModel
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await modelService.DeleteAsync(id);
            return Ok(new { message = $"TyreModel {id} deleted successfully." });
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"TyreModel {id} not found for delete", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException dataex)
        {
            log.LogError($"Error deleting tyre model {id}", dataex);
            return StatusCode(500, dataex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error deleting tyre model {id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}