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

using System.Collections.Concurrent;

namespace Exchange.Services.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     Cache-t Items von <see cref="System.Type" /> <typeparamref name="T" /> für einen Benutzer, es ist Thread-Safe.
    /// </summary>
    /// <typeparam name="T">Der <see cref="System.Type" /> der gecache-t wird.</typeparam>
    public interface IExCacher<T>
    {
        /// <summary>
        ///     Schaut nach ob der Benutzer schon etwas gecachet hat oder nicht.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>
        ///     <c>true</c> wenn der Benutzer bekannt ist sonst <c>false</c>.
        /// </returns>
        bool IsUserKnown(long userId);

        /// <summary>
        ///     Cache-t die items.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="items">Die items.</param>
        void CacheItems(long userId, ConcurrentDictionary<long, T> items);

        /// <summary>
        ///     Holt den gecachten Items.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Die gecachten Items der Benutzer.</returns>
        ConcurrentDictionary<long, T> GetCachedItems(long userId);

        /// <summary>
        ///     Cachte einen item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        /// <param name="item">Der item.</param>
        void CacheItem(long userId, long itemId, T item);

        /// <summary>
        ///     Aktualisiert der gecachte item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        /// <param name="item">Der item.</param>
        void UpdateCachedItem(long userId, long itemId, T item);

        /// <summary>
        ///     Löscht der gecachte item.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="itemId">Der Id von item.</param>
        void DeleteCachedItem(long userId, long itemId);
    }
}