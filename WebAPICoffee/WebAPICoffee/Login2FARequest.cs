using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICoffee
{
    public class Login2FARequest
    {
        public string Login { get; set; }
        public string Token { get; set; }
    }
}