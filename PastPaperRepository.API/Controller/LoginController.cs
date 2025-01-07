using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Contracts.Requests;

namespace PastPaperRepository.API.Controller;


[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserLoginRepository _userLoginRepository;
    private IConfiguration _config;
    private readonly MailgunEmailSender _mailgunEmailSender;

    public LoginController(IUserLoginRepository userLoginRepository, IConfiguration config, MailgunEmailSender mailgunEmailSender)
    {
        _userLoginRepository = userLoginRepository;
        _config = config;
        _mailgunEmailSender = mailgunEmailSender;
    }

    [HttpPost(ApiEndPoints.Login.LoginUser)]
    public async Task<IActionResult> Login([FromBody] LogInUser logInUser)
    {
        Console.WriteLine("Login");
        var user = logInUser.MapToUsersLogin();
        Console.WriteLine(user.Email, user.Password);
        var result = await _userLoginRepository.Login(user);

        if (!result)
        {
            return NotFound();
        }

        var userData = await _userLoginRepository.GetUserClaimModel(user.Email);

        var boolvalue = false;

        if (userData.IsUserDataComplete == 1)
        {
            boolvalue = true;
        }

        var claims = new List<Claim>
        {
            new Claim("email", user.Email),
            new Claim("role", "User"),
            new Claim("userId", userData.Id.ToString()),
            new Claim("name", userData.Name),
            new Claim("userType", userData.UserType),
            new Claim("isUserDataComplete", boolvalue.ToString())
        };
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims:claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);
    
        //sends the token in the body in the response
        return Ok(new { accessToken = token });
    }
    
    [HttpPost(ApiEndPoints.Login.RegisterUser)]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        if (registerUser == null || string.IsNullOrWhiteSpace(registerUser.Name) || string.IsNullOrWhiteSpace(registerUser.Email)  || string.IsNullOrWhiteSpace(registerUser.Password))
        {
            return BadRequest("Invalid user data provided.");
        }
        
        var user = registerUser.MapToUsersRegister();
        
        //var subject = "Welcome to Past Paper Repository";
        
      //var istrue =  _mailgunEmailSender.SendEmailAsync(user.Email, subject, "Welcome to Past Paper Repository. You have successfully registered.");



      var result = await _userLoginRepository.Register(user);
      
        if (!result)
        {
            return StatusCode(500, "An error occurred while creating the user.");
        }
        
        return Ok("User created successfully.");
    }

    [HttpPost(ApiEndPoints.Login.GetAllUserData)]
    public async Task<IActionResult> SendUserData([FromBody] SendUserDataRequest request, CancellationToken token)
    {
        var userData = request.MapToLoggedInUserDetails();
        var result = await _userLoginRepository.SaveLoggedInUserDetails(userData, token);
        if (!result)
        {
            return StatusCode(500, "An error occurred while saving the user data.");
        }
        
        return Ok("User data saved successfully.");
    }
}