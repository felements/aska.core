using System.Text.RegularExpressions;

namespace aska.core.models.MetaValueTypes
{
    public class MetaPhone : MetaObject<string>
    {
        private static readonly Regex FilterEx = new Regex(@"\D", RegexOptions.Compiled);
        private const int PhoneMaxLength = 10;

        public MetaPhone() {}
        
        public MetaPhone(string val)
        {
            RawValue = val;
        }

        public bool HasValue
        {
            get { return !string.IsNullOrWhiteSpace(RawValue); }
        }
        
        public sealed override string Value
        {
            get { return RawValue; }
            set
            {
                RawValue = FilterEx.Replace(value ?? "", "");
                if (RawValue.Length > PhoneMaxLength)
                    RawValue = RawValue.TrimStart('7', '8').Limit(PhoneMaxLength);
            }
        }

        public static implicit operator string(MetaPhone s)
        {
            return s.RawValue;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaPhone;} }
    }
}