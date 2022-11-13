
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Database;
using server.Models;
using server.Models.Account;
using server.Models.Authentication;
using server.Utilities;

namespace server.Controllers;

[ApiController]
[Route("/api/v1/tokens")]
public class TokenController : MainController
{
    private readonly IJwtTokenService _jwtTokenService;
    private TokenCache _tokenCache;
    public TokenController(DbConnection dbConnection, IJwtTokenService jwtTokenService, TokenCache tokenCache) : base(dbConnection)
    {
        _tokenCache = tokenCache;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        if (!Db.SelectAndDeserialize<PersonalAccountModel>($"SELECT * FROM account WHERE email = @email AND is_blocked = 0", new() { ["email"] = loginModel.Email }, out var accounts))
        {
            return DatabaseError("Error while searching for account");
        }

        if (!accounts.Any())
        {
            return BadRequest(new ResponseModel("Account associated with this email is not found"));
        }

        PersonalAccountModel account = accounts.First();

        string passwordHash = account.Password;
        string reqPasswordHash = Utils.SHA256String(loginModel.Password);

        if (!passwordHash.Equals(reqPasswordHash))
        {
            return BadRequest(new ResponseModel("Password is incorrect"));
        }

        string token = _jwtTokenService.CreateAccessToken(account.Id, account.Level.Value);
        return Created("", new JwtAccessToken(token));
    }

    [HttpDelete]
    [Authorize]
    public IActionResult Logout()
    {
        if (!_tokenCache.BlockToken(Request.Headers.Authorization.First().Replace("Bearer ", ""), Utils.UnixTimeStampToDateTime(long.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Exp)))))
        {
            return ServerError("Failed to logout");
        }

        return NoContent();
    }
}