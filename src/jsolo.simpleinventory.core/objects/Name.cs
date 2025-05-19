using System.Collections.Generic;

using jsolo.simpleinventory.core.common;



namespace jsolo.simpleinventory.core.objects;


public class Name : ValueObject
{
    #region properties
    public virtual string Title { get; protected set; }

    public virtual string Surname { get; protected set; }

    public virtual string FirstName { get; protected set; }
    #endregion


    #region constructors
    protected Name() { }

    
    public Name(string title, string surname, string firstname)
    {
        this.Title = title;
        this.Surname = surname;
        this.FirstName = firstname;
    }
    #endregion

    #region instance methods & properties
    public virtual string FullName {
        get {
            return $"{this.Title} {this.FirstName} {this.Surname}";
        }
    }
    #endregion
    

    #region  implementatiions and overrides
    protected sealed override IEnumerable<object> GetEqualityComponents() => new object[]
    {
        this.Title,
        this.FirstName,
        this.Surname
    };
    #endregion
}
