namespace AppointmentManagementService.CustomExceptions
{
    public class DoctorNotFoundException:Exception
    {
        public DoctorNotFoundException(string message):base(message) { }
    }
}
