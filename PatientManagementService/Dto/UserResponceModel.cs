using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PatientManagementService.Dto
{
    public class UserResponceModel
    {
        
        public string UserId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string UserRole { get; set; }
        
        public string IsApproved { get; set; } = "false";
    }
}
