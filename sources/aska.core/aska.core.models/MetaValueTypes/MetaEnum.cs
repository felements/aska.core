using System;

namespace aska.core.models.MetaValueTypes
{
    public class MetaEnum<TEnum> : MetaObject<TEnum> where TEnum : struct
    {
        public MetaEnum() { }

        public MetaEnum(TEnum value)
        {
            Value = value;
        }

        public MetaEnum(string val)
        {
            RawValue = val;

        }

        public sealed override TEnum Value
        {
            get
            {
                TEnum value;
                Enum.TryParse(RawValue, true, out value);
                return value;
            }
            set { RawValue = (value as Enum).ToString("O"); }
        }

        public static implicit operator TEnum(MetaEnum<TEnum> s)
        {
            return s.Value;
        }

        public static implicit operator string(MetaEnum<TEnum> s)
        {
            return s.RawValue;
        }

        public override MetaValueType ValueType { get { return MetaValueType.MetaEnum; } }

        
    }
}