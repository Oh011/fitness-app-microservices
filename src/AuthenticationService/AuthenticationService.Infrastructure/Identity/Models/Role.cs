using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Models
{
    public class Role
    {


        public int Id {  get; set; }

        public string Name { get; set; }


        public ICollection<ApplicationUser> ? Users { get; set; } = new HashSet<ApplicationUser>();

        public string ? Description { get; set; }



        public Role(string Name,string ? description=null) {
        
        

            this.Name = Name;
            this.Description = description;
        
        }
    }
}
