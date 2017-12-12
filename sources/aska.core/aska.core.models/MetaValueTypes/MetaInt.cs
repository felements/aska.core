namespace aska.core.models.MetaValueTypes
{
    public class MetaInt : MetaObject<int>
    {
        public MetaInt() {}

        public MetaInt(int value)
        {
            Value = value;
        }

        public sealed override int Value
        {
            get
            {
                int value;
                return int.TryParse(RawValue, out value) ? value : default(int);
            }
            set { RawValue = value.ToString("D"); }
        }

        public static explicit operator int(MetaInt s)
        {
            return s.Value;
        }

        public static implicit operator string(MetaInt s)
        {
            return s.RawValue;
        }

        public override MetaValueType ValueType { get { return MetaValueType.MetaInt;} }
    }
}