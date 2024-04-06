using System.Text.Json.Serialization;

namespace AppointmentManagementService.Entities
{
    public class AppointmentEntity
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string DoctorID { get; set; }
        public string PatientID { get; set; }
    }
}
