using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagementService.Context;
using UserManagementService.Entities;
using UserManagementService.Interfaces;
using UserManagementService.Services;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserInterface _user;
        public UserController(UserInterface user)
        {
            _user = user;
        }
        //---------------------------------------------------------------------------------------------------------

        [HttpPost("Sign up")]
        public async Task<IActionResult> Register(UserEntity updateDto)
        {
            try
            {
                return Ok(await _user.Register(updateDto));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        [HttpGet("GetUsersList")]
        //[Authorize(Roles = "Doctor,Admin,Patient")]
        public async Task<IActionResult> GetUsersById(string id)
        {
            try
            {
                var values = await _user.GetUsersById(id);
                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete user")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            try
            {
                return Ok(await _user.DeleteUserByEmail(email));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------

        [HttpPut("forgot password")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePasswordRequest(String email)
        {
            return Ok(await _user.ChangePasswordRequest(email));
        }
        //------------------------------------------------------------------------------------------------------------------------

        [HttpPut("otp/{otp}/{password}")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePassword(String otp, String password)
        {
            return Ok(await _user.ChangePassword(otp, password));
        }
        //-------------------------------------------------------------------------------------------------------------------------

        [HttpPut("ResetPassWord/{personEmailUpdate}")]
        public async Task<IActionResult> ResetPasswordByEmail(string personEmailUpdate, [FromBody] UserEntity updateDto)
        {
            try
            {
                return Ok(await _user.ResetPasswordByEmail(personEmailUpdate, updateDto.Password));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Login/{email}/{password}")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                return Ok(await _user.Login(email, password));
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
