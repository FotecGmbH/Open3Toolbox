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
using Database.Context;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers.REST.Base;

namespace WebAPI.Controllers.REST
{
    /// <summary>
    ///     The MVC-Controller for <see cref="TablePublishedProject" />.
    /// </summary>
    /// <seealso
    ///     cref="WebAPI.Controllers.REST.Base.RestBaseController{Exchange.Entities.ProjectDB, Exchange.DTOs.Input.ProjectInput, Exchange.DTOs.Output.ProjectOutput}" />
    public class ProjectsController : RestBaseController<TablePublishedProject, Project, Project>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectsController" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The (auto-)mapper.</param>
        /// <param name="validationVisitor">The validation visitor for the entities</param>
        public ProjectsController(DatabaseContext _dbContext, IMapper mapper, IValidationVisitor validationVisitor) : base(_dbContext, mapper, validationVisitor)
        {
            dbContext = _dbContext;
        }

        #region Properties

        private DatabaseContext dbContext { get; }

        #endregion


        /// <summary>
        ///     Gets the <see cref="TEntity" /> with the given id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <returns>The <see cref="TEntity" />.</returns>
        [HttpGet("/api/Projects/LatestValues/{id}")]
        [EnableQuery]
        public virtual ActionResult<IQueryable<MeasurementValue>> LatestValues(long id)
        {
            var project = dbContext.TblPublishedProjects.Where(a => a.Id == id).Include(ta => ta.Gateways).ThenInclude(a => a.Sensors).FirstOrDefault();
            if (project == null)
            {
                return Ok(null);
            }

            var measurementIds = new List<long>();

            foreach (var gateway in project.Gateways)
            {
                foreach (var sensor in gateway.Sensors)
                {
                    var mappedSensors = Mapper.Map<TablePublishedSensor, Sensor>(sensor);
                    foreach (var ifs in mappedSensors.Interfaces)
                    {
                        if (ifs.GetType() == typeof(I2CInterface))
                        {
                            foreach (var m in ((I2CInterface) ifs).I2cChips)
                            {
                                measurementIds.AddRange(m.Measurements.ToList().Select(a => a.Id));
                            }
                        }
                        else if (ifs.GetType() == typeof(GpioInterface))
                        {
                            foreach (var m in ((GpioInterface) ifs).GpioChips)
                            {
                                measurementIds.AddRange(m.Measurements.ToList().Select(a => a.Id));
                            }
                        }
                    }
                }
            }

            var lstReturn = new List<MeasurementValue>();

            if (measurementIds.Count > 0)
            {
                foreach (var measurementId in measurementIds)
                {
                    var xx = dbContext.TblMeasurementData.Where(a => a.MeasurementId == measurementId).OrderByDescending(a => a.Id).FirstOrDefault();
                    if (xx != null)
                    {
                        lstReturn.Add(Mapper.Map<TableMeasurementData, MeasurementValue>(xx));
                    }
                }
            }

            return Ok(lstReturn);
        }
    }
}