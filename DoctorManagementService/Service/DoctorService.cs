using Dapper;
using DoctorManagementService.Context;
using DoctorManagementService.CustomExceptions;
using DoctorManagementService.Entities;
using DoctorManagementService.Interfaces;
using System.Data;

namespace DoctorManagementService.Service
{
    public class DoctorService : DoctorInterface
    {
        private readonly DapperContext _context;
        public DoctorService(DapperContext context)
        {
            _context = context;
        }
        public async Task<string> AddDoctor(DoctorEntity re_var)
        {
            var query = "insert into Doctors(DoctorID,DoctorName,Specialization,Qualifications,Schedule) values(@DoctorID,@DoctorName,@Specialization,@Qualifications,@Schedule)";

            var parameters = new DynamicParameters();
            parameters.Add("@DoctorId", re_var.DoctorID, DbType.String);
            parameters.Add("@DoctorName", re_var.DoctorName, DbType.String);
            parameters.Add("@Specialization", re_var.Specialization, DbType.String);
            parameters.Add("@Qualifications", re_var.Qualifications, DbType.String);
            parameters.Add("@Schedule", re_var.Schedule, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
            return "Doctor details inserted successfully";
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<string> DeleteDoctor(string doctorID)
        {
            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(DoctorId) FROM doctors WHERE   DoctorID= @DoctorID";

            using (var connection = _context.CreateConnection())
            {
                int idCount = await connection.ExecuteScalarAsync<int>(countQuery, new { DoctorID = doctorID });

                if (idCount > 0)
                {
                    var deleteQuery = "DELETE FROM doctors WHERE DoctorID= @DoctorID";
                    rowsAffected = await connection.ExecuteAsync(deleteQuery, new { DoctorID = doctorID });

                    if (rowsAffected > 0)
                    {
                        return ($"{rowsAffected} users are deleted");
                    }
                }
                else
                {
                    throw new DoctorNotFoundException($"No Doctor found with id: {doctorID}");
                }
            }
            return $"{rowsAffected} user(s) are deleted";
        }

        public async Task<IEnumerable<DoctorEntity>> GetDoctorsBySpecialization(string specialization)
        {
            var query = "SELECT * FROM doctors where Specialization=@Specialization";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<DoctorEntity>(query, new { Specialization = specialization });

                if (users != null && users.Any())
                {
                    return users.ToList();
                }
                else
                {
                    throw new DoctorNotFoundException($"No Doctor found with this speciliazation: {specialization}");
                }
            }
        }

        public async Task<string> UpdateSchedule(string doctorId, string newSchedule)
        {

            int rowsAffected = 0;
            var countQuery = "SELECT COUNT(DoctorId) FROM doctors WHERE   DoctorID= @DoctorID";

            using (var connection = _context.CreateConnection())
            {
                int idCount = await connection.ExecuteScalarAsync<int>(countQuery, new { DoctorID = doctorId });

                if (idCount > 0)
                {
                    var query = "UPDATE doctors SET Schedule = @newSchedule WHERE DoctorID= @DoctorID";
                    var parameters = new DynamicParameters();
                    parameters.Add("DoctorId",doctorId,DbType.String);
                    parameters.Add("@newSchedule", newSchedule, DbType.String);
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return ($"Schedule is updated successfully");
                    }
                    return ($"no one row affected");
                }
                else
                {
                    throw new DoctorNotFoundException($"No Doctor found with this id: {doctorId}");
                }
            }
        }
    }
}
