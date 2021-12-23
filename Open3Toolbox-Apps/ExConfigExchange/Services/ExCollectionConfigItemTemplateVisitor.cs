// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Threading.Tasks;
using Biss.Serialize;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// Dieser Visitor ist für die Resolvierung vom <see cref="ExCollectionConfigItem.ItemTemplate"/>-s.
    /// </summary>
    /// <see cref="ExConfigExchange.Services.Interfaces.IExConfigVisitor{System.Threading.Tasks.Task{ExConfigExchange.Models.Interfaces.IExConfigItem}}" />
    public class ExCollectionConfigItemTemplateVisitor : IExConfigVisitor<Task<IExConfigItem>>
    {
        /// <summary>
        /// Der interface resolver <see cref="Func"/>.
        /// </summary>
        private readonly Func<ExObjectConfigItem, Task<ExObjectConfigItem>> _interfaceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="interfaceResolver">The interface resolver.</param>
        /// <exception cref="System.ArgumentNullException">interfaceResolver</exception>
        public ExCollectionConfigItemTemplateVisitor(Func<ExObjectConfigItem, Task<ExObjectConfigItem>> interfaceResolver)
        {
            _interfaceResolver = interfaceResolver ?? throw new ArgumentNullException(nameof(interfaceResolver));
        }

        #region Interface Implementations

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.String" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExStringConfigItem exStringConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exStringConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Uri" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExUrlConfigItem exUrlConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exUrlConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Byte" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExByteConfigItem exByteConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exByteConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int32" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExIntConfigItem exIntConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exIntConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int64" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExLongConfigItem exLongConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exLongConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Single" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExFloatConfigItem exFloatConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exFloatConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Double" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExDoubleConfigItem exDoubleConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exDoubleConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Boolean" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExBoolConfigItem exBoolConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissClone.Clone(exBoolConfigItem) as IExConfigItem);

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="!:enum" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExEnumConfigItem exEnumConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissDeserialize.FromJson<ExEnumConfigItem>(exEnumConfigItem.ToJson()) as IExConfigItem); /// <see cref="BissClone.Clone"/> handles reference types incorrectly so this is needed.

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Object" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public async Task<IExConfigItem> Visit(ExObjectConfigItem exObjectConfigItem, Func<Task<IExConfigItem>> optionalCall = null)
        {
            if (exObjectConfigItem.IsInterface)
                return await this._interfaceResolver(exObjectConfigItem);

            return BissDeserialize.FromJson<ExObjectConfigItem>(exObjectConfigItem.ToJson()); /// <see cref="BissClone.Clone"/> handles reference types incorrectly so this is needed.
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Collections.IEnumerable" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Einen Kopie des resolvierten <see cref="ExCollectionConfigItem.ItemTemplate"/>.
        /// </returns>
        public Task<IExConfigItem> Visit(ExCollectionConfigItem exCollectionConfigItem, Func<Task<IExConfigItem>> optionalCall = null) =>
            Task.FromResult(BissDeserialize.FromJson<ExCollectionConfigItem>(exCollectionConfigItem.ToJson()) as IExConfigItem); /// <see cref="BissClone.Clone"/> handles reference types incorrectly so this is needed.

        #endregion
    }
}
