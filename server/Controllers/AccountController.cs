
using Microsoft.AspNetCore.Mvc;
using server.Models.Authentication;
using server.Models.Account;
using Microsoft.AspNetCore.Authorization;
using server.Database;

namespace server.Controllers;

[ApiController]
[Route("api/v1/accounts")]
public class AccountController : MainController
{
    public AccountController(DbConnection db) : base(db)
    {
    }

    [HttpPost]
    public IActionResult Register(RegistrationModel regForm)
    {
        if (!Regexes.Email.IsMatch(regForm.Email))
        {
            return BadRequest(new ResponseModel("Email is invalid"));
        }

        if (Regexes.InvalidName.IsMatch(regForm.Name) || Regexes.InvalidName.IsMatch(regForm.LastName) ||
            string.IsNullOrEmpty(regForm.Name) || string.IsNullOrEmpty(regForm.LastName))
        {
            return BadRequest(new ResponseModel("Name or last name is empty or contain invalid characters"));
        }

        if (regForm.Password.Length < 8)
        {
            return BadRequest(new ResponseModel("Password should contain at least 8 characters"));
        }

        if (!Db.SelectAndDeserialize($"SELECT email FROM account WHERE email = @email", new() { ["email"] = regForm.Email }, out var emails))
        {
            return DatabaseError();
        }

        if (emails.Any())
        {
            return BadRequest(new ResponseModel("Account with this email already exists"));
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

        return Created($"/api/v1/accounts/{Db.LastInsertedId}", new ResponseModel("Account has been created"));
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = Role.Everyone)]
    public IActionResult GetAccount(int id)
    {
        List<AccountModel> accounts = null;
        List<PersonalAccountModel> personalAccounts = null;
        
        bool success = (id.ToString() != UserId && !User.IsInRole(Role.Admin)) ? 
            Db.SelectAndDeserialize<AccountModel>($"SELECT * FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }, out accounts) :
            Db.SelectAndDeserialize<PersonalAccountModel>($"SELECT * FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }, out personalAccounts);

        if (!success)
        {
            return DatabaseError();
        }

        AccountModel account = personalAccounts?.FirstOrDefault() ?? accounts?.FirstOrDefault();

        if (account == null)
        {
            return NotFound(new ResponseModel("Account is not found"));
        }

        return Ok(account);
    }

    [HttpGet]
    [Route("own")]
    [Authorize(Roles = Role.Everyone)]
    public IActionResult GetOwnAccount() => GetAccount(int.Parse(UserId));

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = Role.Admin)]
    public IActionResult BlockAccount(int id)
    {
        if (!Db.SelectAndDeserialize<PersonalAccountModel>($"SELECT * FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }, out var account))
        {
            return DatabaseError();
        }

        if (!account.Any())
        {
            return NotFound(new ResponseModel("Account was not found"));
        }

        if (!Db.Execute($"UPDATE account SET is_blocked = true WHERE id = @id AND is_blocked = 0", new() { ["id"] = id }))
        {
            return DatabaseError("Error occured while deleting account");
        }

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = Role.Everyone)]
    public IActionResult BlockOwnAccount() => BlockAccount(int.Parse(UserId));

    [HttpGet]
    [Authorize(Roles = Role.Admin)]
    public IActionResult GetAccountList()
    {
        if (!Db.SelectAndDeserialize<AccountModel>("SELECT id, name, last_name, date_created, level FROM account WHERE is_blocked = 0", new(), out List<AccountModel> accounts))
        {
            return DatabaseError("Error getting list of accounts");
        }

        return Ok(accounts);
    }

    [HttpPut]
    [Authorize(Roles = Role.Everyone)]
    public IActionResult UpdateAccount(ProfileModel profileModel)
    {
        if (Regexes.InvalidName.IsMatch(profileModel.Name) || Regexes.InvalidName.IsMatch(profileModel.LastName) ||
            string.IsNullOrEmpty(profileModel.Name) || string.IsNullOrEmpty(profileModel.LastName))
        {
            return BadRequest(new ResponseModel("Name or last name is empty or contain invalid characters"));
        }

        if (!Db.SelectAndDeserialize($"SELECT id FROM account WHERE id = @id AND is_blocked = 0", new() { ["id"] = UserId }, out var account))
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
            ["id"] = UserId
        };

        if (!Db.Execute($"UPDATE account SET name = @name, last_name = @last_name, phone_number = @phone_number WHERE id = @id", parameters))
        {
            return DatabaseError("Error updating account");
        }

        return Ok(new ResponseModel("Account has been updated"));
    }

}