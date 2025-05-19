using System;
using System.Collections.Generic;

using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.objects;



namespace jsolo.simpleinventory.core.entities;


public class Vendor : Entity<int>
{
    #region properties
    /// <summary>
    /// 
    /// </summary>
    public virtual string CompanyName { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual Name ContactPersonName { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string MobilePhoneNumber { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string TelephoneNumber { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string FascimileNumber { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string EmailAddress { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string PhysicalAddress { get; protected set; }
    #endregion


    #region constructors
    /// <summary>
    /// 
    /// </summary>
    protected Vendor() { }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="companyName"></param>
    /// <param name="nameOfContact"></param>
    /// <param name="telephone"></param>
    /// <param name="mobilePhone"></param>
    /// <param name="fax"></param>
    /// <param name="email"></param>
    /// <param name="address"></param>
    public Vendor(
        int id,
        string companyName,
        Name nameOfContact,
        string telephone,
        string mobilePhone,
        string fax,
        string email,
        string address,
        DateTime createdOn,
        string creatorId,
#nullable enable
        DateTime? lastUpdatedOn = null,
        string? lastUpdaterId = null
#nullable disable
    )
    {
        this.Id = id;

        this.CreatedOn = createdOn;
        this.CreatorId = creatorId;

        this.SetCompanyName(companyName)
            .SetNameOfContact(nameOfContact)
            .SetTelephoneNumber(telephone)
            .SetMobilePhoneNumber(mobilePhone)
            .SetFacsimileNumber(fax)
            .SetEmailAddress(email)
            .SetPhysicalAddress(address)
            .SetLastModifierAsAt(lastUpdaterId, lastUpdatedOn);
    }
    #endregion

    #region instance methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyName"></param>
    /// <returns></returns>
    public virtual Vendor SetCompanyName(string companyName)
    {
        this.CompanyName = companyName;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="nameOfContact"></param>
    /// <returns></returns>
    public virtual Vendor SetNameOfContact(Name nameOfContact)
    {
        this.ContactPersonName = nameOfContact;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="telephoneNumber"></param>
    /// <returns></returns>
    public virtual Vendor SetTelephoneNumber(string telephoneNumber)
    {
        this.TelephoneNumber = telephoneNumber;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="mobileNumber"></param>
    /// <returns></returns>
    public virtual Vendor SetMobilePhoneNumber(string mobileNumber)
    {
        this.MobilePhoneNumber = mobileNumber;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="faxNumber"></param>
    /// <returns></returns>
    public virtual Vendor SetFacsimileNumber(string faxNumber)
    {
        this.FascimileNumber = faxNumber;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public virtual Vendor SetEmailAddress(string emailAddress)
    {
        this.EmailAddress = emailAddress;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public virtual Vendor SetPhysicalAddress(string address)
    {
        this.PhysicalAddress = address;

        return this;
    }
    #endregion
        

    #region implementations and overrides
    protected override IEnumerable<object> GetEqualityComponents() =>
    [
        this.CompanyName,
        this.ContactPersonName?.FullName == ""
    ];
    #endregion
}
