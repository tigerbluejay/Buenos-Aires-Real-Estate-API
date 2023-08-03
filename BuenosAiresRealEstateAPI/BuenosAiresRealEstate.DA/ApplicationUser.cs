using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Data

{
    public class ApplicationUser : IdentityUser
    { 
        public string Name { get; set; }
    }
}


/* In summary, the purpose of extending IdentityUser and adding the Name property in 
 * this code snippet is to create a custom user model that inherits all the core identity 
 * properties from IdentityUser and includes an additional property Name to store the user's name. 
 * This allows you to have a more tailored user model for your specific application, where you can
 * store and access user-related information as needed. */