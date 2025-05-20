using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.core.common;


namespace jsolo.simpleinventory.impl.identity
{
    public class User : IdentityUser<int>
    {
        #region  properties
        public virtual string Surname { get; protected set; }

        public virtual string FirstName { get; protected set; }

        public virtual DateTime? Birthdate { get; protected set; }

        public virtual string Position { get; protected set; }

        public virtual ICollection<UserRole> Roles { get; protected set; } = new List<UserRole>();

        public virtual ICollection<IdentityUserClaim<int>> Claims
        {
            get; protected set;
        } = new List<IdentityUserClaim<int>>();

        public virtual ICollection<UserLogin> Logins { get; protected set; } = new List<UserLogin>();


        /// <summary>The creator of the <see cref="User"/>. Reserved for auditing purposes.</summary>
        public virtual string CreatorId { get; protected set; }

        /// <summary>
        /// The timestamp the <see cref="User"/> was created. Reserved for auditing purposes.
        /// </summary>
        public virtual DateTime CreatedOn { get; protected set; }

        /// <summary>
        /// The last updater/modifier of the <see cref="User"/>. Reserved for auditing purposes.
        /// </summary>
        public virtual string LastModifierId { get; protected set; }

        /// <summary>
        /// The timestamp the <see cref="User"/> was last updated/modified. Reserved for auditing
        /// purposes.
        /// </summary>
        public virtual DateTime? LastModifiedOn { get; protected set; }


        public virtual bool HasAvatar => !string.IsNullOrWhiteSpace(FileName) &
            !string.IsNullOrWhiteSpace(FileExtension) & !string.IsNullOrWhiteSpace(FileContentType) &
            FileUploadDate != null;

        public virtual string FileName { get; protected set; }

        public virtual string FileExtension { get; protected set; }

        public virtual string FileContentType { get; protected set; }

        public virtual DateTime? FileUploadDate { get; protected set; }
        #endregion

        protected User() { }

        public User(
            int id, string surname,
            string firstNames,
            string username,
            string emailaddr,
            DateTime? birthday = null,
            string phoneNum = null,
            DateTime? createdOn = null,
            string creatorId = null,
            DateTime? updatedOn = null,
            string lastUpdaterId = null
        )
        {
            this.Id = id;

            this.Surname = surname;
            this.FirstName = firstNames;
            this.UserName = username;
            this.Email = emailaddr;
            this.PhoneNumber = phoneNum;

            this.CreatedOn = createdOn ?? DateTime.Now;
            this.CreatorId = creatorId;
            this.LastModifiedOn = updatedOn;
            this.LastModifierId = lastUpdaterId;
        }


        public virtual User UpdateName(string surname, string firstNames)
        {
            this.Surname = surname;
            this.FirstName = firstNames;

            return this;
        }


        public virtual User UpdateBirthdate(DateTime? birthday)
        {
            this.Birthdate = birthday;

            return this;
        }


        public virtual User UpdatePosition(string position)
        {
            this.Position = position;

            return this;
        }


        public virtual User SetAvatarDetails(string fileName, string fileExt, string contentType)
        {
            this.FileName = fileName;
            this.FileExtension = fileExt;
            this.FileContentType = contentType;
            this.FileUploadDate = DateTime.Now;

            return this;
        }

        public virtual User UpdatedNowBy(string updaterId)
        {
            this.LastModifiedOn = DateTime.Now;
            this.LastModifierId = updaterId;

            return this;
        }
    }
}