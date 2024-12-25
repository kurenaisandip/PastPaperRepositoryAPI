using System.Security.Cryptography.X509Certificates;

namespace PastPaperRepository.API;

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
    
    public static class Login
    {
        public const string LoginUser = $"{ApiBaseUrl}/login";
        public const string RegisterUser = $"{ApiBaseUrl}/register";
    }
    
    public static class EducationalEntities
    {
        public const string CreateRole = $"{ApiBaseUrl}/role";
        public const string CreateSchool = $"{ApiBaseUrl}/school";
        public const string UpdateRole = $"{ApiBaseUrl}/role/update{{id:int}}";
        public const string DeleteRole = $"{ApiBaseUrl}/role/delete{{id:int}}";
    }
    
    public static class Payments
    {
        public const string CheckoutSession = $"{ApiBaseUrl}/payment/create-checkout-session";
        public const string CreatePayment = $"{ApiBaseUrl}/payment/create-payment";

    } 
  
}