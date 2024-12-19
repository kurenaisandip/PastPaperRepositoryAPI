using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Models.SpacedRepetition;

namespace PastPaperRepository.Application.Services;

public class QuestionAnswers
{
    public long Id { get; set; }
    public string PastPaperId { get; set; }
    public int QuestionNumber { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class SpacedRepetitionService : ISpacedRepetitionService
{
    private List<LearningDeck> _learningDeckRepository = new List<LearningDeck>
    {
        new LearningDeck
        {
            UserId = 101,
            PastPaperId = "PP001",
            AddedDate = new DateTime(2024, 12, 1),
            NextReviewDate = new DateTime(2024, 12, 19),
            Status = "Learning",
            QuestionNumber = null,
            Score = 0
        },
        new LearningDeck
        {
            UserId = 102,
            PastPaperId = "PP002",
            AddedDate = new DateTime(2024, 11, 15),
            NextReviewDate = new DateTime(2024, 12, 15),
            Status = "Learning",
            QuestionNumber = 10,
            Score = 90
        },
        new LearningDeck
        {
            UserId = 103,
            PastPaperId = "PP003",
            AddedDate = new DateTime(2024, 12, 5),
            NextReviewDate = new DateTime(2024, 12, 25),
            Status = "Learning",
            QuestionNumber = null,
            Score = null
        },
        new LearningDeck
        {
            UserId = 104,
            PastPaperId = "PP004",
            AddedDate = new DateTime(2024, 10, 20),
            NextReviewDate = new DateTime(2024, 12, 30),
            Status = "Learning",
            QuestionNumber = 8,
            Score = 75
        },
        new LearningDeck
        {
            UserId = 105,
            PastPaperId = "PP005",
            AddedDate = new DateTime(2024, 9, 10),
            NextReviewDate = new DateTime(2024, 12, 22),
            Status = "Learning",
            QuestionNumber = null,
            Score = null
        }
    };


    List<PastPapers> pastPapers = new List<PastPapers>
    {
        new PastPapers
        {
            PastPaperId = "PP001", Title = "Maths 2023", SubjectId = 1, CategoryId = 1, Year = 2023, ExamType = "Final",
            DifficultyLevel = "Medium", ExamBoard = "XYZ", FilePath = "/files/papers/math2023.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP002", Title = "Physics 2023", SubjectId = 2, CategoryId = 2, Year = 2023,
            ExamType = "Midterm", DifficultyLevel = "Hard", ExamBoard = "ABC",
            FilePath = "/files/papers/physics2023.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP003", Title = "Chemistry 2022", SubjectId = 3, CategoryId = 3, Year = 2022,
            ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "XYZ",
            FilePath = "/files/papers/chemistry2022.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP004", Title = "Biology 2021", SubjectId = 4, CategoryId = 4, Year = 2021,
            ExamType = "Final", DifficultyLevel = "Medium", ExamBoard = "DEF",
            FilePath = "/files/papers/biology2021.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP005", Title = "History 2020", SubjectId = 5, CategoryId = 5, Year = 2020,
            ExamType = "Final", DifficultyLevel = "Hard", ExamBoard = "ABC", FilePath = "/files/papers/history2020.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP006", Title = "Geography 2023", SubjectId = 6, CategoryId = 6, Year = 2023,
            ExamType = "Final", DifficultyLevel = "Medium", ExamBoard = "XYZ",
            FilePath = "/files/papers/geography2023.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP007", Title = "English 2021", SubjectId = 7, CategoryId = 7, Year = 2021,
            ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "DEF", FilePath = "/files/papers/english2021.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP008", Title = "Economics 2020", SubjectId = 8, CategoryId = 8, Year = 2020,
            ExamType = "Final", DifficultyLevel = "Hard", ExamBoard = "XYZ",
            FilePath = "/files/papers/economics2020.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP009", Title = "Computer Science 2022", SubjectId = 9, CategoryId = 9, Year = 2022,
            ExamType = "Midterm", DifficultyLevel = "Medium", ExamBoard = "ABC",
            FilePath = "/files/papers/compsci2022.pdf"
        },
        new PastPapers
        {
            PastPaperId = "PP010", Title = "Statistics 2023", SubjectId = 10, CategoryId = 10, Year = 2023,
            ExamType = "Final", DifficultyLevel = "Easy", ExamBoard = "DEF",
            FilePath = "/files/papers/statistics2023.pdf"
        },
    };


    // Populate the QuestionAnswers list
    List<QuestionAnswers> questionAnswers = new List<QuestionAnswers>
    {
        new QuestionAnswers
            { Id = 1, PastPaperId = "PP001", QuestionNumber = 1, Question = "What is 2+2?", Answer = "4" },
        new QuestionAnswers
            { Id = 2, PastPaperId = "PP001", QuestionNumber = 2, Question = "What is 5x3?", Answer = "15" },
        new QuestionAnswers
        {
            Id = 3, PastPaperId = "PP001", QuestionNumber = 3, Question = "State Newton's First Law.",
            Answer = "An object in motion stays in motion."
        },
        new QuestionAnswers
        {
            Id = 4, PastPaperId = "PP001", QuestionNumber = 4, Question = "What is acceleration?",
            Answer = "Rate of change of velocity."
        },
        new QuestionAnswers
            { Id = 5, PastPaperId = "PP001", QuestionNumber = 5, Question = "What is H2O?", Answer = "Water." },
        new QuestionAnswers
        {
            Id = 6, PastPaperId = "PP003", QuestionNumber = 2, Question = "What is the chemical symbol for Sodium?",
            Answer = "Na"
        },
        new QuestionAnswers
        {
            Id = 7, PastPaperId = "PP004", QuestionNumber = 1, Question = "What is photosynthesis?",
            Answer = "Process by which plants make food."
        },
        new QuestionAnswers
        {
            Id = 8, PastPaperId = "PP004", QuestionNumber = 2, Question = "What are stomata?",
            Answer = "Small openings on leaves."
        },
        new QuestionAnswers
        {
            Id = 9, PastPaperId = "PP005", QuestionNumber = 1, Question = "Who was the first president of the USA?",
            Answer = "George Washington."
        },
        new QuestionAnswers
            { Id = 10, PastPaperId = "PP005", QuestionNumber = 2, Question = "When did WW2 start?", Answer = "1939." },
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

    public Task<List<QuestionAnswers>> ShowQuestionAnswerAsync(string pastPaperId, long userId,
        CancellationToken token = default)
    {
        // Check if the past paper id exists in the pastPapers list
        var pastPaper = pastPapers.FirstOrDefault(p => p.PastPaperId == pastPaperId);
        if (pastPaper == null)
        {
            return null;
        }

        // Check if the past paper id exists in the _learningDeckRepository list for the given user
        var learningDeck =
            _learningDeckRepository.FirstOrDefault(ld => ld.PastPaperId == pastPaperId && ld.UserId == userId);
        if (learningDeck == null)
        {
            return null;
        }

        // Check if the past paper id exists in the questionAnswers list
        var result = (from learningDecks in _learningDeckRepository
            where learningDecks.UserId == userId && learningDecks.PastPaperId == pastPaperId
            join paper in pastPapers on learningDecks.PastPaperId equals paper.PastPaperId
            join question in questionAnswers on learningDecks.PastPaperId equals question.PastPaperId
            orderby learningDecks.Score, learningDecks.NextReviewDate
            select new QuestionAnswers
            {
                Id = question.Id,
                PastPaperId = question.PastPaperId,
                QuestionNumber = question.QuestionNumber,
                Question = question.Question,
                Answer = question.Answer
            }).ToList();

        // Return the result wrapped in a Task
        return Task.FromResult(result);
    }
}