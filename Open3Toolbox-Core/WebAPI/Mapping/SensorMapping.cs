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

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Biss.Serialize;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Entities.Bases;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace WebAPI.Mapping
{
    /// <summary>
    ///     Represents the mapping profile for <see cref="TablePublishedSensor" />.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class SensorMapping : Profile
    {
        /// <summary>
        ///     Creates The mapping for the
        ///     Data Base entitiy from the input and to the output
        /// </summary>
        public SensorMapping()
        {
            CreateMap<Sensor, TableBaseSensor>()
                .Include<Sensor, TablePublishedSensor>();

            CreateMap<TableBaseSensor, Sensor>()
                .Include<TablePublishedSensor, Sensor>();

            CreateMap<Sensor, TablePublishedSensor>()
                .ForMember(sensor => sensor.JsonStringInfo, opt => { opt.MapFrom(sensIn => sensIn.Interfaces.ToJson()); })
                .ForMember(sensor => sensor.AllOpCodes, opt => { opt.MapFrom(sensIn => sensIn.AllOpCodes.ToJson()); });

            CreateMap<TablePublishedSensor, Sensor>();
            CreateMap<TablePublishedSensor, Sensor>()
                .ForMember(sensorOut => sensorOut.Interfaces, opt => { opt.MapFrom(sensDB => BissDeserialize.FromJson<IEnumerable<CommunicationInterface>>(sensDB.JsonStringInfo).ToList()); })
                .ForMember(sensorOut => sensorOut.AllOpCodes, opt => { opt.MapFrom(sensDB => BissDeserialize.FromJson<List<byte>>(sensDB.AllOpCodes).ToList()); });
        }
    }
}