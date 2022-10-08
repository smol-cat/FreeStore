
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

        if (Regexes.InvalidName.IsMatch(regForm.Name) || Regexes.InvalidName.IsMatch(regForm.LastName) ||
            string.IsNullOrEmpty(regForm.Name) || string.IsNullOrEmpty(regForm.LastName))
        {
            return NotAcceptable(new ResponseModel("Name or last name is empty or contain invalid characters"));
        }

        if (regForm.Password.Length < 8)
        {
            return NotAcceptable(new ResponseModel("Password should contain at least 8 characters"));
        }

        if (!Db.SelectAndDeserialize($"SELECT email FROM account WHERE email = @email", new() { ["email"] = regForm.Email }, out var emails))
        {
            return DatabaseError();
        }

        if (emails.Any())
        {
            return NotAcceptable(new ResponseModel("Account with this email already exists"));
        }

        string hash = Utils.SHA256String(regForm.Password);

        Dictionary<string, object> parameters = new()
        {
            ["name"] = regForm.Name,
            ["last_name"] = regForm.LastName,
            ["email"] = regForm.Email,
            ["password"] = hash,
            ["date_created"] = DateTime.Now.ToString(Globals.MySqlDateFormat)
        };

        if (!Db.Execute("INSERT INTO account (name, last_name, email, password, date_created) VALUES (@name, @last_name, @email, @password, @date_created)", parameters))
        {
            return DatabaseError("Failed to create an account");
        }

        return Created($"/api/v1/account/{Db.LastInsertedId}", new ResponseModel());
    }



    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!Db.SelectAndDeserialize($"SELECT * FROM account WHERE email = @email AND is_blocked = 0", new() { ["email"] = loginModel.Email }, out var accounts))
        {
            return DatabaseError("Error while searching for account");
        }

        if (!accounts.Any())
        {
            return NotAcceptable(new ResponseModel("Account associated with this email is not found"));
        }

        var account = accounts.First();

        string passwordHash = (string)account["password"];
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

        return Ok(new ResponseModel("You've logged in"));
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
        if (!Db.SelectAndDeserialize<PersonalAccountModel>($"SELECT * FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }, out var account))
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
        if (!Db.SelectAndDeserialize<PersonalAccountModel>($"SELECT * FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }, out var account))
        {
            return DatabaseError();
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account was not found"));
        }

        if (!Db.Execute($"UPDATE account SET is_blocked = true WHERE id = {id} AND is_blocked = 0", new() { ["id"] = id }))
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

        return DeleteAccount(((PersonalAccountModel)((OkObjectResult)loginResult).Value).Id);
    }

    [HttpGet]
    public IActionResult GetAccountList()
    {
        if (!Db.SelectAndDeserialize<AccountModel>("SELECT id, name, last_name, date_created FROM account WHERE is_blocked = 0", new(), out List<AccountModel> accounts))
        {
            return DatabaseError("Error getting list of accounts");
        }

        return Ok(accounts);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateAccount(int id, ProfileModel profileModel)
    {
        if (!Db.SelectAndDeserialize($"SELECT id FROM account WHERE id = {id} AND is_blocked = 0", new() { ["id"] = id }, out var account))
        {
            return DatabaseError("Error occured while searching for account");
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account was not found"));
        }

        Dictionary<string, object> parameters = new()
        {
            ["name"] = profileModel.Name,
            ["last_name"] = profileModel.LastName,
            ["phone_number"] = profileModel.PhoneNumber,
            ["id"] = id
        };

        if (!Db.Execute($"UPDATE account SET name = @name, last_name = @last_name, phone_number = @phone_number WHERE id = @id", parameters))
        {
            return DatabaseError("Error updating account");
        }

        return Ok(new ResponseModel("Account has been updated"));
    }

}