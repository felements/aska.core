using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;
using aska.core.models.Variants;

namespace aska.core.models.User
{
    public class UserPrincipal : IUserIdentity, IRegularEntity, IEntityFakeDeleted
    {
        public static Expression<Func<UserPrincipal, bool>> IsActiveRule = (obj) => !obj.IsDeleted;
        private string _name, _primaryEmail;

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public UserPrincipal()
        {
        }

        /// <summary>
        /// Creates new instance of the User account record
        /// </summary> 
        /// <param name="name">User identity name. Should be unique.</param>
        /// <param name="primaryEmail">User's primary email address.</param>
        public UserPrincipal(string name, string primaryEmail)
        {
            Id = Guid.NewGuid();
            Name = name;
            PrimaryEmail = primaryEmail;
        }

        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        /// User's extended data entity ID
        /// </summary>
        public Guid? UserDataEntityId { get; set; }

        [ForeignKey("UserDataEntityId")]
        public virtual ObjectEntity UserDataEntity { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<UserIdentity> Identity { get; set; }

        /// <summary>
        /// User's credential identity
        /// </summary>
        [Required]
        [StringLength(200)]
        [Index(IsUnique = true)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException();
                _name = value.ToLowerInvariant();
            }
        }

        [Required]
        public string PrimaryEmail
        {
            get { return _primaryEmail; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException();
                _primaryEmail = value.ToLowerInvariant();
            }
        }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }

        public bool IsDeleted { get; set; }

        public string UserName
        {
            get { return Name; }
            set { return; }
        }

        [NotMapped]
        public IEnumerable<string> Claims
        {
            get
            {
                if (UserRoles == null) return new string[] {};
                return UserRoles.SelectMany(role => role.Claims, (role, claim) => claim).Distinct().Select(x=>x.ToString("G")).ToList();
            }
        }

        [NotMapped]
        public UserClaim[] UserClaims
        {
            get
            {
                if (UserRoles == null) return new UserClaim[] { };
                return UserRoles.SelectMany(role => role.Claims, (role, claim) => claim).Distinct().ToArray();
            }
        }
    }
}