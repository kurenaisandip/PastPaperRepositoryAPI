using RestSharp;
using RestSharp.Authenticators;

namespace PastPaperRepository.API;

public class MailgunEmailSender
{
    private readonly string _apiKey;
    private readonly string _domain;
    private string _baseUrl = "https://api.mailgun.net/v3";
    
    public MailgunEmailSender(string apiKey, string domain)
    {
        _apiKey = apiKey;
        _domain = domain;
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body, string from = null, bool isHtml = false)
    {
        
        var options = new RestClientOptions(_baseUrl) {
            Authenticator = new HttpBasicAuthenticator("api", _apiKey)
        };
        RestClient client = new RestClient(options);
       

        var request = new RestRequest($"{_domain}/messages", Method.Post);
        request.AddParameter("to", to);
        request.AddParameter("from", from ?? $"mailgun@{_domain}");
        request.AddParameter("subject", subject);

        if (isHtml)
        {
            request.AddParameter("html", body);
        }
        else
        {
            request.AddParameter("text", body);
        }

        var response = await client.ExecuteAsync(request);

        return response.IsSuccessful;
    }
}