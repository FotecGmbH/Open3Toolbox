// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 18.02.2021 15:00
// Entwickler       Matthias Mandl, Sebastian Szvetecz, Istvan Galfi
// Projekt          Dataskop

using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    /// Enables the Inheritance of Json Objects.
    /// Diese Klasse wurde generiert...
    /// </summary>
    internal class ExJsonInheritanceConverter<TBase> : JsonConverter
    {
        internal static readonly string DefaultDiscriminatorName = "discriminator";
    
        private readonly string _discriminator;
    
        [ThreadStatic]
        private static bool _isReading;
    
        [ThreadStatic]
        private static bool _isWriting;
    
        public ExJsonInheritanceConverter()
        {
            _discriminator = DefaultDiscriminatorName;
        }
    
        public ExJsonInheritanceConverter(string discriminator)
        {
            _discriminator = discriminator;
        }
    
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                _isWriting = true;
    
                var jObject = Newtonsoft.Json.Linq.JObject.FromObject(value, serializer);
                jObject.AddFirst(new Newtonsoft.Json.Linq.JProperty(_discriminator, GetSubtypeDiscriminator(value.GetType())));
                writer.WriteToken(jObject.CreateReader());
            }
            finally
            {
                _isWriting = false;
            }
        }
    
        public override bool CanWrite
        {
            get
            {
                if (_isWriting)
                {
                    _isWriting = false;
                    return false;
                }
                return true;
            }
        }
    
        public override bool CanRead
        {
            get
            {
                if (_isReading)
                {
                    _isReading = false;
                    return false;
                }
                return true;
            }
        }
    
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<Newtonsoft.Json.Linq.JObject>(reader);
            if (jObject == null)
                return null;
    
            var discriminatorValue = jObject.GetValue(_discriminator);
            var discriminator = discriminatorValue != null ? Newtonsoft.Json.Linq.Extensions.Value<string>(discriminatorValue) : null;
            var subtype = GetObjectSubtype(objectType, discriminator);

            var objectContract = serializer.ContractResolver.ResolveContract(subtype) as Newtonsoft.Json.Serialization.JsonObjectContract;
            if (objectContract == null || System.Linq.Enumerable.All(objectContract.Properties, p => p.PropertyName != _discriminator))
            {
                jObject.Remove(_discriminator);
            }

            try
            {
                _isReading = true;
                return serializer.Deserialize(jObject.CreateReader(), subtype); /// If this throws an Exception, you likely haven't yet mapped the implementation in <see cref="TBase"/> using <see cref="ExJsonInheritanceAttribute"/>
            }
            finally
            {
                _isReading = false;
            }
        }
    
        private Type GetObjectSubtype(Type objectType, string discriminator)
        {
            foreach (var attribute in typeof(TBase).GetCustomAttributes<ExJsonInheritanceAttribute>(true))
                if (attribute.Key == discriminator)
                    return attribute.Type;
    
            return objectType;
        }
    
        private string GetSubtypeDiscriminator(Type objectType)
        {
            foreach (var attribute in typeof(TBase).GetCustomAttributes<ExJsonInheritanceAttribute>(true))
                if (attribute.Type == objectType)
                    return attribute.Key;
    
            return objectType.Name;
        }
    }

}
