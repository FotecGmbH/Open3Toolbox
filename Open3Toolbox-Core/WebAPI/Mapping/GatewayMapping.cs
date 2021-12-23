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
using AutoMapper;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Entities.Bases;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace WebAPI.Mapping
{
    /// <summary>
    ///     Represents the mapping profile for <see cref="TablePublishedGateway" />.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class GatewayMapping : Profile
    {
        /// <summary>
        ///     Creates The mapping for the
        ///     Data Base entitiy from the input and to the output
        /// </summary>
        public GatewayMapping()
        {
            CreateMap<Gateway, TableBaseGateway>()
                .Include<Gateway, TablePublishedGateway>();
            CreateMap<TableBaseGateway, Gateway>()
                .Include<TablePublishedGateway, Gateway>();

            CreateMap<Gateway, TablePublishedGateway>()
                .ForMember(gate => gate.Sensors, opt => { opt.MapFrom(gate => gate.Sensors.ToList()); });
            CreateMap<TablePublishedGateway, Gateway>();
        }
    }
}