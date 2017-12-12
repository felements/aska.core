using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using aska.core.models.Variants;

namespace aska.core.models.User
{
    public class UserRole : IRegularEntity
    {
        private const string ClaimsDelimiter = ",";

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public UserRole()
        {
        }

        public UserRole(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
        
        public virtual ICollection<UserPrincipal> UserPrincipals { get; set; }

        [Key, Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Index(IsUnique = true)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException();
                _name = value;
            }
        }
        private string _name;

        public string Title { get; set; }

        public bool IsBuiltin { get; set; }

        [NotMapped]
        public UserClaim[] Claims
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ClaimsRaw)) return new UserClaim[] {};
                return ClaimsRaw
                    .Split(new[] {ClaimsDelimiter}, StringSplitOptions.RemoveEmptyEntries)
                    .AsParallel()
                    .Select(x => Enum.Parse(typeof(UserClaim), x))
                    .Cast<UserClaim>()
                    .ToArray();
            }
            set
            {
                if (value == null || !value.Any())
                    ClaimsRaw = string.Empty;
                else
                    ClaimsRaw = string.Join(ClaimsDelimiter, value.Distinct().Cast<int>());
            }
        }

        public string ClaimsRaw { get; set; }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }
    }
}