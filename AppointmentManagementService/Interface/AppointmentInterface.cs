using AppointmentManagementService.Entities;

namespace AppointmentManagementService.Interface
{
    public interface AppointmentInterface
    {
        public Task<string> AddAppointment(AppointmentEntity ref_var);
        public Task<string> DeleteAppointment(int appointmentId, string doctorId);
        public Task<IEnumerable<AppointmentEntity>> GetAppointmentsByDoctor(string doctorId);
        public Task<string> UpdateAppointmentDate(int appointmentId, DateTime newDate);

    }
}
