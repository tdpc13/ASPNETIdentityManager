using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASPNETIdentityManager.Entities
{
    public class User : IdentityUser
    {
        [NotMapped]
        public List<Role> UserRoles { get; set; }
        [NotMapped]
        public List<IdentityUserClaim<string>> UserClaims { get; set; }
    }
}