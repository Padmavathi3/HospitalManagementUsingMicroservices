namespace UserManagementService.CustomExceptions
{
    public class InvalidPasswordException:Exception
    {
        public InvalidPasswordException() { }
        public InvalidPasswordException(string message):base(message) { }
    }
}
