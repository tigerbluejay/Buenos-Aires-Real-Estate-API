using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Identity_DTOs
{
    // we have a registration request DTO but not a registration response DTO
    // this is because we just send them a 200 OK response if registration is successful
    public class RegistrationRequestDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
