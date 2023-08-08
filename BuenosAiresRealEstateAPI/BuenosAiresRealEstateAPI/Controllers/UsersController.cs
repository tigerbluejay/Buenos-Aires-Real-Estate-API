using BuenosAiresRealEstate.API.Models.Identity_DTOs;
using BuenosAiresRealEstate.API.Models.Models;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/UsersAuth")]
    [ApiVersionNeutral]
    public class UsersController : Controller
    {

        private readonly IUserRepository _userRepository;
        protected APIResponse _response;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var loginResponse = await _userRepository.Login(loginRequestDTO);

            // if it returns null on either, it failed.
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token)) 
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Username does not exist or Password is Incorrect");
                return BadRequest(_response);
            }

            // user can login
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            bool userDoesNotExistInDb = _userRepository.isUniqueUser(registrationRequestDTO.UserName);

            // if false, user exists in db, we cannot proceed
            if (!userDoesNotExistInDb)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Username already exists");
                return BadRequest(_response);   
            }

            // user does not exist in db, we can proceed
            var user = await _userRepository.Register(registrationRequestDTO);

            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Errors.Add("Error while registering");
                return BadRequest(_response);  
            }

            // user registered successfully
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }
    }
}
