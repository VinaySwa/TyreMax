using Microsoft.AspNetCore.Mvc;
using TyreSaleService.Common;
using TyreSaleService.Models;
using TyreSaleService.Services;

/// <summary>
/// Controller for managing tyre companies data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TyreCompaniesController : ControllerBase
{
    private readonly ITyreCompanyService companyService;
    private readonly ITyreService tyreService;
    private readonly ILogger<TyreCompaniesController> log;

    /// <summary>
    /// Constructor for TyreCompaniesController.
    /// </summary>
    /// <param name="serviceCompany"></param>
    /// <param name="ServiceTyre"></param>
    /// <param name="logger"></param>
    public TyreCompaniesController(ITyreCompanyService serviceCompany, ITyreService ServiceTyre, ILogger<TyreCompaniesController> logger)
    {
        companyService = serviceCompany;
        tyreService = ServiceTyre;
        log = logger;
    }

    /// <summary>
    /// Retrieves all tyres companies
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TyreCompany>>> GetAll()
    {
        try
        {
            var companies = await companyService.GetAllAsync();
            return Ok(companies);
        }
        catch (DataAccessException daex)
        {
            log.LogError(daex, "Error retrieving tyre companies");
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error retrieving tyre companies");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Gets the tyre company corresponding to the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TyreCompany>> Get(int id)
    {
        try
        {
            var company = await companyService.GetByIdAsync(id);
            return Ok(company);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"Tyre company {id} not found", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException daex)
        {
            log.LogError($"Error retrieving tyre company {id}", daex);
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error retrieving tyre company {id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Gets Tyres based on given width, profile, rimsize
    /// </summary>
    /// <param name="width"></param>
    /// <param name="profile"></param>
    /// <param name="rimSize"></param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<TyreCompany>>> GetByDimensions(
            [FromQuery] int width,
            [FromQuery] int profile,
            [FromQuery] int rimSize)
    {
        try
        {
            var tyres = await tyreService.FindByDimensionsAsync(width, profile, rimSize);

            var companies = tyres
                .GroupBy(t => t.Model.Company)
                .Select(g => new TyreCompany
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Models = g
                        .GroupBy(t => t.Model)
                        .Select(mg => new TyreModel
                        {
                            Id = mg.Key.Id,
                            Name = mg.Key.Name,
                            CompanyId = mg.Key.CompanyId,
                            Tyres = mg.ToList()
                        })
                        .ToList()
                })
                .ToList();

            return Ok(companies);
        }
        catch (DataAccessException daex)
        {
            log.LogError(daex, "Error searching tyre companies by dimensions");
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error searching tyre companies by dimensions");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Creates TyreCompany
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<TyreCompany>> Create(TyreCompany company)
    {
        try
        {
            var created = await companyService.CreateAsync(company);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (DataAccessException daex)
        {
            log.LogError(daex, "Error creating tyre company");
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Unexpected error creating tyre company");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Updates TyreCompany
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update(TyreCompany company)
    {
        try
        {
            await companyService.UpdateAsync(company);
            return Ok(company);
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"Tyre company {company.Id} not found for update", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException daex)
        {
            log.LogError($"Error updating tyre company {company.Id}", daex);
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error updating tyre company {company.Id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    /// Deletes TyreCompany
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await companyService.DeleteAsync(id);
            return Ok(new { message = $"Tyre company {id} deleted successfully." });
        }
        catch (KeyNotFoundException keyex)
        {
            log.LogWarning($"Tyre company {id} not found for delete", keyex);
            return NotFound(keyex.Message);
        }
        catch (DataAccessException daex)
        {
            log.LogError($"Error deleting tyre company {id}", daex);
            return StatusCode(500, daex.Message);
        }
        catch (Exception ex)
        {
            log.LogError($"Unexpected error deleting tyre company {id}", ex);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}