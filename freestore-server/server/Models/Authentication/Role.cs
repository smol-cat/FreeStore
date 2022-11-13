
namespace server.Models.Authentication;
public class Role
{
    public const string User = nameof(User);
    public const string Admin = nameof(Admin);

    public const string Everyone = User + "," + Admin;

    public static string FromId(int id)
    {
        switch (id)
        {
            case 0:
                return User;
            case 1:
                return Admin;
            default:
                return null;
        }
    }
}