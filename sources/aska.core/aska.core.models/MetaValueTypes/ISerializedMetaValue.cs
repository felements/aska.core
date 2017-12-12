namespace aska.core.models.MetaValueTypes
{
    public interface ISerializedMetaValue
    {
        string RawValue { get; set; }
        MetaValueType ValueType { get; }
    }
}