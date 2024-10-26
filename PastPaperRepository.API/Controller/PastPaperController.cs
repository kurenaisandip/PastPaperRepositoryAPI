using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Services;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Responses;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class PastPaperController : ControllerBase
{
    private readonly IPastPaperService _pastPaperService;

    public PastPaperController(IPastPaperService pastPaperService)
    {
        _pastPaperService = pastPaperService;
    }

    [HttpPost(ApiEndPoints.PastPaper.Create)]
    public async Task<IActionResult> CreatePastPapers([FromBody]CreatePastPaperRequest request, CancellationToken token)
    {
        var pastPaper = request.MapToPastPapers();
       await _pastPaperService.CreatePastPaperAsync(pastPaper, token);
        
       // Map the pastPaper object to a response DTO
       var response = pastPaper.MapToResponsePastPaper();
       // return Ok(response);
       // return Created($"/api/createpastpaper/{response.PastPaperId}", response);
       return CreatedAtAction(nameof(GetPastPaper), new { id = response.PastPaperId }, response);
    }

    [HttpGet(ApiEndPoints.PastPaper.Get)]
    public async Task<IActionResult> GetPastPaper([FromRoute] string id, CancellationToken token)
    {
        var pastPaper = await _pastPaperService.GetPastPaperByIdAsync(id, token);

        if (pastPaper is null)
        {
            return NotFound();
        }

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    } 
    
    [HttpGet(ApiEndPoints.PastPaper.GetBySlug)]
    public async Task<IActionResult> GetPastPaperBySlug([FromRoute] string idOrSlug, CancellationToken token)
    {
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperService.GetPastPaperByIdAsync(id) : await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug);
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperService.GetPastPaperByIdAsync(id) : await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug);
        
          var pastPaper =  await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug, token);
        
        if (pastPaper is null)
        {
            return NotFound();
        }

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    }

    [HttpGet(ApiEndPoints.PastPaper.GetAll)]
    public async Task<IActionResult> GetAllPastPapers(CancellationToken token)
    {
        var pastPapers = await _pastPaperService.GetAllPastPapersAsync(token);

        if (pastPapers is null)
        {
            return NotFound();
        }

        var response = pastPapers.MapToPastPapersResponse();
        
        return Ok(response);
    }

    [HttpPut(ApiEndPoints.PastPaper.Update)]
    public async Task<IActionResult> UpdatePastPaper([FromRoute] string id, [FromBody] UpdatePastPaperRequest request, CancellationToken token)
    {
        var pastPaper = request.MapToPastPapers(id);
        var updatedPastPaper = await _pastPaperService.UpdatePastPaperAsync(pastPaper, token);

        if (updatedPastPaper is null)
        {
            return NotFound();
        }
        
        var response = updatedPastPaper.MapToUpdatedPastPaper();
        return Ok(response);
    }

    [HttpDelete(ApiEndPoints.PastPaper.Delete)]
    public async Task<IActionResult> DeletePastPaper([FromRoute] string id, CancellationToken token)
    {
        var deletedPastPaper = await _pastPaperService.DeletePastPaperAsync(id, token);
        if (!deletedPastPaper)
        {
            return NotFound();
        }

        return Ok();
    }
}
    