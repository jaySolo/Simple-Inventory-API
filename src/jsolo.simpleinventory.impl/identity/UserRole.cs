using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace jsolo.simpleinventory.impl.identity
{
    public class UserRole: IdentityRole<int>, IIdentityPermissionRole<UserPermission, int>
    {
        #region properties
        public virtual string Description { get; protected set; }

        public virtual ICollection<User> Users { get; protected set; } = new List<User>();

        public virtual ICollection<UserPermission> Permissions { get; protected set; } = new List<UserPermission>();

        public virtual string CreatedBy { get; protected set; }

        public virtual DateTime CreatedOn { get; protected set; }

        public virtual string LastModifiedBy { get; protected set; }

        public virtual DateTime? LastModifiedOn { get; protected set; }
        #endregion


        #region constructor methods
        protected UserRole() { }

        public UserRole(
            int id, string name,
            string description,
            DateTime? createdOn = null,
            string creator = null
        ) {

            this.Id = id;

            this.Name = name;
            this.Description = description;

            this.CreatedOn = createdOn ?? DateTime.Now;
            this.CreatedBy = !string.IsNullOrWhiteSpace(creator) ? creator : null;
        }
        #endregion

        
        #region instance methods
        public virtual UserRole UpdateName(string name, string modifier) 
        {
            if(!string.IsNullOrWhiteSpace(name)) { 
                this.Name = name;
                this.LastModifiedOn = DateTime.Now;
                this.LastModifiedBy = modifier;
            }

            return this;
        }


        public virtual UserRole UpdateDescription(string description, string modifier)
        {
            this.Description = description;

            this.LastModifiedOn = DateTime.Now;
            this.LastModifiedBy = modifier;

            return this;
        }


        public virtual UserRole AddPermission(UserPermission permission) 
        {
            if (permission != null) { this.Permissions.Add(permission); }

            return this;
        }


        public virtual UserRole RemovePermission(UserPermission permission) 
        {
            if (permission != null) { this.Permissions.Remove(permission); }

            return this;
        }

        

        /// <summary>
        /// Updates the last updater/modifier of the <see cref="UserRole"/> and the timestamp the
        /// <see cref="UserRole"/> was last updated/modified.
        /// </summary>
        /// <param name="modifier">The new last updater/modifier of the <see cref="UserRole"/>.</param>
        /// <param name="modifiedOn">
        /// The new timestamp the <see cref="UserRole"/> was last updated/modified.
        /// </param>
        public virtual UserRole SetLastModifierAsAt(string modifier, DateTime modifiedOn)
        {
            this.LastModifiedBy = modifier;
            this.LastModifiedOn = modifiedOn;

            return this;
        }
        #endregion
    }
}
