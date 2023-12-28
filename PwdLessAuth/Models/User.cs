namespace PwdLessAuth.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PublicKey { get; set; }

    public string LastAnswer { get; set; }

    public string SecurityQuestion { get; set; }

    public string SecQuestionAnswer { get; set; }

    public static explicit operator User(UserSignUpDto v)
    {
        return new User
        {
            Username = v.Username,
            Email = v.Email,
            PublicKey = v.PublicKey,
            SecurityQuestion = v.SecurityQuestion,
            SecQuestionAnswer = v.SecQuestionAnswer
        };
    }
}