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
using ExConfigExchange.Annotations;
using ExConfigExchange.Models;
using Newtonsoft.Json;

namespace ExConfigExchange.Services
{
    /// <summary>
    ///     Diese Hilfsklasse hilft den cref="IExConfigItemManager <see cref="ExConfigExchange.Annotations" /> zu
    ///     behandeln.
    /// </summary>
    public static class ExConfigItemAnnotationHelper
    {
        /// <summary>
        ///     Versucht den display key zu holen.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>cref="null" /> oder der Wert vom <see cref="DisplayKeyAttribute.DisplayKey" />.</returns>
        public static string GetDisplayKey(this MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<DisplayKeyAttribute>();
            return attribute is null ? null : attribute.DisplayKey;
        }

        /// <summary>
        ///     Versucht den <see cref="JsonPropertyAttribute.PropertyName" /> wert zu holen.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>cref="null" /> oder der Wert vom <see cref="JsonPropertyAttribute.PropertyName" />.</returns>
        public static string GetJsonPropertyName(this MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<JsonPropertyAttribute>();
            return attribute is null ? null : attribute.PropertyName;
        }

        /// <summary>
        ///     Versucht den regex pattern zu holen.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>="null" /> oder der Wert vom <see cref="RegexAttribute.RegexPattern" />.</returns>
        public static string GetRegExPattern(this MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<RegexAttribute>();
            return attribute is null ? null : attribute.RegexPattern;
        }

        /// <summary>
        ///     Entscheidet ob dieser <see cref="MemberInfo" /> cref="null" /> immer gelassen werden sollte oder nicht.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns><c>true</c> falls  cref="null" /> lassen sollte, sonst <c>false</c>.</returns>
        public static bool ShouldLeaveNull(this MemberInfo member) =>
            member.GetCustomAttribute<LeaveNullAttribute>() != null;

        /// <summary>
        ///     Entscheidet ob dieser <see cref="MemberInfo" /> vor Benutzer versteckt werden sollte oder nicht.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>
        ///     <returns><c>true</c> falls vor Benutzer versteckt werden sollte, sonst <c>false</c>.</returns>
        /// </returns>
        public static bool IsHidden(this MemberInfo member) =>
            member.GetCustomAttribute<HiddenAttribute>() != null || member.GetCustomAttribute<LeaveNullAttribute>() != null;

        /// <summary>
        ///     Entscheidet ob dieser <see cref="MemberInfo" /> für den Benutzer nur lesbar werden sollte oder nicht.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>
        ///     <returns><c>true</c> falls für den Benutzer nur lesbar sein sollte, sonst <c>false</c>.</returns>
        /// </returns>
        public static bool IsReadOnly(this MemberInfo member) =>
            member.GetCustomAttribute<ReadOnlyAttribute>() != null || member.GetCustomAttribute<LeaveNullAttribute>() != null;

        /// <summary>
        ///     Entscheidet ob dieser <see cref="MemberInfo" /> als interface behandelt werden sollte oder nicht.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns><c>true</c> falls als interface behandelt sollte, sonst <c>false</c>.</returns>
        public static bool ShouldBeTreatedAsInterface(this MemberInfo member) =>
            member.GetCustomAttribute<InterfaceAttribute>() != null;

        /// <summary>
        ///     Entscheidet ob dieser <see cref="MemberInfo" /> als eine andere <see cref="Type" /> vom Benutzer konfiguriert
        ///     werden sollte oder nicht.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>Der <see cref="Type" /> wie dieser <see cref="MemberInfo" /> vom Benutzer konfiguriert werden sollte.</returns>
        public static Type ShouldConfigureAs(this MemberInfo member)
        {
            var attribute = member.GetCustomAttribute<ConfigureAsAttribute>();
            return attribute is null ? null : attribute.Target;
        }

        /// <summary>
        ///     Versucht den <see cref="ValidRangeAttribute" /> zu holen und daraus einen <see cref="ExRange" /> Instanz zu
        ///     kreiren.
        /// </summary>
        /// <param name="member">Der <see cref="MemberInfo" />.</param>
        /// <returns>
        ///     cref="null" /> wenn keine <see cref="ValidRangeAttribute" /> vorhanden, sonst einen
        ///     <see cref="ExRange" /> Instanz mit dem Werten von <see cref="ValidRangeAttribute" />.
        /// </returns>
        public static ExRange GetValidRange(this MemberInfo member)
        {
            var range = member.GetCustomAttribute<ValidRangeAttribute>();
            return range is null ? null : new ExRange(range.Min, range.Max, range.Step);
        }

        /// <summary>
        ///     If is implemented is required
        /// </summary>
        /// <param name="member">The member where to look if it is required</param>
        /// <returns>If it is required</returns>
        public static bool IsImplementationRequired(this MemberInfo member) => member.GetCustomAttribute<ImplementationRequiredAttribute>() != null;
    }
}