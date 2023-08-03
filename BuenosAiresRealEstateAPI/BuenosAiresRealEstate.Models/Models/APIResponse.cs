using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuenosAiresRealEstate.API.Models.Models
{
    // this class is created with the intent that all actions in controllers
    // return the same object, which is of type API Response
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; }
        public object Result { get; set; }

        public APIResponse() 
        {
            Errors = new List<string>();
            IsSuccess = false;

        }
    }
}
