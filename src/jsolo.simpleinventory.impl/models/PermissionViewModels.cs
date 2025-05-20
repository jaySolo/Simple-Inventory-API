using System;
using System.Collections.Generic;

namespace jsolo.simpleinventory.sys.models
{
    public class PermissionViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Route { get; set; }

        public List<string> AcceptedMethods { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }
        
        public List<string> Roles { get; set; }
    }


    public class AddEditPermissionViewModel : PermissionViewModel
    {
        public string AdminPassword { get; set; }
    }
}
