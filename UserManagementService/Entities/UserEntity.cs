using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Entities
{
    public class UserEntity
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserRole {  get; set; }
    }
}
