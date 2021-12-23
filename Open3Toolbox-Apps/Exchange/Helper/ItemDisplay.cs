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

namespace Exchange.Helper
{
    /// <summary>
    ///     <para>Wrapper für Darstellung in Picker</para>
    ///     Klasse ItemDisplay. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ItemDisplay<T>
    {
        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="displayString">The display string</param>
        public ItemDisplay(T item, string displayString)
        {
            Item = item;
            DisplayString = displayString;
        }

        /// <summary>
        ///     <summary>
        ///         Initializes a new instance
        ///     </summary>
        ///     <param name="item">The item</param>
        public ItemDisplay(T item)
        {
            Item = item;

            //if (item is EnumSpecialTimeType timeType)
            //{
            //}
            //else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        #region Properties

        /// <summary>
        ///     Item
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        ///     Darstellung des Item in UI
        /// </summary>
        public string DisplayString { get; set; }

        #endregion
    }
}