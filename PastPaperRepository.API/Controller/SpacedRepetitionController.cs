using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.Application.Services;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class SpacedRepetitionController : ControllerBase
{
    private readonly ISpacedRepetitionService _spacedRepetitionService;

    public SpacedRepetitionController(ISpacedRepetitionService spacedRepetitionService)
    {
        _spacedRepetitionService = spacedRepetitionService;
    }
    
    [HttpPost("api/spaced-repetition/AddToLearningDeck")]
    public async Task<IActionResult> AddToLearningDeckAsync([FromBody])
}