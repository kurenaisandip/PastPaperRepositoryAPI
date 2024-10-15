using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repository;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Responses;

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
        var pastPaper = new PastPapers()
        {
            PastPaperId = Guid.NewGuid(),
            Title = request.Title,
            SubjectId = request.SubjectId,
            CategoryId = request.CategoryId,
            Year = request.Year,
            ExamType = request.ExamType,
            DifficultyLevel = request.DifficultyLevel,
            ExamBoard = request.ExamBoard,
            FilePath = request.FilePath
        };
       await _pastPaperRepository.CreatePaspaperAsync(pastPaper);
        
       // Map the pastPaper object to a response DTO
       var response = new PastPaperResponse()
       {
           PastPaperId = pastPaper.PastPaperId,
           Title = pastPaper.Title,
           SubjectId = pastPaper.SubjectId,
           CategoryId = pastPaper.CategoryId,
           Year = pastPaper.Year,
           ExamType = pastPaper.ExamType,
           DifficultyLevel = pastPaper.DifficultyLevel,
           ExamBoard = pastPaper.ExamBoard,
           FilePath = pastPaper.FilePath
       };

       // return Ok(response);
       return Created($"/api/createpastpaper/{response.PastPaperId}", response);
    }
}
    