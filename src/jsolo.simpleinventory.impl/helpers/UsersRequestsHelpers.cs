using System;
using System.Linq;
using jsolo.simpleinventory.impl.identity;
using jsolo.simpleinventory.sys.models;

namespace jsolo.simpleinventory.impl.helpers
{
    static class UsersRequestsHelpers
    {
        public static UserProfileViewModel MapToProfileViewModel(User usr) => new UserProfileViewModel
        {
            Id = usr.Id,
            FirstName = usr.FirstName,
            LastName = usr.Surname,
            EmailAddress = usr.Email,
            IsEmailConfirmed = usr.EmailConfirmed,
            PhoneNumber = usr.PhoneNumber,
            Birthday = usr.Birthdate,
            Position = usr.Position,
            IsPhoneConfirmed = usr.PhoneNumberConfirmed,
            CanBeLockedOut = usr.LockoutEnabled,
            IsLocked = usr.LockoutEnabled && DateTimeOffset.Now <= usr.LockoutEnd,
            Username = usr.UserName,
            UserRoles = usr.Roles.Select(r => r.Name).ToList(),
            CreatedOn = usr.CreatedOn,
            LastUpdatedOn = usr.LastModifiedOn
        };
    }
}