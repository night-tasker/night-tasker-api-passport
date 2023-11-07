﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NightTasker.Passport.Domain.Entities.Common;
using NightTasker.Passport.Domain.Entities.User;
using NightTasker.Passport.Application.Common.Extensions;

namespace NightTasker.Passport.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        base.SavingChanges += OnSavingChanges;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        UseSnakeCaseNamingConventions(builder);
    }

    private void UseSnakeCaseNamingConventions(ModelBuilder builder)
    {
        foreach(var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ConvertCamelToSnakeCase());
            
            foreach(var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ConvertCamelToSnakeCase());
            }

            foreach(var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ConvertCamelToSnakeCase());
            }

            foreach(var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName()?.ConvertCamelToSnakeCase());
            }

            foreach(var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ConvertCamelToSnakeCase());
            }
        }
    }
    
    private void OnSavingChanges(object? sender, SavingChangesEventArgs args)
    {
        ConfigureEntityDates();
    }

    private void ConfigureEntityDates()
    {
        var updatedEntities = ChangeTracker.Entries()
            .Where(x => x.Entity is IUpdatedDateTimeOffset && x.State == EntityState.Modified)
            .Select(x => x.Entity as IUpdatedDateTimeOffset);

        foreach (var entity in updatedEntities)
        {
            if (entity is not null)
            {
                entity.UpdatedDateTimeOffset = DateTimeOffset.Now;
            }
        }

        var createdEntities = ChangeTracker.Entries()
            .Where(x => x.Entity is ICreatedDateTimeOffset && x.State == EntityState.Added)
            .Select(x => x.Entity as ICreatedDateTimeOffset);

        foreach (var entity in createdEntities)
        {
            if (entity is not null)
            {
                entity.CreatedDateTimeOffset = DateTimeOffset.Now;
            }
        }
    }
}