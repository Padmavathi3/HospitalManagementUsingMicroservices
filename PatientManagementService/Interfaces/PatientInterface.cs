using Microsoft.AspNetCore.Http.HttpResults;
using PatientManagementService.Entities;
using System.Reflection;

namespace PatientManagementService.Interfaces
{
    public interface PatientInterface
    {
        //create patient
        public Task<string> CreatePatient(PatientEntity re_var);

        //Get the patient details based on id
        public Task<IEnumerable<PatientEntity>> GetPatientById(string id);

    }
}