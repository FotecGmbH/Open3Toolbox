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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Exchange.Services.ConfigurationTool.Interfaces;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     Lokale Version vom <see cref="IExCacher{T}" />
    /// </summary>
    /// <typeparam name="T">Der Typ der zu cachenden Items</typeparam>
    /// <seealso cref="Exchange.Services.ConfigurationTool.Interfaces.IExCacher{T}" />
    public class LocalCacher<T> : IExCacher<T>
    {
        /// <summary>
        ///     Die items.
        /// </summary>
        private static readonly ConcurrentDictionary<long, ConcurrentDictionary<long, T>> _items = new ConcurrentDictionary<long, ConcurrentDictionary<long, T>>();

        #region Interface Implementations

        /// <summary>
        ///     Schaut nach ob der Benutzer schon etwas gecachet hat oder nicht.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>
        ///     <c>true</c> wenn der Benutzer bekannt ist sonst <c>false</c>.
        /// </returns>
        public bool IsUserKnown(long userId) =>
            _items.ContainsKey(userId);

        /// <summary>
        ///     Cache-t die items.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="items">Die items.</param>
        /// <exception cref="System.Exception">Users Project were already cached, use {nameof(this.CacheItem)} instead.</exception>
        public void CacheItems(long userId, ConcurrentDictionary<long, T> items)
        {
            if (!_items.TryAdd(userId, items))
            {
                throw new Exception($"Users Project were already cached, use {nameof(CacheItem)} instead.");
            }
        }

        /// <summary>
        ///     Cachte einen item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        /// <param name="item">Der item.</param>
        public void CacheItem(long userId, long itemId, T item)
        {
            if (_items.ContainsKey(userId))
            {
                _items[userId].TryAdd(itemId, item);
            }
            else
            {
                _items.TryAdd(userId, new ConcurrentDictionary<long, T>());
                _items[userId].TryAdd(itemId, item);
            }
        }

        /// <summary>
        ///     Löscht der gecachte item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        public void DeleteCachedItem(long userId, long itemId)
        {
            _items[userId].Remove(itemId, out var _);
        }

        /// <summary>
        ///     Holt den gecachten Items.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>
        ///     Die gecachten Items der Benutzer.
        /// </returns>
        public ConcurrentDictionary<long, T> GetCachedItems(long userId) =>
            _items[userId];

        /// <summary>
        ///     Aktualisiert der gecachte item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        /// <param name="item">Der item.</param>
        public void UpdateCachedItem(long userId, long itemId, T item)
        {
            _items[userId].Remove(itemId, out var _);
            _items[userId].TryAdd(itemId, item);
        }

        #endregion
    }
}