using AppointmentManagementService.Entities;
using AppointmentManagementService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace AppointmentManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentInterface _appointment;
        private readonly HttpClient _httpClient;
        public AppointmentController(AppointmentInterface appointment, HttpClient httpClient)
        {
            _appointment = appointment;
            _httpClient = httpClient;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPost("create appointment")]
        public async Task<IActionResult> AddAppointment(AppointmentEntity updateDto)
        {
            try
            {
                return Ok(await _appointment.AddAppointment(updateDto));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        [HttpGet("GetAppointmentByDoctor")]
        //[Authorize]
        public async Task<IActionResult> GetAppointmentsByDoctor(string doctorId)
        {
            try
            {
                var values = await _appointment.GetAppointmentsByDoctor(doctorId);
                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete appointment")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId, string doctorId)
        {
            try
            {
                return Ok(await _appointment.DeleteAppointment(appointmentId,doctorId));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------

        [HttpPut("UpdateSchedule")]
        public async Task<IActionResult> UpdateAppointmentDate(int appointmentId, DateTime newDate)
        { 
            try
            {
                return Ok(await _appointment.UpdateAppointmentDate(appointmentId,newDate));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getDoctorBySpecialization")]
        public async Task<IActionResult> GetDoctorBySpecialization(string specialization)
        {
            try
            {
                var Response = await _httpClient.GetAsync($"https://localhost:7245/api/Doctor/GetDoctorBySpecialization?specialization={specialization}");
                if (Response.IsSuccessStatusCode)
                {
                    var content = await Response.Content.ReadAsStringAsync();
                    return Ok(content);
                }

                return NotFound("User Not Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
