namespace PastPaperRepository.Application.Models.SpacedRepetition;

public class QuestionAnswers
{
    // public long Id { get; set; }
    // public string PastPaperId { get; set; }
    public int question_number { get; set; }
    public string question { get; set; }
    public string answer { get; set; }
    public int score { get; set; }
}