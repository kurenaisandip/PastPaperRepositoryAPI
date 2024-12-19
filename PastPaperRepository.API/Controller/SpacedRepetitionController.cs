using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Services;
using PastPaperRepository.Contracts.Requests.SpacedRepetition;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class SpacedRepetitionController : ControllerBase
{
    private readonly ISpacedRepetitionService _spacedRepetitionService;

    public SpacedRepetitionController(ISpacedRepetitionService spacedRepetitionService)
    {
        _spacedRepetitionService = spacedRepetitionService;
    }
    [AllowAnonymous]
    [HttpPost("api/spaced-repetition/AddToLearningDeck")]
    public async Task<IActionResult> AddToLearningDeckAsync([FromBody] CreateLearningDeckRequest request, CancellationToken token = default)
    {
        var mappedRequest = request.MapToLearningDeck();
        var result = await _spacedRepetitionService.AddToLearningDeckAsync(mappedRequest, token);
        
        
        return Ok(result);
    }
    
    // TODO 1: map the result to response contract
    [AllowAnonymous]
    [HttpGet("api/spaced-repetition/GetLearningDeck")]
    public async Task<IActionResult> GetLearningDeckAsync([FromQuery] long userId,  CancellationToken token = default)
    {
        var result = await _spacedRepetitionService.GetLearningDeckAsync(userId, token);
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpGet("api/spaced-repetition/ShowQuestionAnswer")]
    public async Task<IActionResult> ShowQuestionAnswerAsync([FromQuery] string pastPaperId, [FromQuery] long UserId,  CancellationToken token = default)
    {
       var result = await _spacedRepetitionService.ShowQuestionAnswerAsync(pastPaperId, UserId, token);
        return Ok(result);
    }
}