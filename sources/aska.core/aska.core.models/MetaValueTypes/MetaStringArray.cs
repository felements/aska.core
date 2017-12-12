using System;

namespace aska.core.models.MetaValueTypes
{
    public class MetaStringArray : MetaObject<string[]>
    {
        private const char ValuesSplitter = ',';

        public MetaStringArray() {}
        
        public MetaStringArray(string val)
        {
            RawValue = val;
        }

        public bool HasValue
        {
            get { return !string.IsNullOrWhiteSpace(RawValue); }
        }
        
        public sealed override string[] Value
        {
            get
            {
                return !HasValue 
                    ? new string[] { } 
                    : RawValue.Split(new char[] { ValuesSplitter }, StringSplitOptions.RemoveEmptyEntries);
            }
            set { RawValue = string.Join(ValuesSplitter.ToString(), value); }
        }

        public static implicit operator string(MetaStringArray s)
        {
            return s.RawValue;
        }

        public static implicit operator string[](MetaStringArray s)
        {
            return s.Value;
        }

        public override MetaValueType ValueType { get {return MetaValueType.MetaStringArray;} }
    }
}