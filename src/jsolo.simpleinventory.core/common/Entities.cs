using System;
using System.Collections.Generic;
using System.Linq;



namespace jsolo.simpleinventory.core.common
{

    ///<summary>Interface for an entity without an ID.</summary>
    public interface IEntity
    {
#nullable enable
        /// <summary>The creator of the <see cref="IEntity"/>. Reserved for auditing purposes.</summary>
        string? CreatorId { get; }

        /// <summary>
        /// The timestamp the <see cref="IEntity"/> was created. Reserved for auditing purposes.
        /// </summary>
        DateTime? CreatedOn { get; }

        /// <summary>
        /// The last updater/modifier of the <see cref="IEntity"/>. Reserved for auditing purposes.
        /// </summary>
        string? LastModifierId { get; }

        /// <summary>
        /// The timestamp the <see cref="IEntity"/> was last updated/modified. Reserved for auditing
        /// purposes.
        /// </summary>
        DateTime? LastModifiedOn { get; }
#nullable disable
    }



    ///<summary>Interface for a generic entity with an ID.</summary>
    public interface IEntity<TId> : IEntity where TId : IEquatable<TId>
    {
        /// <summary>The unique ID of the <see cref="IEntity"/>.</summary>
        TId Id { get; }
    }



    /// <summary>An entity without an ID. The base class for all entities.</summary>
    public abstract class Entity : IEntity, IEquatable<Entity>
    {
        #region  properties
#nullable enable
        /// <summary>
        /// The id of the creator of the <see cref="Entity"/>. Reserved for auditing purposes.
        /// </summary>
        public virtual string? CreatorId { get; protected set; }

        /// <summary>
        /// The timestamp the <see cref="Entity"/> was created. Reserved for auditing purposes.
        /// </summary>
        public virtual DateTime? CreatedOn { get; protected set; }

        /// <summary>
        /// The last updater/modifier of the <see cref="Entity"/>. Reserved for auditing purposes.
        /// </summary>
        public virtual string? LastModifierId { get; protected set; }

        /// <summary>
        /// The timestamp the <see cref="Entity"/> was last updated/modified. Reserved for auditing
        /// purposes.
        /// </summary>
        public virtual DateTime? LastModifiedOn { get; protected set; }
#nullable disable
        #endregion

        /// <summary>
        /// Get the list of properties used to make an equality comparison in the event that the IDs
        /// are not equal or are unavailable (non-existant). This method is overridden/implemented by 
        /// all inherited classes.
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();


        /// <summary>
        /// Updates the last updater/modifier of the <see cref="Entity"/> and the timestamp the
        /// <see cref="Entity"/> was last updated/modified.
        /// </summary>
        /// <param name="modifier">The new last updater/modifier of the <see cref="Entity"/>.</param>
        /// <param name="modifiedOn">
        /// The new timestamp the <see cref="Entity"/> was last updated/modified.
        /// </param>
        public virtual Entity SetLastModifierAsAt(string modifierId, DateTime? modifiedOn)
        {
            this.LastModifierId = modifierId;
            this.LastModifiedOn = modifiedOn;

            return this;
        }


        #region implementations & overrides
        /// <summary>
        /// Determines whether the specified <see cref="Entity"/> is equal to the current
        /// <see cref="Entity"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="Entity"/> to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="Entity"/> is equal to the current <see cref="Entity"/>;
        /// otherwise, false.
        /// </returns>
        public virtual bool Equals(Entity other) => !(other is null) &&
            GetType() == other.GetType() && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());


        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current
        /// <see cref="Entity"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="object"/> to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="object"/> is equal to the current <see cref="Entity"/>;
        /// otherwise, false.
        /// </returns>
        public override bool Equals(object obj) => !(obj is null) && (
            GetType() == obj.GetType() && GetEqualityComponents().SequenceEqual(
                ((Entity)obj).GetEqualityComponents()));


        /// <summary> Serves as the default hash function. </summary>
        /// <returns> The hash code for the current object. </returns>
        public override int GetHashCode() => base.GetHashCode();
        #endregion



        #region helpers
        /// <summary>
        /// Checks whether or not a specified value is null, an empty string or a string that has
        /// nothing but white spaces and throws an <see cref="ArgumentNullException"/> if the one of
        /// these conditions is met.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="nameOfValue">
        /// The name of the value, used to for identifying the error causing parameter when throwing
        /// the <see cref="ArgumentNullException"/>.
        /// </param>
        protected void ThrowIfNullOrWhitespaceString(string value, string nameOfValue)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(paramName: nameOfValue,
                                                message: "This value cannot be empty!");
            }
        }
        #endregion
    }



    /// <summary>An generic entity with an ID.</summary>
    /// <typeparam name="TId">
    /// The <see cref="Type"/> of the <see cref="Id"/> being stored by this <see cref="Entity{TId}"/>.
    /// </typeparam>
    public abstract class Entity<TId> : Entity, IEntity<TId>, IEquatable<Entity<TId>>
        where TId : IEquatable<TId>
    {
        #region properties
        /// <summary>The unique ID of the <see cref="Entity{TId}"/>.</summary>
        public virtual TId Id { get; protected set; }
        #endregion


        #region implementations & overrides
        /// <summary>
        /// Determines whether the specified <see cref="Entity{TId}"/> is equal to the current
        /// <see cref="Entity{TId}"/>.
        /// </summary>
        /// <param name="other">The other <see cref="Entity{TId}"/> to compare with the current
        /// object.</param>
        /// <returns>
        /// true if the specified <see cref="Entity{TId}"/> is equal to the current
        /// <see cref="Entity{TId}"/>; otherwise, false.
        /// </returns>
        public virtual bool Equals(Entity<TId> other) => !(other is null) &&
            GetType() == other.GetType() && Id.Equals(other.Id) ||
                GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        #endregion
    }
}
