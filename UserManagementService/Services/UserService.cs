using UserManagementService.Entities;
using Dapper;
using UserManagementService.Interfaces;
using UserManagementService.Context;
using System.Data;
using UserManagementService.NestedMethods;
using UserManagementService.CustomExceptions;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagementService.Services
{
    public class UserService:UserInterface
    {
        private readonly DapperContext _context;
        private readonly IConfiguration configuration;
        private static string otp;
        private static string mailid;
        private static UserEntity entity;

        public UserService(DapperContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public async Task<string> Register(UserEntity re_var)
        {
            var query = "insert into users(UserId,FirstName,LastName,Email,Password,UserRole) values(@UserId,@FirstName,@LastName,@Email,@Password,@UserRole)";
            string encryptedPassword = UserMethods.EncryptPassword(re_var.Password);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", re_var.UserId, DbType.String);
            parameters.Add("@FirstName", re_var.FirstName, DbType.String);
            parameters.Add("@LastName", re_var.LastName, DbType.String);
            if (!UserMethods.IsValidGmailAddress(re_var.Email))
            {
                throw new InvalidEmailFormatException("Invalid Gmail address format");
            }
            else
            {
                parameters.Add("@Email", re_var.Email, DbType.String);
            }
            if (!UserMethods.IsStrongPassword(re_var.Password))
            {
                throw new InvalidPasswordException("password is invalid format");
            }
            else
            {
                parameters.Add("@Password", encryptedPassword, DbType.String);
            }
            parameters.Add("@UserRole", re_var.UserRole, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
            return "registration done successfully";
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<UserEntity>> GetUsersById(string id)
        {
            var query = "SELECT * FROM users where UserId=@UserId";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserEntity>(query, new { UserId = id });

                if (users != null && users.Any())
                {
                    return users.ToList();
                }
                else
                {
                    throw new UserNotFoundException($"User not found with ID: {id}");
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
        public async Task<string> Login(string email, string password)
        {
            var query = "SELECT * FROM users WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserEntity>(query, new { Email = email });

                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        // Decrypt the stored password
                        string storedPassword = UserMethods.DecryptPassword(user.Password);
                        Console.WriteLine(storedPassword + "   pass " + password);
                        // Compare the decrypted stored password with the provided password
                        if (password .Equals( storedPassword))
                        {
                            // Return the user immediately upon successful login
                            //return new List<UserEntity> { user };
                            String token = TokenGeneration(user);
                            return ($"Token: {token}");
                        }
                    }

                    // If the loop completes without finding a matching password, throw a PasswordMismatchException
                    throw new PasswordMissmatchException("Password does not match.");
                }
                else
                {
                    // Properly interpolate the email in the exception message
                    throw new UserNotFoundException($"User not found with email: {email}");
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------

        public Task<string> ChangePasswordRequest(string email)
        {
            try
            {
                entity = GetUsersByEmail(email).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId" + e.Message);
            }

            string generatedotp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                generatedotp += r.Next(0, 10);
            }
            otp = generatedotp;
            mailid = email;
            UserMethods.sendMail(email, generatedotp);
            Console.WriteLine(otp);
            return Task.FromResult("MailSent ✔️");
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<string> ResetPasswordByEmail(string email, string newPassword)
        {
            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(email) FROM users WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                int emailCount = await connection.ExecuteScalarAsync<int>(countQuery, new { Email = email });

                if (emailCount > 0)
                {
                    var query = "UPDATE users SET Password = @NewPassword WHERE Email = @Email";
                    string encryptedPassword = UserMethods.EncryptPassword(newPassword);
                    var parameters = new DynamicParameters();
                    parameters.Add("@NewPassword", encryptedPassword, DbType.String);
                    parameters.Add("@Email", email, DbType.String);
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return ($"password updated");
                    }
                    else
                    {
                        return ($"password doesn't update for any user");
                    }
                }
                else
                {
                    throw new UserNotFoundException($"No user found with email: {email}");
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------
        public Task<string> ChangePassword(string otp, string password)
        {
            if (otp.Equals(null))
            {
                return Task.FromResult("Generate Otp First");
            }
            if (UserMethods.DecryptPassword(entity.Password).Equals(password))
            {
                throw new PasswordMissmatchException("Dont give the existing password");
            }

            if (Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\W)(?=.*\d).{8,}$"))
            {
                if (otp.Equals(otp))
                {
                    if (ResetPasswordByEmail(mailid,password).Result.Equals("password updated"))
                    {
                        Console.WriteLine("chech 196");
                        entity = null; otp = null; mailid = null;
                        return Task.FromResult("password changed successfully");
                    }
                }
                else
                {
                    return Task.FromResult("otp miss matching");
                }
            }
            else
            {
                return Task.FromResult("regex is mismatching");
            }
            return Task.FromResult("password not changed");
            
                

        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<string> DeleteUserByEmail(string email)
        {
            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(email) FROM users WHERE Email = @Email";

            using (var connection = _context.CreateConnection())
            {
                int emailCount = await connection.ExecuteScalarAsync<int>(countQuery, new { Email = email });

                if (emailCount > 0)
                {
                    var deleteQuery = "DELETE FROM users WHERE Email = @Email";
                    rowsAffected = await connection.ExecuteAsync(deleteQuery, new { Email = email });

                    if (rowsAffected >0)
                    {
                        return ($"{rowsAffected} users are deleted");
                    }
                }
                else
                {
                    throw new UserNotFoundException($"No user found with email: {email}");
                }
            }
            return $"{rowsAffected} user(s) are deleted";
        }
        //--------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------
        private async Task<IEnumerable<UserEntity>> GetUsersByEmail(string email)
        {
            var query = "select * from users WHERE Email = @Email";
            using (var connection = _context.CreateConnection())
            {
                var person = await connection.QueryAsync<UserEntity>(query, new { Email = email });
                return person.ToList(); // Assuming there's only one user per email
            }

        }
        //---------------------------------------------------------------------------------------------------------------------------------------------
        private string TokenGeneration(UserEntity ref_var)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);
            var claims = new List<Claim>
            {
                   new Claim(ClaimTypes.Email, ref_var.Email),
                   new Claim(ClaimTypes.Role,ref_var.UserRole)
                   // Add additional claims if needed
            };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
