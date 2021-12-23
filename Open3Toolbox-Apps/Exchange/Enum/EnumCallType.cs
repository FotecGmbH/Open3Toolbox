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

namespace Exchange.Enum
{
    /// <summary>
    ///     <para>Enum für Anrufart</para>
    ///     Klasse EnumCallType. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public enum EnumCallType
    {
        // analog zu https://developer.android.com/reference/android/provider/CallLog.Calls.html#TYPE
        Error = 0,
        Incoming = 1,
        Outgoing = 2,
        Missed = 3,
        VoiceMail = 4,
        Rejected = 5,
        Blocked = 6,
        AnsweredExternally = 7,
        Unknown = 8
    }
}