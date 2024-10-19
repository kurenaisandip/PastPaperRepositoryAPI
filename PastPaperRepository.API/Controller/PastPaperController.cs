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
       return Created($"/api/createpastpaper/{response.PastPaperId}", response);
    }

    [HttpGet(ApiEndPoints.PastPaper.Get)]
    public async Task<IActionResult> GetPastPaper([FromBody] Guid id)
    {
        var pastPaper = await _pastPaperRepository.GetPastPaperByIdAsync(id);

        if (pastPaper is null)
        {
            return NotFound();
        }

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    }
}
    