namespace aska.core.models.MetaValueTypes
{
    public abstract class MetaObject<T> : ISerializedMetaValue
    {
        public abstract T Value { get; set; }
        public abstract MetaValueType ValueType { get; }
        public string RawValue { get; set; }
    }
}