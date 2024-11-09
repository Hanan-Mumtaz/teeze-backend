using Microsoft.AspNetCore.Mvc;
using teeze.Services;
using teeze.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace teeze.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly SignInModel _signInModel;

        public UsersController(IUserServices userServices, SignInModel signInModel)
        {
            _userServices = userServices;
            _signInModel = signInModel;
        }

        // Sign-up endpoint
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            // Check if the user already exists
            var existingUser = _userServices.GetUsersByEmail(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            // Create new user
            var newUser = new UserModel
            {
                Fullname = request.Fullname,
                Email = request.Email,
                Password = request.Password // No hashing
            };

            _userServices.CreateUser(newUser);
            return Ok(new { message = "Sign up successful" });
        }

        // Sign-in endpoint
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest request)
        {
            var result = _signInModel.Authenticate(request.Email, request.Password);

            if (result == "Sign in successful")
            {
                var user = _userServices.GetUsersByEmail(request.Email);
                if (user != null)
                {
                    return Ok(new
                    {
                        message = result,
                        fullname = user.Fullname,
                    });
                }
            }
            return BadRequest(new { message = result });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromBody] ImageUploadRequest request)
        {
            var user = _userServices.GetUsersByEmail(request.Email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (string.IsNullOrEmpty(request.Image64))
            {
                return BadRequest(new { message = "No image uploaded." });
            }

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(request.Image64);
            }
            catch (FormatException)
            {
                return BadRequest(new { message = "Invalid base64 string." });
            }

            user.Image64 = request.Image64;

            var updateResult = await _userServices.UpdateUserImageAsync(user);

            if (updateResult)
            {
                return Ok(new { message = "Image uploaded successfully" });
            }
            else
            {
                return StatusCode(500, new { message = "Error updating user image." });
            }
        }

        [HttpGet("{email}")]
        public ActionResult<UserModel> GetUsersByEmail(string email)
        {
            var user = _userServices.GetUsersByEmail(email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }




        [HttpGet("get-image-file/{email}")]
        public IActionResult GetImageFile(string email)
        {
            var user = _userServices.GetUsersByEmail(email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            if (string.IsNullOrEmpty(user.Image64))
            {
                return NotFound(new { message = "No image found for this user." });
            }

            // Return the full base64 image string as it was stored (including data:image/...;base64)
            return Ok(new { imageBase64 = user.Image64 });
        }

        [HttpGet]
        public ActionResult<List<UserModel>> GetAllUsers()
        {
            var users = _userServices.GetAllUsers();
            return Ok(users);
        }
        [HttpPost("AddSearchHistory")]
        public async Task<IActionResult> AddSearchHistory([FromBody] SearchHistoryRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.SearchHistory))
            {
                return BadRequest("Email and search text must be provided.");
            }

            try
            {
                await _userServices.AddSearchHistory(request.Email, request.SearchHistory);
                return Ok("Search text added to history successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetSearchHistory/{email}")]
        public async Task<IActionResult> GetSearchHistory(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email must be provided.");
            }

            try
            {
                var searchHistory = await _userServices.GetSearchHistory(email);
                return Ok(searchHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class SignUpRequest
        {
            public string Fullname { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class SignInRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }

        }
        public class ImageUploadRequest
        {
            public string Email { get; set; }
            public string Image64 { get; set; } 
        }
        public class SearchHistoryRequest
        {
            public string Email { get; set; }
            public string SearchHistory { get; set; }
        }
    }
}
