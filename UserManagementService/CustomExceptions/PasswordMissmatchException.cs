namespace UserManagementService.CustomExceptions
{
    public class PasswordMissmatchException:Exception
    {
        
        public PasswordMissmatchException(string message):base(message) { }
    }
}
