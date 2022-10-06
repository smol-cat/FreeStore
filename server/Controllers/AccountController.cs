
using Microsoft.AspNetCore.Mvc;
using server.Models.Authentication;
using server.Models.Account;

namespace server.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController : MainController
{
    [HttpPost]
    [Route("register")]
    public IActionResult Register(RegistrationModel regForm)
    {
        if (!Regexes.Email.IsMatch(regForm.Email))
        {
            return NotAcceptable(new ResponseModel("Email is invalid"));
        }

        if (Regexes.InvalidName.IsMatch(regForm.Name) || Regexes.InvalidName.IsMatch(regForm.LastName))
        {
            return NotAcceptable(new ResponseModel("Name or last name contain invalid characters"));
        }

        if (regForm.Password.Length < 8)
        {
            return NotAcceptable(new ResponseModel("Password should contain at least 8 characters"));
        }

        if (!Db.SelectAndDeserialize($"SELECT email FROM account WHERE email = \"{regForm.Email}\"", out var emails))
        {
            return DatabaseError();
        }

        if (emails.Any())
        {
            return NotAcceptable(new ResponseModel("Account with this email already exists"));
        }

        string hash = Utils.SHA256String(regForm.Password);

        if (!Db.Execute("INSERT INTO account (name, last_name, email, password, date_created)" +
        $"VALUES (\"{regForm.Name}\", \"{regForm.LastName}\", \"{regForm.Email}\", \"{hash}\", \"{DateTime.Now.ToString(Globals.MySqlDateFormat)}\")"))
        {
            return DatabaseError();
        }

        return Created($"/api/v1/account/{Db.LastInsertedId}", new ResponseModel());
    }



    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!Db.SelectAndDeserialize<AccountModel>($"SELECT * FROM account WHERE email = \"{loginModel.Email}\" AND is_blocked = 0", out var accounts))
        {
            return DatabaseError("Error while searching for account");
        }

        if (!accounts.Any())
        {
            return NotAcceptable(new ResponseModel("Account associated with this email is not found"));
        }

        var account = accounts.First();

        string passwordHash = account.Password;
        string reqPasswordHash = Utils.SHA256String(loginModel.Password);

        if (!passwordHash.Equals(reqPasswordHash))
        {
            return NotAcceptable(new ResponseModel("Password is incorrect"));
        }

        // var claims = new List<Claim>();
        // claims.Add(new Claim("email", account.Email));
        // claims.Add(new Claim(ClaimTypes.Email, account.Email));
        // claims.Add(new Claim("level", account.Level.ToString()));
        // claims.Add(new Claim(ClaimTypes.PrimaryGroupSid, account.Level.ToString()));
        // ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        // ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        // await HttpContext.SignInAsync(claimsPrincipal);
        account.Password = null;

        return Ok(account);
    }

    // [HttpGet]
    // [Route("logout")]
    // public IActionResult Logout()
    // {
    //     SignOut();
    //     return Ok(new ResponseModel("You've been signed out"));
    // }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetAccount(int id)
    {
        if (!Db.SelectAndDeserialize<AccountModel>($"SELECT * FROM account WHERE id = {id} AND is_blocked = 0", out var account))
        {
            return DatabaseError();
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account is not found"));
        }

        return Ok(account.First());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        if (!Db.SelectAndDeserialize($"SELECT id FROM account WHERE id = {id} AND is_blocked = 0", out var account))
        {
            return DatabaseError("Error occured while searching for account");
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account was not found"));
        }

        if (!Db.Execute($"UPDATE account SET is_blocked = true WHERE id = {id} AND is_blocked = 0"))
        {
            return DatabaseError("Error occured while deleting account");
        }

        return Ok(new ResponseModel("Account has been successfully deleted"));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteOwnAccount(LoginModel loginModel)
    {
        IActionResult loginResult = await Login(loginModel);
        if (loginResult is not OkObjectResult)
        {
            return loginResult;
        }

        return DeleteAccount(((AccountModel)((OkObjectResult)loginResult).Value).Id);
    }

    [HttpGet]
    public IActionResult GetAccountList()
    {
        if (!Db.SelectAndDeserialize<AccountModel>("SELECT id, name, last_name, is_blocked, level FROM account WHERE is_blocked = 0", out List<AccountModel> accounts))
        {
            return DatabaseError("Error getting list of accounts");
        }

        return Ok(accounts);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateAccount(int id, ProfileModel profileModel)
    {
        if (!Db.SelectAndDeserialize($"SELECT id FROM account WHERE id = {id} AND is_blocked = 0", out var account))
        {
            return DatabaseError("Error occured while searching for account");
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account was not found"));
        }

        if (!Db.Execute($"UPDATE account SET name = \"{profileModel.Name}\", last_name = \"{profileModel.LastName}\", phone_number = \"{profileModel.PhoneNumber}\" WHERE id = {id}"))
        {
            return DatabaseError("Error updating account");
        }

        return Ok(new ResponseModel("Account has been updated"));
    }

}