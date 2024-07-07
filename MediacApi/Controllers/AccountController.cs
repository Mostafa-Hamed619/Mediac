using Mailjet.Client.Resources;
using MediacApi.Data.Entities;
using MediacApi.DTOs.Account;
using MediacApi.DTOs.Subscribes;
using MediacApi.HelperClasses;
using MediacApi.Services;
using MediacApi.Services.IRepositories;
using MediacBack.HelperClasses;
using MediacBack.Services.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MediacApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly JWTService jwtService;
        private readonly UserManager<Data.Entities.User> userManager;
        private readonly SignInManager<Data.Entities.User> signInManager;
        private readonly IFileRespository fileRepo;
        private readonly EmailService emailService;
        private readonly iUserRepository userRepo;
        private readonly IConfiguration config1;

        public AccountController(IConfiguration config,JWTService jwtService,
            UserManager<Data.Entities.User> userManager,
            SignInManager<Data.Entities.User> signInManager, IFileRespository fileRepo,EmailService emailService,iUserRepository userRepo
            ,IConfiguration _config)
        {
            this.config = config;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.fileRepo = fileRepo;
            this.emailService = emailService;
            this.userRepo = userRepo;
            config1 = _config;
        }
        [HttpGet("refresh-token")]
        public async Task<ActionResult<userDto>> RefreshUserDto()
        {
            var user = await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return await createUserDto(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Data.Entities.User>> Register([FromForm]RegisterDto model)
        {
            if (await checkIfUserExists(model.Email,model.UserName)) return BadRequest("This email is already used.");

            ApiResponse apiResponse = new ApiResponse();
            var userToAdd = new Data.Entities.User()
            {
                Email = model.Email.ToLower(),
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                UserName = model.UserName.ToLower()
                
            };
            if (model.photoImage != null)
            {
                var fileResult = fileRepo.SaveImage(model.photoImage, Folder.UserFolder);

                if (fileResult.Item1 == 1)
                {
                    userToAdd.PhotoImage = fileResult.Item2;
                }
            }

            var result = await userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
           
            try
            {
                if(await sendConfirmationEmailAsync(userToAdd))
                {
                    apiResponse.Title = "Registration Successfully done.";
                    apiResponse.status = true;
                    apiResponse.Body = userToAdd;
                    return Ok(apiResponse);
                }
                return BadRequest("Failed to send email.please call the moderators");
            }
            catch(Exception ex)
            {
                return BadRequest("Failed to send email.please call the moderators");
            }

        }

        [HttpPost("Login")]
        public async Task<ActionResult<userDto>> Login(LogInDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null) { return Unauthorized("User is not existing, you can register");}

            if (user.EmailConfirmed == false) return Unauthorized("Email is not confirmed, you have to confirm your email first");
            
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.IsLockedOut)
            {
                return Unauthorized("Account is locked.");
            }

            if (!result.Succeeded) { return Unauthorized("Invalid username or password."); }

            return await createUserDto(user);
        }

        [HttpPut("Confirm-Email")]
        public async Task<IActionResult> confirmEmail(ConfirmedEmailDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return Unauthorized("This email is not existed.");
            }
            ApiResponse apiresponse = new ApiResponse();
            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await userManager.ConfirmEmailAsync(user, decodedToken);
                if (result.Succeeded)
                {

                    apiresponse.Title = "Email Confirmation";
                    apiresponse.Body = "Your email has been confrimed succesfully";
                    apiresponse.status = true;
                   
                    return Ok(apiresponse);
                }
                else
                {
                    apiresponse.status = false;
                    apiresponse.Title = "Email Confirmation";
                    apiresponse.Body = "Error in email confirmation, try later please.";
                      return BadRequest(apiresponse);
                }
             
            }
            catch(Exception ex)
            {
                apiresponse.status = false;
                apiresponse.Title = "Email Confirmation";
                apiresponse.Body = "Error in email confirmation, try later please.";
                return BadRequest(apiresponse);
            }
        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> forgotpassword(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null) { return Unauthorized("This email is not existed"); }
            
            try
            {
                if(await sendForgotPasswordAsync(user))
                {
                    return Ok("forgot password email sent, check your email.");
                }
                return BadRequest("error in sending email process.");
            }
            catch(Exception ex)
            {
                return BadRequest("error in sendng email process.");
            }
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> resetpassword(ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null) { return Unauthorized("this email is not existed yet."); }

            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await userManager.ResetPasswordAsync(user, decodedToken, model.password);

             
                if (result.Succeeded)
                {
                    ApiResponse apiResponse = new ApiResponse()
                    {
                        Body = "Password reset succesfully done.",
                        Title = "Password Reset",
                        status = true
                    };
                    return Ok(apiResponse);
                }
                else
                {
                    return BadRequest("Error in password reset, try later please");
                }
            }catch(Exception ex)
            {
                return BadRequest("Error in reset password process.");
            }
        }

        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) { return Unauthorized("this email is not exitsed yet."); }
            if(user.EmailConfirmed == true) { return BadRequest("your email is already confirmed."); }
            try
            {
                ApiResponse apiResponse = new ApiResponse();
                if (await sendConfirmationEmailAsync(user))
                {
                    apiResponse.Title = "Email Confirmation.";
                    apiResponse.status = true;
                    apiResponse.Body = "Email confirmed successfully";
                    return Ok(apiResponse);
                }
                return BadRequest("Failed to send email.please call the moderators");
                
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to send email confirmation. please call the moderators.");
            }
        }

        [HttpGet("get-users")]
        [Authorize(policy: "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<getUsersDto>>> GetUsers()
        {
            var users = await userRepo.getAllUsers();
            return Ok(users);
        }

        [HttpGet("Mailjet-apiKey")]
        public IActionResult getMailJetApiKey()
        {
            var result =new
            {
                ApiKey = config1["MailJet:APIKey"],
                SecretKey = config1["MailJet:SecretKey"],
                JWTKEY = config["JWT:Key"]
            };
            return Ok(result);
        }
        #region Private Helper Methods
        private async Task<userDto> createUserDto(Data.Entities.User user)
        {
            return new userDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = await jwtService.CreateJWT(user)
            };
        }

        private async Task<bool> checkIfUserExists(string email,string userName)
        {
            return await userManager.Users.AnyAsync(x => x.Email == email.ToLower() || x.UserName == userName.ToLower());
        }

        private async Task<bool> sendConfirmationEmailAsync(Data.Entities.User user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{config["JWT:CLientUrl"]}/{config["Email:ConfirmationLinkPath"]}?token={token}&email={user.Email}";

            string htmlContent = $@"
                <h1>Email Confirmation</h1>
                <p>Hello {user.FirstName} {user.LastName} .</p>
                <p>please confirm your email by clicking this link</p>
                <a href='{url}'>Click here</a>
                <br>
                <strong>Thank you </strong>
                <br>
                <i>'{config["Email:ApplicationName"]}'</i>";

            var emailSend = new EmailSendDto(user.Email, htmlContent, "Confirm Email");

            return await emailService.sendEmail(emailSend);
        }

        private async Task<bool> sendForgotPasswordAsync(Data.Entities.User user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{config["JWT:CLientUrl"]}/{config["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";

            string htmlContent = $@"
                <h1>Reset Password</h1>
                <p>Hello {user.FirstName} {user.LastName} .</p>
                <p>Reset your password by clicking this link</p>
                <a href='{url}'>Click here</a>
                <br>
                <strong>Thank you </strong>
                <br>
                <i>'{config["Email:ApplicationName"]}'</i>";

            var emailSend = new EmailSendDto(user.Email, htmlContent, "Confirm Email");

            return await emailService.sendEmail(emailSend);
        }
        #endregion
    }
}
