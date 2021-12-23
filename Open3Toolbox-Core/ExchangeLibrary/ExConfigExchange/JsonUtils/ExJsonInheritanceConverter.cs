// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    ///     Enables the Inheritance of Json Objects.
    ///     Diese Klasse wurde generiert...
    /// </summary>
    internal class ExJsonInheritanceConverter<TBase> : JsonConverter
    {
        /// <summary>
        ///     The default discriminator name
        /// </summary>
        internal static readonly string DefaultDiscriminatorName = "discriminator";

        /// <summary>
        ///     If it is reading
        /// </summary>
        [ThreadStatic] private static bool _isReading;

        /// <summary>
        ///     If it is writing
        /// </summary>
        [ThreadStatic] private static bool _isWriting;

        /// <summary>
        ///     The discriminator
        /// </summary>
        private readonly string _discriminator;

        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        public ExJsonInheritanceConverter()
        {
            _discriminator = DefaultDiscriminatorName;
        }

        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        /// <param name="discriminator">The name of the discriminator</param>
        public ExJsonInheritanceConverter(string discriminator)
        {
            _discriminator = discriminator;
        }

        #region Properties

        /// <summary>
        ///     If the converter can write
        /// </summary>
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

        /// <summary>
        ///     If the converter can read
        /// </summary>
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

        #endregion

        /// <summary>
        ///     Writes json
        /// </summary>
        /// <param name="writer">The json writer</param>
        /// <param name="value">The value</param>
        /// <param name="serializer">The serializer</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            try
            {
                _isWriting = true;

                var jObject = JObject.FromObject(value, serializer);
                jObject.AddFirst(new JProperty(_discriminator, GetSubtypeDiscriminator(value.GetType())));
                writer.WriteToken(jObject.CreateReader());
            }
            finally
            {
                _isWriting = false;
            }
        }

        /// <summary>
        ///     If the converter can convert
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        ///     Read
        /// </summary>
        /// <param name="reader">The json writer</param>
        /// ///
        /// <param name="objectType">The type of the object</param>
        /// <param name="existingValue">The existing value</param>
        /// <param name="serializer">The serializer</param>
        /// <returns>The object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            if (jObject == null)
            {
                return null!;
            }

            var discriminatorValue = jObject.GetValue(_discriminator);
            var discriminator = discriminatorValue != null ? discriminatorValue.Value<string>() : null;
            var subtype = GetObjectSubtype(objectType, discriminator!);

            var objectContract = serializer.ContractResolver.ResolveContract(subtype) as JsonObjectContract;
            if (objectContract == null || objectContract.Properties.All(p => p.PropertyName != _discriminator))
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

        /// <summary>
        ///     Getting the subtype
        /// </summary>
        /// <param name="objectType">Type of the object</param>
        /// <param name="discriminator">Discriminator</param>
        /// <returns>The type</returns>
        private Type GetObjectSubtype(Type objectType, string discriminator)
        {
            foreach (var attribute in typeof(TBase).GetCustomAttributes<ExJsonInheritanceAttribute>(true))
            {
                if (attribute.Key == discriminator)
                {
                    return attribute.Type;
                }
            }

            return objectType;
        }

        /// <summary>
        ///     Getting the subtype discriminator
        /// </summary>
        /// <param name="objectType">Type of the object</param>
        /// <returns>The name of the object type</returns>
        private string GetSubtypeDiscriminator(Type objectType)
        {
            foreach (var attribute in typeof(TBase).GetCustomAttributes<ExJsonInheritanceAttribute>(true))
            {
                if (attribute.Type == objectType)
                {
                    return attribute.Key;
                }
            }

            return objectType.Name;
        }
    }
}