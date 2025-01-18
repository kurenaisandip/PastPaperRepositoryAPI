namespace PastPaperRepository.Contracts.Responses;

public class PastPaperResponse
{
    public string PastPaperId { get; init; }
    public string Title { get; init; }

    public string Slug { get; init; }
    public int SubjectId { get; init; }
    public int CategoryId { get; init; }
    public int Year { get; init; }
    public string ExamType { get; init; }
    public string DifficultyLevel { get; init; }
    public string ExamBoard { get; init; }
    public string FilePath { get; init; }
}

public class QuestionAnswerResponse
{
    public int Id { get; init; }
    public string Question { get; init; }
    public string Answer { get; init; }
}