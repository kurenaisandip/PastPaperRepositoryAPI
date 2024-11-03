namespace PastPaperRepository.API.Auth;

public static class IdentityExtensions
{
 public static string? GetUserId(this HttpContext httpContext)
 {
     var userId =  httpContext.User.Claims.SingleOrDefault(x => x.Type == "userId");

     if (userId is null )
     {
         return null;
     }

     return userId.ToString();
 }
}