namespace tests.utils;

public record CredentialsModel(string Name, string LastName, string Email, string Password)
{
    public long Id;
}
