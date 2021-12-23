// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Json;
using System.Linq;
using Biss.Log.Producer;
using Biss.Serialize;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// Dieser Visitor ist für die Konvertierung vom <see cref="IExConfigItem"/> Implementationen in <see cref="JsonValue"/>s.
    /// </summary>
    /// <seealso cref="ExConfigExchange.Services.Interfaces.IExConfigVisitor{System.Json.JsonValue}" />
    public class ExConfigItemJsonConverterVisitor : IExConfigVisitor<JsonValue>
    {
        #region Interface Implementations

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.String" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExStringConfigItem exStringConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse($"\"{exStringConfigItem.Value}\"");

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Uri" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExUrlConfigItem exUrlConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse($"\"{exUrlConfigItem.Value}\"");

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Byte" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExByteConfigItem exByteConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exByteConfigItem.Value.ToString());

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int32" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExIntConfigItem exIntConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exIntConfigItem.Value.ToString());

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int64" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExLongConfigItem exLongConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exLongConfigItem.Value.ToString());

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Single" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExFloatConfigItem exFloatConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exFloatConfigItem.Value.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Double" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExDoubleConfigItem exDoubleConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exDoubleConfigItem.Value.ToString("0.000000000000000", System.Globalization.CultureInfo.InvariantCulture));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Boolean" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExBoolConfigItem exBoolConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(exBoolConfigItem.Value.ToString().ToLower());

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="!:enum" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExEnumConfigItem exEnumConfigItem, Func<JsonValue> optionalCall = null) => 
            JsonValue.Parse(
                exEnumConfigItem.Selected is null 
                    ? exEnumConfigItem.Value.First().Value.ToString() 
                    : exEnumConfigItem.Selected.Value.ToString()
                    );

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Object" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExObjectConfigItem exObjectConfigItem, Func<JsonValue> optionalCall = null)
        {
            if (exObjectConfigItem.IsInterface)
                return null;

            var obj = new JsonObject();
            foreach (var exConfigItem in exObjectConfigItem.Value)
                obj.Add(exConfigItem.Key, exConfigItem.Value is null ? null : exConfigItem.Value.Accept(this));

            if (exObjectConfigItem.HadConfigureAsAttribute)
                return JsonValue.Parse(obj.ToString().ToJson());
            else
                return obj;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Collections.IEnumerable" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Der <see cref="JsonValue"/> vom <see cref="IExConfigItem"/> Implementation.
        /// </returns>
        public JsonValue Visit(ExCollectionConfigItem exCollectionConfigItem, Func<JsonValue> optionalCall = null)
        {
            var arr = new JsonArray();

            foreach (var exConfigItem in exCollectionConfigItem.Value)
                arr.Add(exConfigItem.Accept(this));

            return arr;
        }

        #endregion
    }
}
