namespace PwdLessAuth.Models;

public class UserSignUpDto
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string PublicKey { get; set; }

    public string SecurityQuestion { get; set; }

    public string SecQuestionAnswer { get; set; }
}