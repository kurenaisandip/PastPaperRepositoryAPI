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
    [HttpGet("api/spaced-repetition/ShowQuestionAnswer/{pastPaperId}/{userId}")]
    public async Task<IActionResult> ShowQuestionAnswerAsync(
        [FromRoute] string pastPaperId,
        [FromRoute] long userId,
        CancellationToken token = default)
    {
        try
        {
            // Validate inputs
            if (string.IsNullOrEmpty(pastPaperId))
            {
                return BadRequest("PastPaperId cannot be null or empty.");
            }

            if (userId <= 0)
            {
                return BadRequest("UserId must be a positive number.");
            }

            // Call the service to get the question and answer data
            var result = await _spacedRepetitionService.ShowQuestionAnswerAsync(userId, pastPaperId, token);

            // Check if result is null or empty
            if (result == null || !result.Any())
            {
                return NotFound($"No data found for pastPaperId: {pastPaperId} and userId: {userId}");
            }

            return Ok(result);
        }
        catch (OperationCanceledException)
        {
            // Handle request cancellation
            return StatusCode(499, "The request was canceled.");
        }
        catch (Exception ex)
        {
            // Log the exception (replace with your logging mechanism)
            Console.Error.WriteLine($"Error in ShowQuestionAnswerAsync: {ex.Message}");

            // Return a generic error message
            return StatusCode(500, "An error occurred while processing your request. Please try again later.");
        }
    }

}