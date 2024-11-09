using teeze.Services;

public class SignInModel
{
    private readonly IUserServices _userService;

    public SignInModel(IUserServices userService)
    {
        _userService = userService;
    }

    public string Authenticate(string email, string password)
    {
        var user = _userService.GetUsersByEmail(email);

        if (user == null)
        {
            return "User does not exist";
        }

        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return "Sign in successful";
        }

        return "Invalid credentials";
    }
}
