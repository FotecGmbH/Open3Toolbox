// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    /// Kann für die deserializierung von unbekannte interface implementationen verwendet werden mit hilfe von <see cref="JsonConverterAttribute"/>. <br/>
    /// <warning>Habe es soweit ich konnte abgesichert aber könnte noch immer gefährlich sein.</warning>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    public class InterfaceJsonConverter<T> : JsonConverter where T : IDeserializable
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Invalid Typing, {objectType.FullName} ∉ {typeof(T).FullName}
        /// or
        /// Warning, Type ({type.FullName}) is not assignable from given Generic type ({typeof(T).FullName})
        /// </exception>
        /// <exception cref="InvalidOperationException">No such assembly loaded: {e.Message}</exception>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!typeof(T).IsAssignableFrom(objectType))
                throw new ArgumentException($"Invalid Typing, {objectType.FullName} ∉ {typeof(T).FullName}");

            var jObject = JToken.ReadFrom(reader) as JObject;
            if (jObject is null)
                return null;

            try
            {
                Type type = IDeserializableJsonExtensions.GetType(jObject);
                if (!typeof(T).IsAssignableFrom(type))
                    throw new ArgumentException($"Warning, Type ({type.FullName}) is not assignable from given Generic type ({typeof(T).FullName})");

                return serializer.Deserialize(jObject.CreateReader(), type);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException($"No such assembly loaded: {e.Message}");
            }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
