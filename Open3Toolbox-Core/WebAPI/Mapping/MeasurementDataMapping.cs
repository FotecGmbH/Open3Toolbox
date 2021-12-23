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

using AutoMapper;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.ExchangeData;

namespace WebAPI.Mapping
{
    /// <summary>
    ///     Represents the mapping profile for <see cref="TableMeasurementData" />.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class MeasurementDataMapping : Profile
    {
        /// <summary>
        ///     Creates The mapping for the
        ///     Data Base entitiy from the input and to the output
        /// </summary>
        public MeasurementDataMapping()
        {
            CreateMap<MeasurementValue, TableMeasurementData>();
            CreateMap<TableMeasurementData, MeasurementValue>();
        }
    }
}