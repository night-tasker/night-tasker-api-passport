﻿using Mapster;
using NightTasker.Passport.Application.Features.Users.Models;
using NightTasker.Passport.Domain.Entities.User;

namespace NightTasker.Passport.Application.Profiles;

public class UserProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateUserDto, User>()
            .ConstructUsing(dto => new User(dto.UserName));
    }
}