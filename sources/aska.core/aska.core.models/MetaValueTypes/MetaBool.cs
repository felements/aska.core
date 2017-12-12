namespace aska.core.models.MetaValueTypes
{
    public class MetaBool : MetaObject<bool>
    {
        public MetaBool()  {}

        public MetaBool(bool value)
        {
            Value = value;
        }

        public MetaBool(string value)
        {
            RawValue = value;
        }

        public sealed override bool Value
        {
            get
            {
                bool value;
                return bool.TryParse(RawValue, out value) && value;
            }
            set { RawValue = value.ToString(); }
        }

        public static explicit operator bool(MetaBool s)
        {
            return s.Value;
        }

        public static implicit operator string(MetaBool s)
        {
            return s.RawValue;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaBool;} }
    }
}