namespace PatientManagementService.CustomExceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException(string message) : base(message) { }
       
    }
}
