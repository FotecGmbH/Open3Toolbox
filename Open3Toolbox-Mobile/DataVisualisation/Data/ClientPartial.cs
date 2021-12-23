// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 23.12.2021 12:29
// Entwickler      Manuel Fasching
// Projekt         DataVisualisation
//
// Released under MIT

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DataVisualisation
{
    public partial class CommunicationInterface
    {
        #region Properties

        [JsonProperty("interfaceType", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public string InterfaceType { get; set; }

        [JsonProperty("chips", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Chip> Chips { get; set; }

        [JsonIgnore]
        public ICollection<Measurement> AllMeasurements { get; set; }

        #endregion
    }

    public class Chip
    {
        #region Properties

        [JsonProperty("name", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [JsonProperty("id", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public int Id { get; set; }


        [JsonProperty("measurements", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Measurement> Measurements { get; set; }

        #endregion
    }

    public class Measurement
    {
        #region Properties

        [JsonProperty("name", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        [JsonProperty("id", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public long Id { get; set; }

        public string IdAsString
        {
            get { return Id.ToString(); }
        }

        [JsonProperty("port", Required = Required.Default)]
        [Required(AllowEmptyStrings = true)]
        public int Port { get; set; }

        #endregion
    }
}