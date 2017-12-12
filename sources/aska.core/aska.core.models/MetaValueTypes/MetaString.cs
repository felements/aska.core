namespace aska.core.models.MetaValueTypes
{
    public class MetaString: MetaObject<string>
    {
        public MetaString() {}
        public MetaString(string value)
        {
            Value = value;
        }

        public sealed override string Value
        {
            get { return RawValue; }
            set { RawValue = value; }
        }

        //public static explicit operator string(MetaString s)
        //{
        //    return s.Value;
        //}

        public static implicit operator string(MetaString s)
        {
            return s.Value;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaString;} }
    }
}