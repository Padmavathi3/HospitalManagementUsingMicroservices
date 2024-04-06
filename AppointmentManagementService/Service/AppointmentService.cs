using AppointmentManagementService.Context;
using AppointmentManagementService.CustomExceptions;
using AppointmentManagementService.Entities;
using AppointmentManagementService.Interface;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AppointmentManagementService.Service
{
    public class AppointmentService : AppointmentInterface
    {
        private readonly DapperContext _context;
        public AppointmentService(DapperContext contex)
        {
            _context = contex;
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        public async Task<string> AddAppointment(AppointmentEntity ref_var)
        {
            var query = "insert into Appointments(AppointmentID,AppointmentDate,DoctorID,PatientID) values(@AppointmentID,@AppointmentDate,@DoctorID,@PatientID)";

            var parameters = new DynamicParameters();
            parameters.Add("@AppointmentID", ref_var.AppointmentID, DbType.Int32);
            parameters.Add("@AppointmentDate", ref_var.AppointmentDate, DbType.Date);
            parameters.Add("@DoctorID", ref_var.DoctorID, DbType.String);
            parameters.Add("@PatientID", ref_var.PatientID, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
            return "appointment details stored successfully";
        }
        //-----------------------------------------------------------------------------------------------------------------------------------

        public async Task<string> DeleteAppointment(int appointmentId, string doctorId)
        {
            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(*) FROM appointments WHERE   DoctorID= @DoctorID and AppointmentID=@AppointmentID";

            using (var connection = _context.CreateConnection())
            {
                int idCount = await connection.ExecuteScalarAsync<int>(countQuery, new { DoctorID = doctorId, AppointmentID= appointmentId });

                if (idCount > 0)
                {
                    var deleteQuery = "DELETE FROM appointments WHERE DoctorID= @DoctorID and AppointmentID=@AppointmentID";
                    rowsAffected = await connection.ExecuteAsync(deleteQuery, new { DoctorID = doctorId, AppointmentID = appointmentId });

                    if (rowsAffected > 0)
                    {
                        return ($"{rowsAffected} users are deleted");
                    }
                }
                else
                {
                    throw new AppointmentNotFoundException($"No Appointment was registered with this Doctorid {doctorId}");
                }
            }
            return $"{rowsAffected} user(s) are deleted";
        }
        //-----------------------------------------------------------------------------------------------------------------------------------
        public async Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDoctor(string doctorId)
        {
            var query = "SELECT * FROM Appointments where  DoctorID= @DoctorID ";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<AppointmentEntity>(query, new { DoctorID = doctorId });

                if (users != null && users.Any())
                {
                    return users.ToList();
                }
                else
                {
                    throw new DoctorNotFoundException($"No Doctor found with this id: {doctorId}");
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------

        public async Task<string> UpdateAppointmentDate(int appointmentId, DateTime newDate)
        {
            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(*) FROM Appointments WHERE AppointmentID=@AppointmentID";

            using (var connection = _context.CreateConnection())
            {
                int idCount = await connection.ExecuteScalarAsync<int>(countQuery, new { AppointmentID = appointmentId });

                if (idCount > 0)
                {
                    var query = "UPDATE appointments SET AppointmentDate = @newDate WHERE AppointmentID=@AppointmentID";
                    var parameters = new DynamicParameters();
                    parameters.Add("AppointmentID", appointmentId, DbType.Int32);
                    parameters.Add("@newDate", newDate, DbType.Date);
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return ($"Appointment Date is updated successfully");
                    }
                    return ($"no one row affected");
                }
                else
                {
                    throw new AppointmentNotFoundException($"No Appointment found with this id: {appointmentId}");
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        

    }
}