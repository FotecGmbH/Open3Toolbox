// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Text.RegularExpressions;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// Diese Klasse dient zur Validierung vom <see cref="IExConfigItem"/> Implementationen.
    /// </summary>
    /// <seealso cref="ExConfigExchange.Services.Interfaces.IExConfigVisitor{System.Boolean}" />
    public class ExConfigItemValidationVisitor : IExConfigVisitor<bool>
    {

        /// <summary>
        /// Entscheidet ob der Wert im Vorgeschribenen berreich liegt.
        /// </summary>
        /// <param name="value">Der Wert.</param>
        /// <param name="range">Der <see cref="ExRange"/> Instanz.</param>
        /// <returns><c>true</c> falls der Wert im validen Bereich, sonst <c>false</c>.</returns>
        public bool ValueInValidRange(double value, ExRange range) =>
            value >= range.Min && value <= range.Max && value % range.Step == 0;
        
        #region Interface Implementations

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.String" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExStringConfigItem exStringConfigItem, Func<bool> optionalCall = null) => 
            exStringConfigItem.RegexPattern is null ? true : Regex.IsMatch(exStringConfigItem.Value, exStringConfigItem.RegexPattern);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Uri" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExUrlConfigItem exUrlConfigItem, Func<bool> optionalCall = null) => exUrlConfigItem.Valid;

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Byte" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExByteConfigItem exByteConfigItem, Func<bool> optionalCall = null) => 
            exByteConfigItem.ValidRange is null ? exByteConfigItem.Valid : (exByteConfigItem.Valid && this.ValueInValidRange(exByteConfigItem.Value, exByteConfigItem.ValidRange));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int32" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExIntConfigItem exIntConfigItem, Func<bool> optionalCall = null) => 
            exIntConfigItem.ValidRange is null ? exIntConfigItem.Valid : (exIntConfigItem.Valid && this.ValueInValidRange(exIntConfigItem.Value, exIntConfigItem.ValidRange));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int64" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExLongConfigItem exLongConfigItem, Func<bool> optionalCall = null) => 
            exLongConfigItem.ValidRange is null ? exLongConfigItem.Valid : (exLongConfigItem.Valid && this.ValueInValidRange(exLongConfigItem.Value, exLongConfigItem.ValidRange));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Single" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExFloatConfigItem exFloatConfigItem, Func<bool> optionalCall = null) => 
            exFloatConfigItem.ValidRange is null ? exFloatConfigItem.Valid : (exFloatConfigItem.Valid && this.ValueInValidRange(exFloatConfigItem.Value, exFloatConfigItem.ValidRange));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Double" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExDoubleConfigItem exDoubleConfigItem, Func<bool> optionalCall = null) => 
            exDoubleConfigItem.ValidRange is null ? exDoubleConfigItem.Valid : (exDoubleConfigItem.Valid && this.ValueInValidRange(exDoubleConfigItem.Value, exDoubleConfigItem.ValidRange));

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Boolean" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExBoolConfigItem exBoolConfigItem, Func<bool> optionalCall = null) => true;

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="!:enum" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExEnumConfigItem exEnumConfigItem, Func<bool> optionalCall = null) => true;

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Object" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExObjectConfigItem exObjectConfigItem, Func<bool> optionalCall = null)
        {
            if (exObjectConfigItem.IsInterface)
                return !exObjectConfigItem.ImplementationRequired;

            foreach (var item in exObjectConfigItem.Value.Values)
            {
                ///<see cref="Annotations.LeaveNullAttribute"/>
                if (item is null)
                    continue;

                if (!item.Accept(this))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Collections.IEnumerable" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// <c>true</c> falls der <see cref="IExConfigItem"/> implementation valid ist.
        /// </returns>
        public bool Visit(ExCollectionConfigItem exCollectionConfigItem, Func<bool> optionalCall = null)
        {
            foreach (var item in exCollectionConfigItem.Value)
                if (!item.Accept(this))
                    return false;

            return true;
        }

        #endregion
    }
}
