using System;

namespace aska.core.networking.JsonSerializer
{
    public class GuidJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((Guid)value).ToString("D"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid);
        }

        public override bool CanWrite { get { return true; } }
        public override bool CanRead { get { return false; } }
    }
}