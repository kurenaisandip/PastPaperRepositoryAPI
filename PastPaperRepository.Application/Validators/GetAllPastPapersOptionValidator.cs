using FluentValidation;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Validators;

public class GetAllPastPapersOptionValidator: AbstractValidator<GetAllPastPapersOptions>
{
    private static readonly string[] AcceptableSortFields = {"Title", "Year", "DifficultyLevel", "ExamType", "ExamBoard"};
    public GetAllPastPapersOptionValidator()
    {
        RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.UtcNow.Year);
        
        RuleFor(x => x.SortField)
            .Must(field => AcceptableSortFields.Contains(field))
            .WithMessage("Invalid sort field. Acceptable fields are: " + string.Join(", ", AcceptableSortFields));
    }
}