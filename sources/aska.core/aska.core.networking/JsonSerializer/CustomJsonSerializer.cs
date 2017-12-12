namespace aska.core.networking.JsonSerializer
{
    public sealed class CustomJsonSerializer : Newtonsoft.Json.JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter(){ CamelCaseText = true}); ;
            this.Converters.Add(new GuidJsonConverter());
            this.ContractResolver = new CamelCasePropertyNamesContractResolver();
            this.Formatting = Formatting.Indented;
            this.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}