using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Identity_DTOs
{
    internal class LoginResponseDTO
    {
        // user will have all the details of the loggedIn User
        public UserDTO User { get; set; }
        // the token is a string that authenticates the user
        // it proves that the user is who they say they are
        // the role of the user is encrypted in the Token
        public string Token { get; set; }

    }
}
