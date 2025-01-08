namespace PastPaperRepository.Application.Models;

public class LoggedInUserDetails
{
    public int UserId { get; set; }
    public string? InstitutionName { get; set; }
    public int? Grade { get; set; }
    public string? Semester { get; set; }
    public string PhoneNumber { get; set; }
    public string? AcademicBackground { get; set; }
    
}