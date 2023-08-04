using BuenosAiresRealEstate.API.Data;
using BuenosAiresRealEstate.API.Models.Identity_DTOs;
using BuenosAiresRealEstate.API.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;

namespace BuenosAiresRealEstate.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
                
        }
        public bool isUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true; // user doesn't exist in the db, therefore it is unique
            }
            return false; // user does exist in the db, it is not unique
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            // retrieve the user from the database
            var user = _db.ApplicationUsers
                .FirstOrDefault(x => x.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            // verify the password is valid
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                LoginResponseDTO emptyLoginResponse = new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
                return emptyLoginResponse;
            }

            // if the user is found we generate a JWT Token
            // we use our Security Key to encrypt our Token

            // STEP 1 - SET UP
            // we retrieve roles
            var roles = await _userManager.GetRolesAsync(user);
            // we create a token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            // we transform the secret key to bytes
            var secretKeytoBytes = Encoding.ASCII.GetBytes(secretKey);

            // STEP 2 - SET UP TOKEN DESCRIPTOR
            // token descriptor contains claims which identify the user, their role
            // it also contains the signing credentials and expiration date
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // we can pass multiple claims, if you have multiple roles for a user you can pass multiple claims
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                // the sign in credentials contains the transformed key using a security algorithm
                SigningCredentials = new(new SymmetricSecurityKey(secretKeytoBytes),
                SecurityAlgorithms.HmacSha256Signature)
            };

            // STEP 3 - GENERATE THE TOKEN
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // STEP 4 - GENERATE THE loginResponseDTO
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                // convert the intialized application user into a userDTO object
                User = _mapper.Map<UserDTO>(user),
                Token = tokenHandler.WriteToken(token)
            };

            return loginResponseDTO;

    }

    public async Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            // initialize application user
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToUpper(),
                Name = registrationRequestDTO.Name
            };

            // Create the user using _userManager
            var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
            
            // if the user was created
            if (result.Succeeded)
            {
                // if the role of admin doesn't exist
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    // create roles of admin and customer
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("customer"));
                }
                // and assign role of admin to user
                await _userManager.AddToRoleAsync(user, "admin");
                
                // retrieve the user to return it
                var userToReturn = _db.ApplicationUsers.
                    FirstOrDefault(x => x.UserName == registrationRequestDTO.UserName);
                
                // map the fileds of the application user to the UserDTO
                UserDTO userDTOtoreturn = _mapper.Map<UserDTO>(userToReturn);
                
                return userDTOtoreturn;
            }

            return new UserDTO();
        }
    }
}
