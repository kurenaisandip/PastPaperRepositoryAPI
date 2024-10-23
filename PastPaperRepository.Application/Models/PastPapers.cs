using System.Text.RegularExpressions;

namespace PastPaperRepository.Application.Models;

public partial class PastPapers
{
    public Guid PastPaperId { get; init; }
    public string Title { get; set; }

    public string Slug => generateSlug();
    public int SubjectId { get; set; }
    public int CategoryId { get; set; }
    public int Year { get; set; }
    public string ExamType { get; set; }
    public string DifficultyLevel { get; set; }
    public string ExamBoard { get; set; }
    public string FilePath { get; set; }
    
    private string generateSlug()
    {
        // var slugTitle = Regex.Replace(Title, "[^a-zA-Z0-9]", String.Empty).ToLower().Replace("", "-");
        var slugTitle = SlugRegex().Replace(Title, String.Empty)  // Remove non-alphanumeric characters except spaces
            .Trim()  // Remove leading and trailing whitespace
            .Replace(" ", "-")  // Replace spaces with dashes
            .ToLower();  // Convert to lowercase
        return $"{slugTitle}-{Year}";
    }

    [GeneratedRegex("[^a-zA-Z0-9 ]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}