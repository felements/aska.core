using System;

namespace aska.core.models.MetaValueTypes
{
    public class MetaDateTime : MetaObject<DateTime>
    {
        public MetaDateTime() {}

        public MetaDateTime(DateTime value)
        {
            Value = value;
        }

        public MetaDateTime(DateTime? value)
        {
            Value = value ?? DateTime.MinValue;
        }

        public MetaDateTime(string val)
        {
            RawValue = val;
        }

        public sealed override DateTime Value
        {
            get
            {
                DateTime value;
                return DateTime.TryParse(RawValue, out value) ? value : default(DateTime);
            }
            set { RawValue = value.ToString("O"); }
        }

        public static implicit operator DateTime(MetaDateTime s)
        {
            return s.Value;
        }

        public static implicit operator string(MetaDateTime s)
        {
            return s.Value.ToString("O");
        }

        public override MetaValueType ValueType { get { return MetaValueType.MetaDateTime;} }

        public static MetaDateTime UtcNow()
        {
            return new MetaDateTime(DateTime.UtcNow);
        }

        public static MetaDateTime Now()
        {
            return new MetaDateTime(DateTime.Now);
        }
    }
}