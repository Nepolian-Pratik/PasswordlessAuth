namespace PasswordlessAuth.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PublicKey { get; set; }

    public string LastAnswer { get; set; }

    public string SecurityQuestion { get; set; }

    public string SecQuestionAnswer { get; set; }
}