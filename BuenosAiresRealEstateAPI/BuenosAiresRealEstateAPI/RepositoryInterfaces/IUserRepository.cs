using BuenosAiresRealEstate.API.Models.Identity_DTOs;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.RepositoryInterfaces
{
    public interface IUserRepository
    {
        // we need to make sure the user id is unique
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
