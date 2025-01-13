namespace PastPaperRepository.Contracts.Responses;

public class UpdatePastPaperResponse
{
    public string PastPaperId { get; init; }
    public string Title { get; init; }
    public int SubjectId { get; init; }
    public int CategoryId { get; init; }
    public int Year { get; init; }
    public string ExamType { get; init; }
    public string DifficultyLevel { get; init; }
    public string ExamBoard { get; init; }
    public string FilePath { get; init; }
}