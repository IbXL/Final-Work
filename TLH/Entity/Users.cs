using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Entity
{
    public class Users : IdentityUser
    {
        public string Discriminator { get; set; }
    }
}
