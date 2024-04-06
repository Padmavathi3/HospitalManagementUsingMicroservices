using Dapper;
using PatientManagementService.Context;
using PatientManagementService.CustomExceptions;
using PatientManagementService.Entities;
using PatientManagementService.Interfaces;
using System.Data;

namespace PatientManagementService.Services
{
    public class PatientService : PatientInterface
    {
        private readonly DapperContext _context;
        public PatientService(DapperContext context)
        {
            _context = context;
        }

        public async Task<string> CreatePatient(PatientEntity re_var)
        {
            var query = "insert into Patients(PatientId,MedicalHistory,Insurance,Gender,DOB) values(@PatientId,@MedicalHistory,@Insurance,@Gender,@DOB)";

            var parameters = new DynamicParameters();
            parameters.Add("@PatientId", re_var.PatientID, DbType.String);
            parameters.Add("@MedicalHistory", re_var.MedicalHistory, DbType.String);
            parameters.Add("@Insurance", re_var.Insurance, DbType.String);
            parameters.Add("@Gender", re_var.Gender, DbType.String);
            parameters.Add("@DOB", re_var.DOB, DbType.Date);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
            return "Patient details are created successfully";
        }

        public async Task<IEnumerable<PatientEntity>> GetPatientById(string userid)
        {
            var query = "SELECT * FROM Patients where PatientId=@UserId";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<PatientEntity>(query, new { UserId = userid });

                if (users != null && users.Any())
                {
                    return users.ToList();
                }
                else
                {
                    throw new UserNotFoundException($"User not found with ID: {userid}");
                }
            }
        }
        //-------------------------------------------------------------------------------------

    }
}
