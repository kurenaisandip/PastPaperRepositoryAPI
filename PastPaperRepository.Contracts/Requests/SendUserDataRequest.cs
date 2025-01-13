using System.ComponentModel.DataAnnotations;

namespace PastPaperRepository.Contracts.Requests;

public class SendUserDataRequest
{
    public string UserId { get; init; }
    [Required(ErrorMessage = "InstitutionName is required.")]
    public string? InstitutionName { get; init; }

    [Required(ErrorMessage = "Grade is required.")]
    public string? Grade { get; init; }

    [Required(ErrorMessage = "Semester is required.")]
    public string? Semester { get; init; }

    [Required(ErrorMessage = "PhoneNumber is required.")]
    // [Range(1000000000, 9999999999, ErrorMessage = "PhoneNumber must be a 10-digit number.")]
    public string PhoneNumber { get; init; }

    [Required(ErrorMessage = "AcademicBackground is required.")]
     public string? AcademicBackground { get; init; }

}