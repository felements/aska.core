using System;
using System.Linq.Expressions;

namespace aska.core.models.User
{
    public class UserIdentity : IRegularEntity, IEntityFakeDeleted
    {
        public static Expression<Func<UserIdentity, bool>> IsActiveRule = (obj) => !obj.IsDeleted;

        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        public UserIdentity()
        {
        }

        public UserIdentity(UserPrincipal relatedUserPrincipal, UserIdentityProviderKind providerKind )
        {
            Id = Guid.NewGuid();
            UserPrincipalId = relatedUserPrincipal.Id;
            ProviderKind = providerKind;
        }
        
        [Key, Required]
        public Guid Id { get; set; }
        
        public Guid UserPrincipalId { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// provider type selector
        /// </summary>
        public UserIdentityProviderKind ProviderKind { get; set; }

        /// <summary>
        /// serialized provider-specific identity information
        /// </summary>
        public string ProviderDataJson { get; set; }

        public Expression<Func<IEntity, bool>> CompareIdExpression()
        {
            return entity => entity.Id.Equals(Id);
        }

        public bool IsDeleted { get; set; }
    }
}