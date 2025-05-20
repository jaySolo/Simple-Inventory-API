using System;
using System.Collections.Generic;

using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.objects;



namespace jsolo.simpleinventory.core.objects
{
    public class Currency : ValueObject 
    {
        #region properties
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Symbol { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; protected set; }
        #endregion


        #region constructors
        /// <summary>
        /// Creates a new <see cref="Currency">. Reserved for use by ORM.
        /// </summary>
        protected Currency() { }


        /// <summary>
        /// Creates a new <see cref="Currency">.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="symbol"></param>
        /// <param name="description"></param>
        public Currency(string code, string name, string symbol, string description)
        {
            this.Code = code;
            this.Name = name;
            this.Symbol = symbol;
            this.Description = description;
        }
        #endregion

        #region instance methods & properties
        //NOTE: this value object needs no special instance methods or properties
        #endregion
        

        #region  implementatiions and overrides
        protected sealed override IEnumerable<object> GetEqualityComponents() =>
        [
            this.Symbol
        ];
        #endregion
    }
}
