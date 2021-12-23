// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
//
// Automatisch generierter Code. Nicht verändern!
// Erstellt am 11/23/2021 13:26:14

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Biss.Dc.Core;
using Biss.Dc.Server;
using Biss.Serialize;
using Exchange.Model;
using Exchange.Model.ConfigurationTool;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using Exchange.Model.Statistics;

namespace WebServer.DataConnector
{
    /// <summary>
    /// <para>ServerRemoteCallBase</para>
    /// Klasse ServerRemoteCallBase. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public abstract class ServerRemoteCallBase
    {
        /// <summary>
        /// Zugriff auf die Kommunikation mit den angemeldeten Clients
        /// </summary>
        public IDcConnections ClientConnection { get; set; }

        /// <summary>
        /// Workaround um in den Server-Funktionen den Zugriff auf alle angemeldeten Clients zu ermöglichen
        /// </summary>
        /// <param name="connection"></param>
        public void SetClientConnection(object connection)
        {
            if (connection is IDcConnections con)
            {
                ClientConnection = con;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        #region Sendefunktionen

        

        /// <summary>
        /// Daten an DcExMaintenance senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExMaintenance(ExMaintenance data, long? deviceId = null, long? userId = null)
        {
            return await SendInternal("DcExMaintenance", data.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Daten an DcExUserData senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExUserData(ExUserData data, long? deviceId = null, long? userId = null)
        {
            return await SendInternal("DcExUserData", data.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Daten an DcAppCenterSettings senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcAppCenterSettings(ExAppCenterSettings data, long? deviceId = null, long? userId = null)
        {
            return await SendInternal("DcAppCenterSettings", data.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExProjectTemplates senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExProjectTemplates(List<DcServerListItem<ExProject>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExProjectTemplates", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExGatewayTemplates senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExGatewayTemplates(List<DcServerListItem<ExGateway>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExGatewayTemplates", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExSensorTemplates senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExSensorTemplates(List<DcServerListItem<ExSensor>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExSensorTemplates", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExImplementationTemplates senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExImplementationTemplates(List<DcServerListItem<ExObjectConfigItem>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExImplementationTemplates", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an Uids senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExUids(List<DcServerListItem<ExUid>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("Uids", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an Pairs senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExPairs(List<DcServerListItem<ExPair>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("Pairs", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExProjects senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExUserProjects(List<DcServerListItem<ExProject>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExProjects", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExGateways senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExUserGateways(List<DcServerListItem<ExGateway>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExGateways", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExSensors senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExUserSensors(List<DcServerListItem<ExSensor>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExSensors", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExPublishedProjects senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExPublishedProjects(List<DcServerListItem<ExProject>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExPublishedProjects", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExSubViews senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExSubViews(List<DcServerListItem<ExSubView>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExSubViews", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExFinalSubViews senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExFinalSubViews(List<DcServerListItem<ExFinalSubView>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExFinalSubViews", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExActorViews senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExActorViews(List<DcServerListItem<ExActorView>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExActorViews", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExMeasurementViews senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExMeasurementViews(List<DcServerListItem<ExMeasurementView>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExMeasurementViews", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExActorDetails senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExActorDetails(List<DcServerListItem<ExActorDetails>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExActorDetails", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExMeasurementDetails senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExMeasurementDetails(List<DcServerListItem<ExMeasurementDetails>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExMeasurementDetails", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Listen Daten an DcExMeasurementData senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="data">Daten</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        public async Task<int> SendDcExMeasurementData(List<DcServerListItem<ExMeasurementData>> data, long? deviceId = null, long? userId = null)
        {
            var tmp = data.Select(item => item.ToDcTransferListItem()).ToList();
            return await SendInternal("DcExMeasurementData", tmp.ToJson(), deviceId, userId);
        }


        /// <summary>
        /// Daten senden.
        /// Wenn deviceId und userId null sind werden die Daten an alle Geräte gesendet
        /// </summary>
        /// <param name="dpId">Datenpunkt Id</param>
        /// <param name="data">Daten in Json serialisiert</param>
        /// <param name="deviceId">An ein bestimmtes Gerät</param>
        /// <param name="userId">An einen bestimmten Benutzer</param>
        /// <returns>Anzahl der erreichten Geräte</returns>
        private async Task<int> SendInternal(string dpId, string data, long? deviceId, long? userId)
        {
            var d = new DcResult(dpId) {JsonData = data};
            d.Checksum = DcChecksum.Generate(d.JsonData);

            int result = 0;

            if (deviceId == null && userId == null)
            {
                result = await ClientConnection.SendData(d);
            }
            else if (userId != null)
            {
                result = await ClientConnection.SendDataToUser(userId.Value,d);
            }
            else if (deviceId != null)
            {
                if (await ClientConnection.SendDataToDevice(deviceId.Value, d))
                {
                    result = 1;
                }
            }
            return result;
        }

        #endregion

    }
}

