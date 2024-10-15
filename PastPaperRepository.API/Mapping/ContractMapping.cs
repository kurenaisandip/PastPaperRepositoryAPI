using PastPaperRepository.Application.Models;
using PastPaperRepository.Contracts.Requests;
using PastPaperRepository.Contracts.Responses;

namespace PastPaperRepository.API.Mapping;

public static class ContractMapping
{
    public static PastPapers MapToPastPapers(this CreatePastPaperRequest request)
    {
        return new PastPapers()
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
    }

    public static PastPaperResponse MapToResponsePastPaper(this PastPapers pastPaper)
    {
        return new PastPaperResponse()
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

    }
}