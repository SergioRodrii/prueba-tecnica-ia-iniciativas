using BackendDotnet.DTOs;
using BackendDotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendDotnet.Controllers;

[ApiController]
[Route("initiatives")]
public sealed class InitiativesController(IInitiativeService initiativeService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<InitiativeResponse>> Create(
        CreateInitiativeRequest request,
        CancellationToken cancellationToken)
    {
        var initiative = await initiativeService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { initiativeId = initiative.Id }, initiative);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InitiativeResponse>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await initiativeService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{initiativeId:int}")]
    public async Task<ActionResult<InitiativeResponse>> GetById(int initiativeId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await initiativeService.GetByIdAsync(initiativeId, cancellationToken));
        }
        catch (InitiativeNotFoundException exception)
        {
            return NotFound(new { detail = exception.Message });
        }
    }

    [HttpPost("{initiativeId:int}/analyze")]
    public async Task<ActionResult<AnalyzeInitiativeResponse>> Analyze(int initiativeId, CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await initiativeService.AnalyzeAsync(initiativeId, cancellationToken));
        }
        catch (InitiativeNotFoundException exception)
        {
            return NotFound(new { detail = exception.Message });
        }
        catch (AnalysisServiceUnavailableException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { detail = exception.Message });
        }
        catch (AnalysisServiceFailureException exception)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new { detail = exception.Message });
        }
    }
}
