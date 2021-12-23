// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeLibrary.ExchangeData;
using Newtonsoft.Json;

namespace ExchangeLibrary.RestCommunication
{
    using System;

    /// <summary>
    ///     The server http client, which handles the communication to the server
    /// </summary>
    public class ServerHttpClient : IDisposable
    {
        /// <summary>
        ///     The http client
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Initializes a new instance of the client
        /// </summary>
        /// <param name="baseAddress">The Uri of the server</param>
        public ServerHttpClient(Uri baseAddress)
        {
            _httpClient = new HttpClient();
            BaseAddress = baseAddress;
        }

        #region Properties

        /// <summary>
        ///     The base address of the server
        /// </summary>
        public Uri BaseAddress
        {
            get => _httpClient.BaseAddress;
            set => _httpClient.BaseAddress = value ?? throw new ArgumentNullException(nameof(BaseAddress));
        }

        #endregion

        /// <summary>
        ///     Send a measurementvalue to the server
        /// </summary>
        /// <param name="data">The measurement value to send</param>
        /// <returns>The http response from the server</returns>
        public Task<HttpResponseMessage> SendMeasurementValueAsync(MeasurementValue data) =>
            _httpClient.PostAsync(BaseAddress.AbsoluteUri + "api/measurementdata", ConvertToContent(data));

        /// <summary>
        ///     Gets the data for a gateway by its setupID
        /// </summary>
        /// <param name="setupId">The setup id</param>
        /// <returns>The http response from the server</returns>
        public Task<HttpResponseMessage> GetGatewayDataAsync(string setupId) =>
            _httpClient.GetAsync(BaseAddress.AbsoluteUri + "api/gateways/init/" + setupId);

        /// <summary>
        ///     Gets the measurementdatas from the http server
        /// </summary>
        /// <returns>The measurementsdatas</returns>
        public Task<HttpResponseMessage> GetMeasurementDatasAsync() =>
            _httpClient.GetAsync(BaseAddress.AbsoluteUri + "api/measurementdata/dbdata");


        /// <summary>
        ///     Converts data to a string content
        /// </summary>
        /// <typeparam name="T">The type of the data</typeparam>
        /// <param name="data">The data</param>
        /// <returns>The stringcontent of the data</returns>
        private static StringContent ConvertToContent<T>(T data) =>
            new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        #region Interface Implementations

        /// <summary>
        ///     Disposes the http client
        /// </summary>
        public void Dispose()
        {
            _httpClient.Dispose();
        }

        #endregion
    }
}