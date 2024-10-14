using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.Application.Repository;
using PastPaperRepository.Contracts.Requests;

namespace PastPaperRepository.API.Controller;

[ApiController]
[Route("api")]
public class PastPaperController : ControllerBase
{
    private readonly IPastPaperRepository _pastPaperRepository;

    public PastPaperController(IPastPaperRepository pastPaperRepository)
    {
        _pastPaperRepository = pastPaperRepository;
    }

    [HttpPost("createpastpaper")]
    public async Task<IActionResult> CreatePastPapers([FromBody]CreatePastPaperRequest request)
    {
        return Ok(request);
    }
}
    