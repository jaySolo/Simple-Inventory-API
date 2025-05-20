using Microsoft.AspNetCore.Identity;



namespace jsolo.simpleinventory.impl.identity;


public class UserPermission : IdentityPermission<int>
{
    #region properties
    public virtual string Route { get; set; } = "";

    public virtual List<string> AllowedRequests { get; set; } = new List<string>();

    public virtual string CreatedBy { get; set; } = "";

    public virtual DateTime CreatedOn { get; set; } = DateTime.Now;

    public virtual string? LastModifiedBy { get; set; }

    public virtual DateTime? LastModifiedOn { get; set; }

    public new virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    #endregion


    #region constructor methods
    protected UserPermission() { }

    public UserPermission(
        int id, string name, string route, IEnumerable<string> acceptedMethods,
        string description = "", DateTime? createdOn = null, string? creator = null
    )
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;

        this.Route = route;

        foreach (var method in acceptedMethods)
        {

            this.AllowedRequests.Add(method);
        }

        this.CreatedOn = createdOn ?? DateTime.Now;
        this.CreatedBy = creator ?? string.Empty;
    }
    #endregion
}
