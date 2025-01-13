namespace PastPaperRepository.Contracts.Requests;

public class CreatePastPaperRequest
{
    public string Title { get; init; }
    public int SubjectId { get; init; }
    public int CategoryId { get; init; }
    public int Year { get; init; }
    public string ExamType { get; init; }
    public string DifficultyLevel { get; init; }
    public string ExamBoard { get; init; }
    public string FilePath { get; init; }
    
    public List<CreateQuestionRequest> QuestionRequests { get; init; }
}


public class CreateQuestionRequest
{
    public string Content { get; init; }
    public List<CreateAnswerRequest> Answers { get; init; }
}

public class CreateAnswerRequest
{
    public string Content { get; init; }
}