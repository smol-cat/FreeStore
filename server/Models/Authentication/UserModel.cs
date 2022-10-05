
namespace server.Models.Authentication;

public class UserModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int Level { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime Date_created { get; set; }

}