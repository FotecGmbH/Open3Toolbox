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
using AutoMapper.QueryableExtensions;
using Database.Context;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers.REST.Base;

namespace WebAPI.Controllers.REST
{
    /// <summary>
    ///     The MVC-Controller for <see cref="Measurement" />.
    /// </summary>
    /// <seealso cref="Measurement.DTOs.Input.MeasurementInput, Exchange.DTOs.Output.MeasurementOutput}" />
    public class MeasurementDataController : RestBaseController<TableMeasurementData, MeasurementValue, MeasurementValue>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeasurementDataController" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The (auto-)mapper.</param>
        /// <param name="validationVisitor">The vaWählen Sie Ihre Video- und Audiooptionen aus.lidation visitor for the entities</param>
        public MeasurementDataController(DatabaseContext _dbContext, IMapper mapper, IValidationVisitor validationVisitor) : base(_dbContext, mapper, validationVisitor)
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
        [HttpGet("/api/MeasurementData/query/{id}/{orderasc}/{take}/{skip}")]
        [EnableQuery]
        public virtual ActionResult<IQueryable<MeasurementValue>> Get(long id = -1, bool orderasc = false, int take = 20, int skip = 0)
        {
            if (orderasc == false)
            {
                var xx = dbContext.TblMeasurementData.Where(a => a.MeasurementId == id || id == -1).OrderByDescending(a => a.Id).Skip(skip).Take(take).ProjectTo<MeasurementValue>(Mapper.ConfigurationProvider).ToList();
                return Ok(xx);
            }
            else
            {
                var xx = dbContext.TblMeasurementData.Where(a => a.MeasurementId == id || id == -1).Skip(skip).Take(take).ProjectTo<MeasurementValue>(Mapper.ConfigurationProvider).ToList();
                return Ok(xx);
            }
        }

                /// <summary>
        /// Gets the <see cref="TEntity"/> with the given id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <returns>The <see cref="TEntity"/>.</returns>
        [HttpGet("/api/MeasurementData/queryextended/{id}/{take}/{skip}")]
        [EnableQuery]
        public virtual ActionResult<MeasurementValueQueryExtendedResult> QueryExtended(long id = -1, int take = 20, int skip = 0, [FromQuery]string filter = "", [FromQuery]string orderby = "Value desc")
        {
            var result = new MeasurementValueQueryExtendedResult();

            var queryAble = dbContext.TblMeasurementData.AsQueryable();

            if(!string.IsNullOrEmpty(orderby))
            {
                queryAble = queryAble.OrderBy(orderby);
            }

            if(!string.IsNullOrEmpty(filter))
            {
                queryAble = queryAble.Where(filter);
            }

            var xx = queryAble.Where(a => a.MeasurementId == id || id == -1).Skip(skip).Take(take).ProjectTo<MeasurementValue>(Mapper.ConfigurationProvider);
            result.MeasurementValues = xx;
            result.Count = dbContext.TblMeasurementData.Count(a => a.MeasurementId == id || id == -1);
            return Ok(result);
           
        }

        /// <summary>
        /// Gets all <see cref="TEntity"/>s.
        /// </summary>
        /// <returns></returns>
        /*[HttpGet("humanReadableValues")]
        [EnableQuery]
        public virtual ActionResult<IQueryable<List<MeasurementValue>>> GetAll()
        {
            var dbValues = DatabaseContext.TblMeasurementData;

            var results = new List<TableMeasurementData>();

            var opCodeChips = ChipDllsHandler.GetOpCodeChips(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls");

            var sensors = DatabaseContext.TblPublishedSensors.ToList();

            var comInterfaces = new List<CommunicationInterface>();

            sensors.ForEach(sens => comInterfaces.AddRange(BissDeserialize.FromJson<List<CommunicationInterface>>(sens.JsonStringInfo)));

            var values = dbValues.ToList();

            values.ForEach(val =>
            {
                results.Add(val);
            });



            return Ok(results);
        }*/
    }

    public class MeasurementValueQueryExtendedResult
    {
        public long MeasurementId { get; set; }
        public string MeasurementName {get;set;}
        public long Count {get;set;}

        public IQueryable<MeasurementValue> MeasurementValues{get;set;}
    } 
}

