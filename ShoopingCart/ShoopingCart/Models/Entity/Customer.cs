using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoopingCart.Models.Entity
{
    public class Customer
    {

        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Session { get; set; }

    }
}