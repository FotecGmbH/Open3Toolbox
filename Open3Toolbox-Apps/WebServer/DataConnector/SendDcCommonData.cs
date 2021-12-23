// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using System.Linq;
using System.Threading.Tasks;
using Biss.Dc.Core;
using Biss.Interfaces;
using Biss.Serialize;
using Exchange.DataConnector;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für SendDcCommonData</para>
    ///     Klasse SendDcCommonData. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Allgemeine Daten an Device senden. Diese werden über ein Ereignis in der ProjectViewModelBase verarbeitet.
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="type">Enumtyp</param>
        /// <param name="data">Daten</param>
        /// <returns>Ob erfolgreich</returns>
        public Task<bool> SendCommonData(long deviceId, EnumDcCommonCommandsClient type, IBissSerialize data)
        {
            return SendCommonData(deviceId, type, data.ToJson());
        }


        /// <summary>
        ///     Allgemeine Daten an Device senden. Diese werden über ein Ereignis in der ProjectViewModelBase verarbeitet.
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="type">Enumtyp</param>
        /// <param name="data">Daten</param>
        /// <returns>Ob erfolgreich</returns>
        public Task<bool> SendCommonData(long deviceId, EnumDcCommonCommandsClient type, string data)
        {
            var con = ClientConnection.GetClients().FirstOrDefault(f => f.DeviceId == deviceId);
            if (con == null!)
            {
                return Task.FromResult(false);
            }

            return ClientConnection.SendCommonDataToDevice(deviceId, new DcCommonData {Key = type.ToString(), Value = data});
        }
    }
}