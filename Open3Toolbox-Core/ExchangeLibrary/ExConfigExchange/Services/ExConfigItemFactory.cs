// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Reflection;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;

namespace ExConfigExchange.Services
{
    /// <summary>
    ///     Diese Hilfsklasse bietet methoden an um einfache <see cref="IExConfigItem" /> implementation zu erstellen.
    /// </summary>
    public static class ExConfigItemFactory
    {
        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="string" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExStringConfigItem GetString(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            return new ExStringConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue as string ?? string.Empty,
                       Valid = true,
                       RegexPattern = info.GetRegExPattern(),
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="Uri" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExUrlConfigItem GetUrl(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            return new ExUrlConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? new Uri("https://example.com/") : (defaultValue as Uri),
                       Valid = true,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="byte" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExByteConfigItem GetByte(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            var range = info.GetValidRange();
            return new ExByteConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? (range is null ? default : Convert.ToByte(range.Min)) : Convert.ToByte(defaultValue),
                       Valid = true,
                       ValidRange = range!,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="int" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExIntConfigItem GetInt(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            var range = info.GetValidRange();
            return new ExIntConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? (range is null ? default : Convert.ToInt32(range.Min)) : Convert.ToInt32(defaultValue),
                       Valid = true,
                       ValidRange = range!,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="long" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExLongConfigItem GetLong(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            var range = info.GetValidRange();
            return new ExLongConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? (range is null ? default : Convert.ToInt64(range.Min)) : Convert.ToInt64(defaultValue),
                       Valid = true,
                       ValidRange = range!,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="float" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExFloatConfigItem GetFloat(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            var range = info.GetValidRange();
            return new ExFloatConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? (range is null ? default : Convert.ToSingle(range.Min)) : Convert.ToSingle(defaultValue),
                       Valid = true,
                       ValidRange = range!,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="double" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExDoubleConfigItem GetDouble(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            var range = info.GetValidRange();
            return new ExDoubleConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? (range is null ? default : Convert.ToDouble(range.Min)) : Convert.ToDouble(defaultValue),
                       Valid = true,
                       ValidRange = range!,
                   };
        }

        /// <summary>
        ///     Erstellt die <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="bool" />.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="info">Der <see cref="MemberInfo" /></param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> dann vom Benutzer nur lesbar.</param>
        /// <returns>Der entsprechende <see cref="IExConfigItem" /> implementation.</returns>
        public static ExBoolConfigItem GetBool(string displayKey, MemberInfo info, object defaultValue, bool readOnly)
        {
            return new ExBoolConfigItem
                   {
                       DisplayKey = info.GetDisplayKey() ?? displayKey,
                       Hidden = info.IsHidden(),
                       ReadOnly = readOnly || info.IsReadOnly(),
                       Value = defaultValue is null ? default : Convert.ToBoolean(defaultValue),
                   };
        }
    }
}