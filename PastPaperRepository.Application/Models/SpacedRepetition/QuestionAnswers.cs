namespace PastPaperRepository.Application.Models.SpacedRepetition;

public class QuestionAnswers
{
    public long Id { get; set; }
    public string PastPaperId { get; set; }
    public int QuestionNumber { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
}