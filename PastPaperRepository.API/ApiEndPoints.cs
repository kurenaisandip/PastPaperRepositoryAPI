namespace PastPaperRepository.API;

public static class ApiEndPoints
{
    private const string ApiBaseUrl = "api";
    
    public static class PastPaper
    {
     private const string Base = $"{ApiBaseUrl}/createpastpaper";

     public const string Create = Base;

    }
}