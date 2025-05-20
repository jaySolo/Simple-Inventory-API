namespace jsolo.simpleinventory.sys.models;



public class RoleViewModel
{
    public required string Name { get; set; }

    public string Description { get; set; }

    public bool HasAdminRights { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int UserCount { get; set; }

    public List<string> Permissions { get; set; } = [];
}



public class AddEditRoleViewModel : RoleViewModel
{
    public required string AdminPassword { get; set; }
}
