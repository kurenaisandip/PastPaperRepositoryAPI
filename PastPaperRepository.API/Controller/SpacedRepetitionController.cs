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
    public async Task<IActionResult> AddToLearningDeckAsync([FromBody] CreateLearningDeckRequest request,
        CancellationToken token = default)
    {
        var mappedRequest = request.MapToLearningDeck();
        var result = await _spacedRepetitionService.AddToLearningDeckAsync(mappedRequest, token);


        return Ok(result);
    }

    // TODO 1: map the result to response contract
    [AllowAnonymous]
    [HttpGet("api/spaced-repetition/GetLearningDeck/{userId}")]
    public async Task<IActionResult> GetLearningDeckAsync([FromRoute] long userId, CancellationToken token = default)
    {
        try
        {
            var result = await _spacedRepetitionService.GetLearningDeckAsync(userId, token);
            
            if (result == null || !result.Any())
            {
                return NotFound($"No learning deck found for userId: {userId}");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error fetching learning deck: {ex.Message}");
            
            return StatusCode(500, "An error occurred while fetching the learning deck. Please try again later.");
        }
    }


    [AllowAnonymous]
    [HttpGet("api/spaced-repetition/ShowQuestionAnswer")]
    public async Task<IActionResult> ShowQuestionAnswerAsync([FromQuery] string pastPaperId, [FromQuery] long UserId,
        CancellationToken token = default)
    {
        var result = await _spacedRepetitionService.ShowQuestionAnswerAsync(pastPaperId, UserId, token);
        return Ok(result);
    }
}