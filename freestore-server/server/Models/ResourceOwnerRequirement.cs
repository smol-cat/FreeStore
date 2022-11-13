using Microsoft.AspNetCore.Authorization;

namespace server.Models;

public record ResourceOwnerRequirement() : IAuthorizationRequirement;