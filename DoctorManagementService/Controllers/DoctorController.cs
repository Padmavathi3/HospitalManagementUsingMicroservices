using DoctorManagementService.Entities;
using DoctorManagementService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoctorManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorInterface _doctor;
        public DoctorController(DoctorInterface doctor)
        {
            _doctor = doctor;
        }
        //---------------------------------------------------------------------------------------------------------


        [HttpPost("create doctor")]
        public async Task<IActionResult> AddDoctor(DoctorEntity updateDto)
        {
            try
            {
                return Ok(await _doctor.AddDoctor(updateDto));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------

        [HttpGet("GetDoctorBySpecialization")]
        //[Authorize]
        public async Task<IActionResult> GetDoctorsBySpecialization(string specialization)
        {
            try
            {
                var values = await _doctor.GetDoctorsBySpecialization(specialization);
                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete doctor")]
        public async Task<IActionResult> DeleteDoctorl(string id)
        {
            try
            {
                return Ok(await _doctor.DeleteDoctor(id));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------

        [HttpPut("UpdateSchedule")]
        public async Task<IActionResult> UpdateSchedule(string doctorid,string newSchedule)
        {
            try
            {
                return Ok(await _doctor.UpdateSchedule(doctorid,newSchedule));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------

    }
}