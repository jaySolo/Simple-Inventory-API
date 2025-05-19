using System;
using System.Collections.Generic;

using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.objects;



namespace jsolo.simpleinventory.core.objects
{
    public class ProductType : ValueObject 
    {
        #region properties
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; protected set; }
        #endregion


        #region constructors
        /// <summary>
        /// Creates a new <see cref="ProductType">. Reserved for use by ORM.
        /// </summary>
        protected ProductType() { }


        /// <summary>
        /// Creates a new <see cref="ProductType">.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public ProductType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
        #endregion

        #region instance methods & properties
        //NOTE: this value object needs no special instance methods or properties
        #endregion
        

        #region  implementatiions and overrides
        protected sealed override IEnumerable<object> GetEqualityComponents() =>
        [
            this.Name
        ];
        #endregion
    }
}
