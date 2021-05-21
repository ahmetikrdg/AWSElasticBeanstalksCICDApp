using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWSElasticBeanstalksCICDApp.Models
{
    public class UserRegisterModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
