using System;
using System.Collections.Generic;
using System.Linq;

namespace aska.core.models.Mapper
{
    public class MergeCollectionValueResolver<TEntity, TItem> : IValueResolver<TEntity, TEntity, ICollection<TItem>> where TItem: IEntity where TEntity: IEntity
    {
        private readonly Func<TEntity, ICollection<TItem>> _collectionSelector;
        public MergeCollectionValueResolver(Func<TEntity, ICollection<TItem>> collectionSelector)
        {
            _collectionSelector = collectionSelector;
        }

        public ICollection<TItem> Resolve(TEntity source, TEntity destination, ICollection<TItem> destMember, ResolutionContext context)
        {
            if (source == null || destination == null || destMember == null || _collectionSelector(source) == null) return null;

            var sourceMember = _collectionSelector(source);
            var commonIds = new HashSet<Guid>(sourceMember.Select(x=>x.Id));
            var dstIds = new HashSet<Guid>(destMember.Select(x=>x.Id));
            commonIds.IntersectWith(dstIds);

            // remove old
            var removedCount = destMember.Where(x => !commonIds.Contains(x.Id)).Count(destMember.Remove);
            
            // update common items
            foreach (TItem dstItem in destMember)
            {
                var src = sourceMember.First(x => x.Id == dstItem.Id);
                context.Mapper.Map<TItem, TItem>(src, dstItem);
            }

            // add new
            var addedCount = sourceMember.Where(x=>!commonIds.Contains(x.Id)).Count(x=> { destMember.Add(x);return true;}  );

            return destMember;
        }
    }
}