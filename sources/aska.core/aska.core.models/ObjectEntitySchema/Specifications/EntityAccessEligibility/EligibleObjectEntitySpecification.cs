using System;
using System.Collections.Concurrent;
using System.Linq;
using aska.core.models.ObjectEntitySchema.Security;
using aska.core.models.Variants;

namespace aska.core.models.ObjectEntitySchema.Specifications.EntityAccessEligibility
{
    public class EligibleObjectEntitySpecification : ExpressionSpecification<ObjectEntity>
    {
        // ( user_name, user_claims[] )
        private static readonly ConcurrentDictionary<string, UserClaim[]> UserClaims = new ConcurrentDictionary<string, UserClaim[]>();

        // ( user_name, eligible_object_types[] )
        private static readonly ConcurrentDictionary<string, ObjectType[]> EligibleObjectTypes = new ConcurrentDictionary<string, ObjectType[]>();

        private readonly EligibleAction _action;
        private readonly string _userName;

        /// <summary>
        /// Filters entities according to the user's permissions and requested action type
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        public EligibleObjectEntitySpecification(IUserIdentity user, EligibleAction action) : base(entity => true)
        {
            _action = action;
            _userName = user.UserName;

            UserClaims.GetOrAdd(user.UserName, s => user.Claims.Select(c=> Enum.Parse(typeof(UserClaim), c)).Cast<UserClaim>().ToArray());
            var types = EligibleObjectTypes.GetOrAdd(user.UserName, GetEligibleObjectTypes);

            SpecificationExpression = entity => types.Contains(entity.Type);
        }

        /// <summary>
        /// Check if user is eligible to do something with entities of the specified type
        /// </summary>
        /// <param name="otype"></param>
        /// <returns></returns>
        public bool IsEligible(ObjectType otype)
        {
            var types = EligibleObjectTypes.GetOrAdd(_userName, GetEligibleObjectTypes);
            return types.Contains(otype);
        }

        private ObjectType[] GetEligibleObjectTypes(string userName)
        {
            UserClaim[] claims = null;
            if (!UserClaims.TryGetValue(userName, out claims)) return new ObjectType[] {};

            return Enum.GetValues(typeof(ObjectType)).Cast<ObjectType>().Where(otype => IsEligible(_action, otype, claims)).ToArray();
        }

        private static bool IsEligible(EligibleAction action, ObjectType otype, UserClaim[] claims)
        {
            bool eligible = false;
            switch (action)
            {
                case EligibleAction.View:
                    var viewAttrs = EnumExtensions.GetAttributes<ViewPermissionAttribute>(otype).ToList();
                    eligible = viewAttrs.Any(a => claims.Contains(a.Claim));
                    break;
                case EligibleAction.Edit:
                    var editAttrs = EnumExtensions.GetAttributes<EditPermissionAttribute>(otype).ToList();
                    eligible = editAttrs.Any(a => claims.Contains(a.Claim));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            
            return eligible;
        }
    }

    public enum EligibleAction
    {
        View,
        Edit,
    }
}