using System.IdentityModel.Tokens.Jwt;
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
        var user = logInUser.MapToUsersLogin();
        var result = await _userLoginRepository.Login(user);

        if (!result)
        {
            return NotFound();
        }
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);
    
        //sends the token in the body in the response
        return Ok(token);
    }
    
    [HttpPost(ApiEndPoints.Login.RegisterUser)]
    public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
    {
        if (registerUser == null || string.IsNullOrWhiteSpace(registerUser.Name) || string.IsNullOrWhiteSpace(registerUser.Email)  || string.IsNullOrWhiteSpace(registerUser.Password))
        {
            return BadRequest("Invalid user data provided.");
        }
        
        var user = registerUser.MapToUsersRegister();
        
        var subject = "Welcome to Past Paper Repository";
        
      var istrue =  _mailgunEmailSender.SendEmailAsync(user.Email, subject, "Welcome to Past Paper Repository. You have successfully registered.");



      var result = await _userLoginRepository.Register(user);
      
        if (!result)
        {
            return StatusCode(500, "An error occurred while creating the user.");
        }
        
        return Ok("User created successfully.");
    }
}