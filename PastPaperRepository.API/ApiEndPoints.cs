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
        public const string DynamicModal = $"{Base}/dynamic-modal/{{id:int}}";
    }

    public static class Login
    {
        public const string LoginUser = $"{ApiBaseUrl}/login";
        public const string RegisterUser = $"{ApiBaseUrl}/register";
        public const string ForgotPassword = $"{ApiBaseUrl}/forgot-password";
        public const string GetAllUserData = $"{ApiBaseUrl}/send-user-data";
    }

    public static class EducationalEntities
    {
        public const string CreateRole = $"{ApiBaseUrl}/role";
        public const string CreateSchool = $"{ApiBaseUrl}/school";
        public const string CreateCategories = $"{ApiBaseUrl}/categories";
        public const string CreateSubject = $"{ApiBaseUrl}/subject";
    }

    public static class Payments
    {
        public const string CheckoutSession = $"{ApiBaseUrl}/payment/create-checkout-session";
        public const string CreatePayment = $"{ApiBaseUrl}/payment/create-payment";
    }
}