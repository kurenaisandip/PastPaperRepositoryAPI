﻿namespace PastPaperRepository.API;

public static class ApiEndPoints
{
    private const string ApiBaseUrl = "api";
    
    public static class PastPaper
    {
     private const string Base = $"{ApiBaseUrl}/pastpaper";

     public const string Create = Base;
     public const string Get = $"{Base}/{{id:guid}}";
     public const string GetBySlug = $"{Base}/{{idOrSlug}}";
     public const string GetAll = $"{Base}/all";
     public const string Update = $"{Base}/{{id:guid}}";
     public const string Delete = $"{Base}/{{id:guid}}";

    }
}