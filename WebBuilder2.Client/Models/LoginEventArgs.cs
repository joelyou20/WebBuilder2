namespace WebBuilder2.Client.Models
{
    // Custom event arguments for user login
    public class LoginEventArgs : EventArgs
    {
        // Property to hold the username of the logged-in user
        public string UserName { get; set; } = string.Empty;

        // Property to hold the token issued during login
        public string Token { get; set; } = string.Empty;
    }
}
