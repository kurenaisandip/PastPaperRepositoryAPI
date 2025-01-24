using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using PastPaperRepository.API.Auth;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Services;
using PastPaperRepository.Contracts.Requests;

namespace PastPaperRepository.API.Controller;

// [Authorize]
[ApiController]
[ApiVersion(1.0)]
public class PastPaperController : ControllerBase
{
    private readonly IPastPaperService _pastPaperService;

    public PastPaperController(IPastPaperService pastPaperService)
    {
        _pastPaperService = pastPaperService;
    }

    // [Authorize(AuthConstants.Role)]
    [AllowAnonymous]
    [HttpPost(ApiEndPoints.PastPaper.Create)]
    public async Task<IActionResult> CreatePastPapers([FromBody] CreatePastPaperRequest request,
        CancellationToken token)
    {
        var pastPaper = request.MapToPastPapers();
        await _pastPaperService.CreatePastPaperAsync(pastPaper, token);

        // Map the pastPaper object to a response DTO
        var response = pastPaper.MapToResponsePastPaper();
        // return Ok(response);
        // return Created($"/api/createpastpaper/{response.PastPaperId}", response);
        return CreatedAtAction(nameof(GetPastPaper), new { id = response.PastPaperId }, response);
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 30, VaryByHeader = "Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [HttpGet(ApiEndPoints.PastPaper.Get)]
    public async Task<IActionResult> GetPastPaper([FromRoute] string id, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var pastPaper = await _pastPaperService.GetPastPaperByIdAsync(id, userId, token);

        if (pastPaper is null) return NotFound();
        
        return Ok(pastPaper);
    }

    [ResponseCache(Duration = 30, VaryByHeader = "Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [HttpGet(ApiEndPoints.PastPaper.GetBySlug)]
    public async Task<IActionResult> GetPastPaperBySlug([FromRoute] string idOrSlug, CancellationToken token)
    {
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperService.GetPastPaperByIdAsync(id) : await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug);
        // var pastPaper = Guid.TryParse(idOrSlug, out var id) ? await _pastPaperService.GetPastPaperByIdAsync(id) : await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug);

        var pastPaper = await _pastPaperService.GetPastPaperBySlugAsync(idOrSlug, token);

        if (pastPaper is null) return NotFound();

        var response = pastPaper.MapToResponsePastPaper();
        return Ok(response);
    }

    [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "Title", "year", "sortBy"},
        VaryByHeader = "Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [HttpGet(ApiEndPoints.PastPaper.GetAll)]
    public async Task<IActionResult> GetAllPastPapers([FromQuery] GetAllPastPapersRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var options = request.MapToGetAllPastPapersOptions().WithUserId(userId);
        var pastPapers = await _pastPaperService.GetAllPastPapersAsync(options, token);
        var pastPaperCount = await _pastPaperService.GetCountAsync(options.Title, options.Year, token);
        if (pastPapers is null) return NotFound();

        var response = pastPapers.MapToPastPapersResponse(request.Page, request.PageSize, pastPaperCount);

        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpPut(ApiEndPoints.PastPaper.Update)]
    public async Task<IActionResult> UpdatePastPaper([FromRoute] string id, [FromBody] UpdatePastPaperRequest request,
        CancellationToken token)
    {
        var pastPaper = request.MapToPastPapers(id);
        var updatedPastPaper = await _pastPaperService.UpdatePastPaperAsync(pastPaper, token);

        if (updatedPastPaper is null) return NotFound();

        var response = updatedPastPaper.MapToUpdatedPastPaper();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndPoints.PastPaper.Delete)]
    public async Task<IActionResult> DeletePastPaper([FromRoute] string id, CancellationToken token)
    {
        var deletedPastPaper = await _pastPaperService.DeletePastPaperAsync(id, token);
        if (!deletedPastPaper) return NotFound();

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet(ApiEndPoints.PastPaper.DynamicModal)]
    public async Task<IActionResult> DynamicModal([FromRoute] int id, CancellationToken token)
    {
        var result = await _pastPaperService.GetDynamicPastPapersAsync(id, token);
        return Ok(result);
    }
    
    // [Authorize(Policy = AuthConstants.Role)]
       [Authorize]
    [HttpGet("api/search/{title}")]
    public async Task<IActionResult> SearchPastPapers([FromRoute] string title, CancellationToken token)
    {
        var result = await _pastPaperService.SearchPastPapersAsync(title, token);
        return Ok(result);
    }
}