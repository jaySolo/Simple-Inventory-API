using System.ComponentModel.DataAnnotations;


namespace jsolo.simpleinventory.sys.models;


public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public required string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public required string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new and confirmation passwords do not match.")]
    public required string ConfirmPassword { get; set; }
}


public class ChangeUserPasswordViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public required string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new and confirmation passwords do not match.")]
    public required string ConfirmPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Administrator's password")]
    public required string AdminPassword { get; set; }
}


public class LoginViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}


public class UserIdentityStatusViewModel
{
    public int? Id { get; protected set; } = null;

    public string FirstName { get; protected set; }

    public string LastName { get; protected set; }

    public string Username { get; protected set; }

    public string EmailAddress { get; protected set; }

    public bool IsEmailConfirmed { get; protected set; }

    public string PhoneNumber { get; protected set; }

    public bool IsPhoneConfirmed { get; protected set; }

    public bool IsLocked { get; protected set; }

    public string Position { get; protected set; }

    public DateTime? Birthday { get; protected set; }

    public DateTime CreatedOn { get; protected set; }

    public DateTime? LastUpdatedOn { get; protected set; }

    public virtual string Token { get; protected set; }

    public virtual string SessionId { get; protected set; }

    public bool Authenticated { get; protected set; }

    public bool IsAdministrator { get; protected set; } = false;

    public List<string> Roles { get; protected set; } = [];

    public List<PermissionViewModel> Permissions { get; protected set; } = [];

    public UserIdentityStatusViewModel(
        int? id = null,
        string firstNames = "", string surname = "",
        string userName = "",
        string email = "", bool isEmailConfirmed = false,
        string phone = "", bool isPhoneConfirmed = false,
        bool islocked = false,
        string position = "",
#nullable enable
        DateTime? birthday = null,
        DateTime? createdOn = null,
        DateTime? lastModifiedOn = null,
        string? userToken = null,
        string? userSessionId = null,
        bool isUserAuthenticated = false,
        string[]? userRoles = null
#nullable disable
    )
    {
        Id = id;
        FirstName = firstNames;
        LastName = surname;
        Username = userName;
        EmailAddress = email;
        IsEmailConfirmed = isEmailConfirmed;
        PhoneNumber = phone;
        IsPhoneConfirmed = isPhoneConfirmed;
        IsLocked = islocked;
        Position = position;
        Birthday = birthday;
        CreatedOn = createdOn.GetValueOrDefault();
        LastUpdatedOn = lastModifiedOn;
        Token = userToken;
        SessionId = userSessionId;
        Authenticated = isUserAuthenticated;

        if (userRoles != null) { Roles.AddRange(userRoles); }
    }


    public UserIdentityStatusViewModel SetAdminStatus(bool isAdmin)
    {
        IsAdministrator = isAdmin;

        return this;
    }


    public UserIdentityStatusViewModel DefinePermissions(params PermissionViewModel[] permissions)
    {
        if (permissions.Length > 0)
        {
            foreach (var permission in permissions)
            {
                this.Permissions.Add(permission);
            }
        }

        return this;
    }
}


public class UserProfileViewModel
{
    public int Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string EmailAddress { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public string PhoneNumber { get; set; }

    public bool IsPhoneConfirmed { get; set; }

    public DateTime? Birthday { get; set; }

    public bool CanBeLockedOut { get; set; }

    public bool IsLocked { get; set; }

    public string Username { get; set; }

    public string Position { get; set; }

    public List<string> UserRoles { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? LastUpdatedOn { get; set; }
}


public class AddEditUserViewModel : UserProfileViewModel
{
    public string Password { get; set; }

    public string AdminPassword { get; set; }
}
