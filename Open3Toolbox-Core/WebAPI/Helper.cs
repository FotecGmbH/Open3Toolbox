// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Matthias Mandl
// Project:       WebAPI
// 
// Released under MIT

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Database.Context;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace WebAPI
{
    using System;

    /// <summary>
    ///     Helper for helper-functions
    /// </summary>
    public class Helper
    {
        /// <summary>
        ///     the database context
        /// </summary>
        private readonly DatabaseContext _dbContext;

        /// <summary>
        ///     A helper class with support methods
        /// </summary>
        /// <param name="dbContext"></param>
        public Helper(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            SqlNotifierWorker();
        }

        /// <summary>
        ///     Worker for notifying of database changes
        /// </summary>
        public void SqlNotifierWorker()
        {
            var connection = new SqlConnection(_dbContext.ConnectionString);
            var sqlCommand = new SqlCommand("SELECT * FROM TblPublishedProjects", connection);
            var sqlNotificationRequest = new SqlNotificationRequest();
            sqlNotificationRequest.UserData = "NotificationID";

            sqlCommand.Notification = sqlNotificationRequest;
            sqlCommand.ExecuteReader();
        }

        /// <summary>
        ///     Gets the local ip address
        /// </summary>
        /// <returns>local ip address</returns>
        public static IPAddress GetLocalIpAddress() =>
            Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(ipAddr => ipAddr.ToString().Contains("192.168."));


        /// <summary>
        ///     Asignment of id s for all gateways and sensors inside a project
        /// </summary>
        /// <param name="project"></param>
        private void IdAssignment(Project project)
        {
            var gateways = project.Gateways.ToList();

            for (var i = 0; i < gateways.Count; i++)
            {
                gateways.ElementAt(i).Id = i;
                var sensors = gateways.ElementAt(i).Sensors.ToList();
                for (var j = 0; j < sensors.Count; j++)
                {
                    sensors.ElementAt(j).SensorId = long.Parse(i + j.ToString());
                }
            }
        }

        /// <summary>
        ///     Converts data to json string content
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="data">Data to get converted</param>
        /// <returns>String content of the data</returns>
        private StringContent ConvertToContent<T>(T data) =>
            new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        /* public async static void X()
       {
           Thread.Sleep(5000);
           var httpClient = new HttpClient();
           //httpClient.BaseAddress = new Uri("http://192.168.0.27:5000/");
           httpClient.BaseAddress = new Uri("http://192.168.0.30:5000/");

           //////// Deleting measurementData ///////////
           for (int i = 6767; i < 6800; i++)
           {
               try
               {
                   await httpClient.DeleteAsync(httpClient.BaseAddress.AbsoluteUri + "api/measurementdata/" + i);
               }
               catch (Exception e)
               {
               }
               
           }
           //////////////////////////////////////
             
       }*/
    }
}