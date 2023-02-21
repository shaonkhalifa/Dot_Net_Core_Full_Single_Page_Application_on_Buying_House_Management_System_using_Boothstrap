using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP_Project.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        public string? CellPhone { get; set; }
        
        public string? Country { get; set; }


    }
}
