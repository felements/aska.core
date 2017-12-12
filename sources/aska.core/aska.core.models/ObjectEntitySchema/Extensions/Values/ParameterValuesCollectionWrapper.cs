using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aska.core.models.ObjectEntitySchema.Extensions.Values
{
    public class ParameterValuesCollectionWrapper : ICollection<ObjectParameterValue>
    {
        private readonly ICollection<ObjectParameterValue> _baseCollection;
        private readonly ObjectParameterKey _key;
        private readonly Guid _entityId;

        public ParameterValuesCollectionWrapper(Guid entityId, ObjectParameterKey key, ICollection<ObjectParameterValue> baseCollection)
        {
            _entityId = entityId;
            _baseCollection = baseCollection;
            _key = key;
        }

        public IEnumerator<ObjectParameterValue> GetEnumerator()
        {
            return _baseCollection.Where(x => x.ParameterKey == _key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string rawValue)
        {
            _baseCollection.Add(new ObjectParameterValue(_key, _entityId, rawValue));
        }

        public void Add(ObjectParameterValue item)
        {
            _baseCollection.Add(item);
        }

        public void Clear()
        {
            var items = _baseCollection.Where(x => x.ParameterKey == _key).ToList();

            foreach (var objectParameterValue in items)
            {
                _baseCollection.Remove(objectParameterValue);
            }
        }

        public bool Contains(string rawValue)
        {
            return _baseCollection.Any(x => x.ParameterKey == _key && string.Equals(x.RawValue, rawValue, StringComparison.InvariantCulture));
        }

        public bool Contains(ObjectParameterValue item)
        {
            return item != null
                   && item.ParameterKey == _key
                   && _baseCollection.Any(x => x.ParameterKey == _key && x.Id == item.Id);
        }

        public void CopyTo(ObjectParameterValue[] array, int arrayIndex)
        {
            //TODO:
            throw new System.NotImplementedException();
        }

        public bool Remove(ObjectParameterValue item)
        {
            return _baseCollection.Remove(item);
        }

        public int Count
        {
            get { return _baseCollection.Count(x => x.ParameterKey == _key); }
        }

        public bool IsReadOnly
        {
            get { return _baseCollection.IsReadOnly; }
        }

        private IEnumerable<ObjectParameterValue> Filtered()
        {
            return _baseCollection.Where(x => x.ParameterKey == _key);
        }

        public IEnumerable AsTyped(dynamic defaultValue)
        {
            return this.Filtered().Select(value => TypedValue.AsTyped(value, defaultValue)).AsEnumerable();
        }
    }

    public static class ValuesCollectionExtension
    {
        public static ParameterValuesCollectionWrapper ValuesByKey(this ObjectEntitySchema.ObjectEntity entity, ObjectParameterKey key)
        {
            return entity != null ? new ParameterValuesCollectionWrapper(entity.Id, key, entity.Values) : null;
        }

        public static ObjectParameterValue ValueByKey(this ObjectEntitySchema.ObjectEntity entity, ObjectParameterKey key)
        {
            return entity.Values.FirstOrDefault(x=>x.ParameterKey == key);
        }
    }
}