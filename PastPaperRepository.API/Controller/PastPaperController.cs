using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repository;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Responses;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class PastPaperController : ControllerBase
{
    private readonly IPastPaperRepository _pastPaperRepository;

    public PastPaperController(IPastPaperRepository pastPaperRepository)
    {
        _pastPaperRepository = pastPaperRepository;
    }

    [HttpPost(ApiEndPoints.PastPaper.Create)]
    public async Task<IActionResult> CreatePastPapers([FromBody]CreatePastPaperRequest request)
    {
        var pastPaper = request.MapToPastPapers();
       await _pastPaperRepository.CreatePaspaperAsync(pastPaper);
        
       // Map the pastPaper object to a response DTO
       var response = pastPaper.MapToResponsePastPaper();
       // return Ok(response);
       // return Created($"/api/createpastpaper/{response.PastPaperId}", response);
       return CreatedAtAction(nameof(GetPastPaper), new { idOrSlug = response.PastPaperId }, response);
    }

    [HttpGet(ApiEndPoints.PastPaper.Get)]
    public async Task<IActionResult> GetPastPaper([FromRoute] string id)
    {
        var pastPaper = await _pastPaperRepository.GetPastPaperByIdAsync(id);

        if (pastPaper is null)
        {
            return NotFound();
        }

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    } 
    
    [HttpGet(ApiEndPoints.PastPaper.GetBySlug)]
    public async Task<IActionResult> GetPastPaperBySlug([FromRoute] string idOrSlug)
    {
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperRepository.GetPastPaperByIdAsync(id) : await _pastPaperRepository.GetPastPaperBySlugAsync(idOrSlug);
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperRepository.GetPastPaperByIdAsync(id) : await _pastPaperRepository.GetPastPaperBySlugAsync(idOrSlug);
        
          var pastPaper =  await _pastPaperRepository.GetPastPaperBySlugAsync(idOrSlug);
        
        if (pastPaper is null)
        {
            return NotFound();
        }

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    }

    [HttpGet(ApiEndPoints.PastPaper.GetAll)]
    public async Task<IActionResult> GetAllPastPapers()
    {
        var pastPapers = await _pastPaperRepository.GetAllPastPapersAsync();

        if (pastPapers is null)
        {
            return NotFound();
        }

        var response = pastPapers.MapToPastPapersResponse();
        
        return Ok(response);
    }

    [HttpPut(ApiEndPoints.PastPaper.Update)]
    public async Task<IActionResult> UpdatePastPaper([FromRoute] string id, [FromBody] UpdatePastPaperRequest request)
    {
        var pastPaper = request.MapToPastPapers(id);
        var updatedPastPaper = await _pastPaperRepository.UpdatePastPaperAsync(pastPaper);

        if (!updatedPastPaper)
        {
            return NotFound();
        }
        
        var response = pastPaper.MapToUpdatedPastPaper();
        return Ok(response);
    }

    [HttpDelete(ApiEndPoints.PastPaper.Delete)]
    public async Task<IActionResult> DeletePastPaper([FromRoute] string id)
    {
        var deletedPastPaper = await _pastPaperRepository.DeletePastPaperAsync(id);
        if (!deletedPastPaper)
        {
            return NotFound();
        }

        return Ok();
    }
}
    