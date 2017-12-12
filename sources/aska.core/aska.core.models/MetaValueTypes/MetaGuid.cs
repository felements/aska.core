using System;

namespace aska.core.models.MetaValueTypes
{
    public class MetaGuid : MetaObject<Guid>
    {
        public MetaGuid() {}

        public MetaGuid(Guid value)
        {
            Value = value;
        }

        public MetaGuid(string val)
        {
            RawValue = val;
        }

        public MetaGuid(Guid? value)
        {
            Value = value ?? Guid.Empty;
        }

        public sealed override Guid Value
        {
            get
            {
                Guid value;
                return Guid.TryParse(RawValue, out value) ? value : Guid.Empty;
            }
            set { RawValue = value.ToString("D"); }
        }

        public static explicit operator Guid(MetaGuid s)
        {
            return s.Value;
        }

        public static implicit operator string(MetaGuid s)
        {
            return s.RawValue;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaGuid;} }
    }
}