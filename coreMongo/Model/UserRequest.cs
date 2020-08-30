using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coreMongo.Model
{
    public class UserRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "userId cannot be empty")]
        public string userId { get; set; }

        public string name { get; set; }

        public string mobileNumber { get; set; }

    }
}
