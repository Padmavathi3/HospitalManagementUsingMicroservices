using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientManagementService.Dto;
using PatientManagementService.Entities;
using PatientManagementService.Interfaces;
using PatientManagementService.Services;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PatientManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientInterface _patient;
        private readonly HttpClient _httpClient;
        public PatientController(PatientInterface patient, HttpClient httpClient)
        {
            _patient = patient;
            _httpClient = httpClient;
        }
        //---------------------------------------------------------------------------------------------------------

        [HttpPost("create patient")]
        public async Task<IActionResult> CreatePatient(PatientEntity updateDto)
        {
            try
            {
                return Ok(await _patient.CreatePatient(updateDto));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        [HttpGet("GetPatientById")]
        //[Authorize]
        public async Task<IActionResult> GetUsersById(string id)
        {
            try
            {
                var values = await _patient.GetPatientById(id);
                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------

        [HttpGet("get patient all details")]
        public async Task<IActionResult> GetPatientDetails(string userid)
        {
            try
            {
                // Make a GET request to the GetDoctorsBySpecialization endpoint
                var userResponse = await _httpClient.GetAsync($"https://localhost:7282/api/User/GetUsersList?id={userid}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var content = await userResponse.Content.ReadAsStringAsync();
                    //var responseObject = JsonSerializer.Deserialize<List<UserResponceModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return Ok(content);
                }
                return NotFound("User Not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------

        /*//[Authorize]
        [HttpGet("GetPatientDetailss")]
        public async Task<IActionResult> GetPatientDetails(string userid)
        {
            try
            {
                var values = await _patient.GetPatientById(userid);

                if (values != null)
                {
                    var userResponse = await _httpClient.GetAsync($"https://localhost:7282/api/User/GetUsersList?id={userid}");

                    if (userResponse.IsSuccessStatusCode)
                    {
                        var content = await userResponse.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<List<UserResponceModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        // Merge user and patient data into a single object
                        var mergedData = new
                        {
                            PatientData = values,
                            UserData = responseObject
                        };

                        return Ok(new { Success = true, Message = "User and Patient details retrieved successfully", Data = mergedData });
                    }
                    else
                    {
                        return StatusCode(500, new { Success = false, Message = "Failed to retrieve user details" });
                    }
                }
                else
                {
                    return StatusCode(500, new { Success = false, Message = "Failed to retrieve patient details" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "An unexpected error occurred." });
            }
        }*/

        //--------------------------------------------------------------------------------------------------------------------------------------------------------

    }

}
