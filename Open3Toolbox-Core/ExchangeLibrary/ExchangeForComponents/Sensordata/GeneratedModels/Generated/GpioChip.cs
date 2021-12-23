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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;
using OpCodesDllsLibrary;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     Represents a chip, which comunicates via gpio
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    public class GpioChip
    {
        #region Properties

        /// <summary>
        ///     The name of the chip.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [DisplayNameProperty]
        [Hidden]
        public string Name { get; set; }

        /// <summary>
        ///     The id of the sensor
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Hidden]
        public long Id { get; set; }

        /// <summary>
        ///     The measurements of the chip.
        /// </summary>
        [JsonProperty("measurements", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Measurement> Measurements { get; set; }

        /// <summary>
        ///     The actors of the chip.
        /// </summary>
        [JsonProperty("actors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Actor> Actors { get; set; }

        /// <summary>
        ///     The chiptype, which should be the same
        ///     as the chiptype of a iopcodeschip
        /// </summary>
        [JsonProperty("chipType", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [ConfigureAs(typeof(IopCodesChip))]
        [ImplementationRequired]
        public string ChipType { get; set; }

        #endregion
    }
}