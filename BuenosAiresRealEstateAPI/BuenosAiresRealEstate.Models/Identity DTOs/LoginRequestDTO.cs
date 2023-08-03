using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Identity_DTOs
{
    internal class LoginRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
