// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.DTOs;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     Represents data of a company
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    public class Company : IInputDTO<TablePublishedCompany>, IOutputDTO<TablePublishedCompany>
    {
        #region Properties

        /// <summary>
        ///     The id of the company
        /// </summary>
        [Hidden]
        public long Id { get; set; }


        /// <summary>
        ///     The name of the company.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [DisplayNameProperty]
        public string Name { get; set; }

        /// <summary>
        ///     The contact email address of the company.
        /// </summary>
        [JsonProperty("email", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        #endregion
    }
}