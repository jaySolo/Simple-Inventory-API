using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;



namespace jsolo.simpleinventory.core.common
{
    /// <summary>
    /// A generic, immutable object used to store one or values. The base class for all value objects.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        #region protected instance methods
        /// <summary>
        /// Get the list of properties used to make an equality comparison. This method is 
        /// overridden/implemented by all inherited classes.
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();

        #endregion


        #region overrides & implementations
        /// <summary>
        /// Determines whether the specified <see cref="ValueObject"/> is equal to the current
        /// <see cref="ValueObject"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="ValueObject"/> to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="ValueObject"/> is equal to the current
        /// <see cref="ValueObject"/>; otherwise, false.
        /// </returns>
        public virtual bool Equals(/*[AllowNull]*/ ValueObject other)
        {
            if (other == null) { return false; }
            if (GetType() != other?.GetType()) { return false; }
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }
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



    /// <summary>A generic, immutable object used to store a single value.</summary>
    /// <typeparam name="TValue">
    /// The <see cref="Type"/> of the <see cref="Value"/> being stored by this object.
    /// </typeparam>
    public abstract class ValueObject<TValue> : ValueObject, IEquatable<ValueObject<TValue>>
        where TValue : IEquatable<TValue>
    {
        #region properties
        /// <summary>The value being stored by this object. </summary>
        public virtual TValue Value { get; protected set; }
        #endregion


        #region constructors
        /// <summary>
        /// Creates a new <see cref="ValueObject{TValue}"/>. Reserved for use by ORM.
        /// </summary>
        protected ValueObject() { }

        /// <summary> Creates a new <see cref="ValueObject{TValue}"/>.</summary>
        /// <param name="value">The value to store in this (new) object.</param>
        public ValueObject(TValue value) => this.Value = value;
        #endregion


        #region overrides & implementations
        /// <summary>Get the list of properties used to make an equality comparison.</summary>
        protected sealed override IEnumerable<object> GetEqualityComponents() => new object[]
        {
            Value
        };


        /// <summary>
        /// Determines whether the specified <see cref="ValueObject{TValue}"/> is equal to the
        /// current <see cref="ValueObject{TValue}"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="ValueObject{TValue}"/> to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="ValueObject{TValue}"/> is equal to the current
        /// <see cref="ValueObject{TValue}"/>; otherwise, false.
        /// </returns>
        public virtual bool Equals(/*[AllowNull]*/ ValueObject<TValue> other) => !(other is null) && GetType() == (
            other?.GetType()) && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        #endregion
    }



    /// <summary>
    /// A generic, immutable object used to store a single value. Used to implement multi-language
    /// features.
    /// </summary>
    /// <typeparam name="TiD">
    /// The <see cref="Type"/> of the <see cref="Index"/> being stored by this object.
    /// </typeparam>
    public abstract class LangValueObject<TiD> : ValueObject, IEquatable<LangValueObject<TiD>>
        where TiD : IEquatable<TiD>
    {
        public virtual TiD Index { get; protected set; }

        /// <summary>The value being stored by this object. </summary>
        public virtual string Value { get; protected set; }

        /// <summary>
        /// A two (2) letter code that denotes the language of the <see cref="Value"/> 
        /// being stored by this object.
        /// </summary>
        public virtual string LanguageCode { get; protected set; }

        /// <summary>
        /// The language culture information of the <see cref="Value"/> being stored by this
        /// object. Used to implement multi-language features.
        /// </summary>
        public virtual CultureInfo LanguageCulture => new CultureInfo(LanguageCode,
                                                                      useUserOverride: false);


        /// <summary>
        /// Determines whether the specified <see cref="LangValueObject{TiD}"/> is equal to the
        /// current <see cref="LangValueObject{TiD}"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="LangValueObject{TiD}"/> to compare with the current object.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="LangValueObject{TiD}"/> is equal to the current
        /// <see cref="LangValueObject{TiD}"/>; otherwise, false.
        /// </returns>
        public virtual bool Equals(/*[AllowNull]*/ LangValueObject<TiD> other) =>
            !(other is null) && GetType() == (
                other?.GetType()) && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    }
}
