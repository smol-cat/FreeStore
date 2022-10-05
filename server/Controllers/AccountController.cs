
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using server.Entities;
using server.Models.Authentication;

namespace server.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController : MainController
{

    [HttpPost]
    [Route("register")]
    public IActionResult Register(ReqRegistrationModel regForm)
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

        if (!DbConnection.Instance.SelectAndDeserialize($"SELECT email FROM account WHERE email = \"{regForm.Email}\"", out var emails))
        {
            return ServerError(ServerErrorType.Database);
        }

        if (emails.Any())
        {
            return NotAcceptable(new ResponseModel("Account with this email already exists"));
        }

        string hash = Utils.SHA256String(regForm.Password);

        if (!DbConnection.Instance.Execute("INSERT INTO account (name, last_name, email, password, date_created)" +
        $"VALUES (\"{regForm.Name}\", \"{regForm.LastName}\", \"{regForm.Email}\", \"{hash}\", \"{DateTime.Now.ToString(Globals.MySqlDateFormat)}\")"))
        {
            return ServerError(ServerErrorType.Database, "Error inserting to accounts");
        }

        return Created($"/api/v1/account/{DbConnection.Instance.LastInsertedId}", new ResponseModel());
    }

    [HttpGet]
    public IActionResult GetAccountList()
    {
        if (!DbConnection.Instance.SelectAndDeserialize<UserModel>("SELECT * FROM account", out List<UserModel> accounts))
        {
            return ServerError(ServerErrorType.Database, "Error getting list of accounts");
        }

        return Ok(accounts);
    }
}