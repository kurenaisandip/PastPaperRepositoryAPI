using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Models.SpacedRepetition;

namespace PastPaperRepository.Application.Services;
public class QuestionAnswer
{
    public long Id { get; set; }
    public string PastPaperId { get; set; }
    public int QuestionNumber { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
}
public class SpacedRepetitionService : ISpacedRepetitionService
{
     private List<LearningDeck> _learningDeckRepository = new List<LearningDeck>();
     
     List<PastPapers> pastPapers = new List<PastPapers>
{
    new PastPapers { PastPaperId = "PP001", Title = "Maths 2023", SubjectId = 1, CategoryId = 1, Year = 2023, ExamType = "Final", DifficultyLevel = "Medium", ExamBoard = "XYZ", FilePath = "/files/papers/math2023.pdf" },
    new PastPapers { PastPaperId = "PP002", Title = "Physics 2023", SubjectId = 2, CategoryId = 2, Year = 2023, ExamType = "Midterm", DifficultyLevel = "Hard", ExamBoard = "ABC", FilePath = "/files/papers/physics2023.pdf" },
    new PastPapers { PastPaperId = "PP003", Title = "Chemistry 2022", SubjectId = 3, CategoryId = 3, Year = 2022, ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "XYZ", FilePath = "/files/papers/chemistry2022.pdf" },
    new PastPapers { PastPaperId = "PP004", Title = "Biology 2021", SubjectId = 4, CategoryId = 4, Year = 2021, ExamType = "Final", DifficultyLevel = "Medium", ExamBoard = "DEF", FilePath = "/files/papers/biology2021.pdf" },
    new PastPapers { PastPaperId = "PP005", Title = "History 2020", SubjectId = 5, CategoryId = 5, Year = 2020, ExamType = "Final", DifficultyLevel = "Hard", ExamBoard = "ABC", FilePath = "/files/papers/history2020.pdf" },
    new PastPapers { PastPaperId = "PP006", Title = "Geography 2023", SubjectId = 6, CategoryId = 6, Year = 2023, ExamType = "Final", DifficultyLevel = "Medium", ExamBoard = "XYZ", FilePath = "/files/papers/geography2023.pdf" },
    new PastPapers { PastPaperId = "PP007", Title = "English 2021", SubjectId = 7, CategoryId = 7, Year = 2021, ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "DEF", FilePath = "/files/papers/english2021.pdf" },
    new PastPapers { PastPaperId = "PP008", Title = "Economics 2020", SubjectId = 8, CategoryId = 8, Year = 2020, ExamType = "Final", DifficultyLevel = "Hard", ExamBoard = "XYZ", FilePath = "/files/papers/economics2020.pdf" },
    new PastPapers { PastPaperId = "PP009", Title = "Computer Science 2022", SubjectId = 9, CategoryId = 9, Year = 2022, ExamType = "Midterm", DifficultyLevel = "Medium", ExamBoard = "ABC", FilePath = "/files/papers/compsci2022.pdf" },
    new PastPapers { PastPaperId = "PP010", Title = "Statistics 2023", SubjectId = 10, CategoryId = 10, Year = 2023, ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "DEF", FilePath = "/files/papers/statistics2023.pdf" },
};


        // Populate the QuestionAnswers list
        List<QuestionAnswer> questionAnswers = new List<QuestionAnswer>
        {
            new QuestionAnswer { Id = 1, PastPaperId = "PP001", QuestionNumber = 1, Question = "What is 2+2?", Answer = "4" },
            new QuestionAnswer { Id = 2, PastPaperId = "PP001", QuestionNumber = 2, Question = "What is 5x3?", Answer = "15" },
            new QuestionAnswer { Id = 3, PastPaperId = "PP001", QuestionNumber = 1, Question = "State Newton's First Law.", Answer = "An object in motion stays in motion." },
            new QuestionAnswer { Id = 4, PastPaperId = "PP001", QuestionNumber = 2, Question = "What is acceleration?", Answer = "Rate of change of velocity." },
            new QuestionAnswer { Id = 5, PastPaperId = "PP001", QuestionNumber = 1, Question = "What is H2O?", Answer = "Water." },
            new QuestionAnswer { Id = 6, PastPaperId = "PP003", QuestionNumber = 2, Question = "What is the chemical symbol for Sodium?", Answer = "Na" },
            new QuestionAnswer { Id = 7, PastPaperId = "PP004", QuestionNumber = 1, Question = "What is photosynthesis?", Answer = "Process by which plants make food." },
            new QuestionAnswer { Id = 8, PastPaperId = "PP004", QuestionNumber = 2, Question = "What are stomata?", Answer = "Small openings on leaves." },
            new QuestionAnswer { Id = 9, PastPaperId = "PP005", QuestionNumber = 1, Question = "Who was the first president of the USA?", Answer = "George Washington." },
            new QuestionAnswer { Id = 10, PastPaperId = "PP005", QuestionNumber = 2, Question = "When did WW2 start?", Answer = "1939." },
        };

    public Task<bool> AddToLearningDeckAsync(LearningDeck request, CancellationToken token = default)
    {
        try
        {
            // Add the item to the repository
            _learningDeckRepository.Add(request);

            // Return a successful result
            return Task.FromResult(true);
        }
        catch (Exception)
        {
            // Handle any unexpected issues and return false
            return Task.FromResult(false);
        }
    }

    public Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token)
    {
        var learningDecks = _learningDeckRepository
            .Where(deck => deck.UserId == userId)
            .ToList();
        
        var result = (from learningDeck in learningDecks
            join paper in pastPapers on learningDeck.PastPaperId equals paper.PastPaperId
            join questionGroup in questionAnswers.GroupBy(q => q.PastPaperId)
                on learningDeck.PastPaperId equals questionGroup.Key
            select new DeckViewModel
            {
                pastPaperId = paper.PastPaperId,
                Title = paper.Title,
                Totalquestions = questionGroup.Count()
            }).ToList();

        return Task.FromResult(result);

    }
}