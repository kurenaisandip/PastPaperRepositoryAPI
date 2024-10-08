namespace PastPaperRepository.Application.Models;

public class PastPapers
{
    public Guid PastPaperId { get; init; }
    public string Title { get; set; }
    public int SubjectId { get; set; }
    public int CategoryId { get; set; }
    public int Year { get; set; }
    public string ExamType { get; set; }
    public string DifficultyLevel { get; set; }
    public string ExamBoard { get; set; }
    public string FilePath { get; set; }
}