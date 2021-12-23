// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using Biss.Serialize;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    /// <warning>Fusch</warning>
    /// </summary>
    public static class IDeserializableJsonExtensions
    {
        /// <summary>
        /// Deserializiert einen Objekt aus einem Json string.
        /// Verwendet <see cref="IDeserializable"/>.
        /// </summary>
        /// <typeparam name="TInterfaceType">The type of the interface type.</typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static TInterfaceType FromJson<TInterfaceType>(this string json) where TInterfaceType : class, IDeserializable
        {
            return json.FromJson(typeof(TInterfaceType)) as TInterfaceType;
        }

        /// <summary>
        /// Deserializiert einen Objekt aus einem Json string.
        /// Verwendet <see cref="IDeserializable"/>.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Warning, Type ({type.FullName}) is not assignable from given Generic type ({interfaceType.FullName})</exception>
        internal static object FromJson(this string json, Type interfaceType)
        {
            JObject jObject = JObject.Parse(json);
            Type type = GetType(jObject);
            if (!interfaceType.IsAssignableFrom(type))
                throw new ArgumentException($"Warning, Type ({type.FullName}) is not assignable from given Generic type ({interfaceType.FullName})");

            return BissDeserialize.FromJson(json, type);
        }

        /// <summary>
        /// Holt sich den Type mit Hilfe von der <see cref="IDeserializable"/> Interface.
        /// </summary>
        /// <param name="jObject">Der jObject.</param>
        /// <returns></returns>
        internal static Type GetType(JObject jObject)
        {
            var typeFullName = jObject[nameof(IDeserializable.TypeFullName)].Value<string>();
            var assemblyFullName = jObject[nameof(IDeserializable.AssemblyFullname)].Value<string>();
            return AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName == assemblyFullName).GetType(typeFullName);
        }
    }
}
