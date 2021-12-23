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
    ///     Represents the mapping profile for <see cref="TablePublishedProject" />.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ProjectMapping : Profile
    {
        /// <summary>
        ///     Creates The mapping for the
        ///     Data Base entitiy from the input and to the output
        /// </summary>
        public ProjectMapping()
        {
            CreateMap<Project, TableBaseProject>()
                .Include<Project, TablePublishedProject>();
            CreateMap<TableBaseProject, Project>()
                .Include<TablePublishedProject, Project>();

            CreateMap<Project, TablePublishedProject>()
                .ForMember(pro => pro.Company, opt => { opt.MapFrom(proIn => new Company {Email = proIn.Company.Email, Name = proIn.Company.Name}); })
                .ForMember(pro => pro.Gateways, opt => { opt.MapFrom(proIn => proIn.Gateways.ToList()); });
            CreateMap<TablePublishedProject, Project>();
        }
    }
}