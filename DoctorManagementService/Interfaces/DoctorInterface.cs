using DoctorManagementService.Entities;

namespace DoctorManagementService.Interfaces
{
    public interface DoctorInterface
    { 
        public Task<string> AddDoctor(DoctorEntity re_var);
        public Task<string> DeleteDoctor(string DoctorID);
        public Task<IEnumerable<DoctorEntity>> GetDoctorsBySpecialization(string specialization);
        public Task<string> UpdateSchedule(string doctorId, string newSchedule);
    }
}
