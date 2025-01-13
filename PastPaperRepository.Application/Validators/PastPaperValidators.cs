using FluentValidation;
using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Repositories;

namespace PastPaperRepository.Application.Validators;

public class PastPaperValidators : AbstractValidator<PastPapers>
{
    private readonly IPastPaperRepository _pastPaperRepository;

    public PastPaperValidators(IPastPaperRepository pastPaperRepository)
    {
        _pastPaperRepository = pastPaperRepository;
        RuleFor(x => x.PastPaperId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.UtcNow.Year);
        RuleFor(x => x.SubjectId).GreaterThan(0);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.ExamBoard).NotEmpty();
        RuleFor(x => x.DifficultyLevel).NotEmpty();
        RuleFor(x => x.ExamType).NotEmpty();
        RuleFor(x => x.FilePath).NotEmpty();
    }
}