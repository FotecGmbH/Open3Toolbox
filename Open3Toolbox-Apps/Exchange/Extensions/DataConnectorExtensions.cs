// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.Linq;
using System.Threading.Tasks;
using Biss.Dc.Client;
using Biss.Dc.Core;

namespace Exchange.Extensions
{
    /// <summary>
    ///     Extension class for data connector
    /// </summary>
    public static class DataConnectorExtensions
    {
        /// <summary>
        ///     Remove item from list without id
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="list">The list where to remove</param>
        /// <param name="item">The item to remove</param>
        /// <returns>The store-result</returns>
        public static Task<DcStoreResult> RemoveWithoutIdAndStore<T>(this DcDataList<T> list, T item)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var toRemove = list.FirstOrDefault(d => d.Data.Equals(item));
            if (toRemove is null)
            {
                return Task.FromResult(new DcStoreResult {ErrorType = EnumServerError.Connection, ServerExceptionText = "Project no longer exists"});
            }

            list.Remove(toRemove);
            return list.StoreAll();
        }

        /// <summary>
        ///     Add an item to the list and store
        /// </summary>
        /// <typeparam name="T">Type of item</typeparam>
        /// <param name="list">The list where to remove</param>
        /// <param name="item">The item to remove</param>
        /// <returns>The store-result</returns>
        public static Task<DcStoreResult> AddAndStore<T>(this DcDataList<T> list, T item)
        {
            list.Add(new DcListDataPoint<T>(item));
            return list.StoreAll();
        }
    }
}