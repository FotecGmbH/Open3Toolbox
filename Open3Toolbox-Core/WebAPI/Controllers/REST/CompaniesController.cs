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
using Database.Context;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using WebAPI.Controllers.REST.Base;

namespace WebAPI.Controllers.REST
{
    /// <summary>
    ///     The MVC-Controller for <see cref="TablePublishedCompany" />.
    /// </summary>
    /// <seealso
    ///     cref="WebAPI.Controllers.REST.Base.RestBaseController{Exchange.Entities.CompanyDB, Exchange.DTOs.Input.CompanyInput, Exchange.DTOs.Output.CompanyOutput}" />
    public class CompaniesController : RestBaseController<TablePublishedCompany, Company, Company>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CompaniesController" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The (auto-)mapper.</param>
        /// <param name="validationVisitor">The validation visitor for the entities</param>
        public CompaniesController(DatabaseContext dbContext, IMapper mapper, IValidationVisitor validationVisitor) : base(dbContext, mapper, validationVisitor)
        {
        }
    }
}