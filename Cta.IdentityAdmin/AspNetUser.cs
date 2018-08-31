//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Linq;

namespace Cta.IdentityAdmin
{
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    public partial class AspNetUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetUser()
        {
            Id = Guid.NewGuid().ToString();
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.AspNetUserTokens = new HashSet<AspNetUserToken>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTimeOffset> LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public bool Enabled
        {
            get
            {
                return LockoutEnd != new DateTimeOffset(9999, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0));
            }
            set
            {
                LockoutEnd = value ? (DateTimeOffset?) null : new DateTimeOffset(9999, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0));
            }
        }

        public string FirstName
        {
            get { return AspNetUserClaims.FirstOrDefault(x => x.ClaimType == "given_name").ClaimValue; }
            set
            {
                var givenName = AspNetUserClaims.FirstOrDefault(x => x.ClaimType == "given_name");
                if (givenName != null)
                {
                    givenName.ClaimValue = value;
                }
                else
                {
                    AspNetUserClaims.Add(new AspNetUserClaim { ClaimType = "given_name", ClaimValue = value });
                }

            }
        }

        public string LastName
        {
            get { return AspNetUserClaims.FirstOrDefault(x => x.ClaimType == "family_name").ClaimValue; }
            set
            {
                var familyName = AspNetUserClaims.FirstOrDefault(x => x.ClaimType == "family_name");
                if (familyName != null)
                {
                    familyName.ClaimValue = value;
                }
                else
                {
                    AspNetUserClaims.Add(new AspNetUserClaim { ClaimType = "family_name", ClaimValue = value });
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        public bool IsInRole(string roleName)
        {
            return AspNetRoles.Any(x => x.NormalizedName == roleName.ToLower());
        }

        public AccountOrigin Origin { get; set; }
        public string AccountSource { get; set; }
    }

    public enum AccountOrigin {
        Auto, Manual
    }
}
