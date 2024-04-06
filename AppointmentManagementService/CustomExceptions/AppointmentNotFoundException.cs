namespace AppointmentManagementService.CustomExceptions
{
    public class AppointmentNotFoundException:Exception
    {
        public AppointmentNotFoundException(string message):base(message) { }
    }
}
