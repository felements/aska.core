using System;
using System.Linq;
using System.Linq.Expressions;
using aska.core.models.ObjectEntitySchema;

// ReSharper disable once CheckNamespace
namespace ask.realty.contracts.models.ObjectEntitySchema
{
    public partial class ObjectEntity
    {
        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, bool>> IsSecretMissionMemberRule = (obj) => obj.Type == ObjectType.UserDataRealtor;

        public static Expression<Func<aska.core.models.ObjectEntitySchema.ObjectEntity, string>> PublicScoreSelector = (obj) => obj.Values.Where(v => v.ParameterKey == ObjectParameterKey.VotePublicScore).Select(v => v.RawValue).FirstOrDefault();
    }
}