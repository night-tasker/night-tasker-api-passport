﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NightTasker.Passport.Domain.Entities.User;

namespace NightTasker.Passport.Infrastructure.Identity.Identity.Managers;

internal class AppSignInManager : SignInManager<User>
{
    public AppSignInManager(
        UserManager<User> userManager, 
        IHttpContextAccessor contextAccessor, 
        IUserClaimsPrincipalFactory<User> claimsFactory, 
        IOptions<IdentityOptions> optionsAccessor, 
        ILogger<SignInManager<User>> logger, 
        IAuthenticationSchemeProvider schemes, 
        IUserConfirmation<User> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
    }
}