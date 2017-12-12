namespace aska.core.models.MetaValueTypes
{
    public class MetaJsonObject<T> : MetaObject<T>
    {
        public MetaJsonObject() { }

        public MetaJsonObject(T value)
        {
            Value = value;
        }

        public sealed override T Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RawValue)) return default(T);
                var obj = JsonConvert.DeserializeObject<T>(RawValue);
                return obj;
            }
            set { RawValue = value != null ? JsonConvert.SerializeObject(value) : null; }
        }

        public static explicit operator T(MetaJsonObject<T> s)
        {
            return s.Value;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaJsonSerialized;} }
    }
}