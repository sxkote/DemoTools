using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SX.Common.Shared.Models;
using System;

namespace SX.Common.Shared.Classes
{
    public class JsonCustomValueConverter : JsonCustomConverter<CustomValue>
    {
        protected override CustomValue Create(Type objectType, JToken jToken)
        {
            if (jToken == null)
                return null;

            var type = jToken.Type;

            switch (type)
            {
                case JTokenType.Date:
                    return new CustomValueDate((DateTime)jToken);
                case JTokenType.Boolean:
                    return new CustomValueBool((Boolean)jToken);
                case JTokenType.Integer:
                    return new CustomValueNumber((Int64)jToken);
                case JTokenType.Float:
                    return new CustomValueNumber((Decimal)jToken);
                case JTokenType.String:
                    return new CustomValueText((String)jToken);
                default:
                    return new CustomValue<object>(jToken.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            // Load JObject from stream
            var jValue = JValue.Load(reader);

            // Create target object based on JObject
            return Create(objectType, jValue);

            ////Create a new reader for this jObject, and set all properties to match the original reader.
            //JsonReader jObjectReader = jObject.CreateReader();
            //jObjectReader.Culture = reader.Culture;
            //jObjectReader.DateParseHandling = reader.DateParseHandling;
            //jObjectReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            //jObjectReader.FloatParseHandling = reader.FloatParseHandling;

            //// Populate the object properties
            //serializer.Populate(jObjectReader, target);

            //return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var customValue = value as CustomValue;
            if (customValue == null)
            {
                JValue.CreateNull().WriteTo(writer);
            }
            else
            {
                JValue.FromObject(customValue.Value).WriteTo(writer);
            }
        }
    }
}
