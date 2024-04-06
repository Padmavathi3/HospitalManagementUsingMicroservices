using UserManagementService.Entities;

namespace UserManagementService.Interfaces
{
    public interface UserInterface
    {
        //sign up
        public Task <string> Register(UserEntity re_var);

        //Get the details based on id
        public Task<IEnumerable<UserEntity>> GetUsersById(string id);

        //login
        public Task<string> Login(string email, string password);

        //forgotPassword
        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);

        //Reset Password
        public Task<string> ResetPasswordByEmail(string emailid, string newPassword);

        //delete user by admin
        public Task<string> DeleteUserByEmail(string email);




    }
}
