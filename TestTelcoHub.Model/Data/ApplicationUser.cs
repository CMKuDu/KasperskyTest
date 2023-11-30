using Microsoft.AspNetCore.Identity;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Guid? PlanId { get; set; }
        /// <summary>
        /// 
        /// </summary>
    }
}
