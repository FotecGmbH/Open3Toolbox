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
using System.Threading.Tasks;
using AutoMapper;
using Biss.Serialize;
using Database.Context;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers.REST.Base;

namespace WebAPI.Controllers.REST
{
    using System;

    /// <summary>
    ///     The MVC-Controller for <see cref="TablePublishedGateway" />.
    /// </summary>
    /// <seealso cref="TablePublishedGateway.DTOs.Input.GatewayInput, Exchange.DTOs.Output.GatewayOutput}" />
    public class GatewaysController : RestBaseController<TablePublishedGateway, Gateway, Gateway>
    {
        /// <summary>Initializes a new instance of the controller</summary>
        /// <param name="dbContext">The data base context</param>
        /// <param name="mapper">The mapper for inputs an outputs</param>
        /// <param name="validationVisitor">The validation visitor for the entities</param>
        public GatewaysController(DatabaseContext dbContext, IMapper mapper, IValidationVisitor validationVisitor) : base(dbContext, mapper, validationVisitor)
        {
        }

        /// <summary>
        ///     Gets the Gateway with the specified setup Id
        ///     Adds the sensors of that gateway to the gateway (this does not run automatically)
        /// </summary>
        /// <param name="setupId">The specified setupId</param>
        /// <returns>The gateway</returns>
        [HttpGet("init/{setupId}")]
        public async Task<ActionResult<Gateway>> GetBySetupId(string setupId)
        {
            long.TryParse(setupId, out var id);

            var gateway = await DatabaseContext
                .TblPublishedGateways
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            // SubProperties need to be included or Integration with Odata

            if (gateway == null)
            {
                return NotFound();
            }

            IEnumerable<TablePublishedSensor> sensors = DatabaseContext
                .TblPublishedSensors
                .Where(sensor => sensor.ProjectId == gateway.ProjectId && sensor.GatewayId == gateway.Id);
            gateway.Sensors = sensors.ToList();


            ICollection<CommunicationInterface> d = BissDeserialize.FromJson<IEnumerable<CommunicationInterface>>(gateway.Sensors.ElementAt(0).JsonStringInfo).ToList();

            var output = Mapper.Map<TablePublishedGateway, Gateway>(gateway);
            return output;
        }

        /*[HttpPut("uniqueId")]
        public async Task SendUniqueId([FromBody] string uniqueId)
        {
            var t = uniqueId;


            
            GatewayDB gateway = await this.DatabaseContext
                .Gateways
                .Where(x => x.SetupId == Guid.Parse(setupId))
                .SingleOrDefaultAsync();

            IEnumerable<SensorDB> sensors = this.DatabaseContext
                .Sensors
                .Where(sensor => sensor.ProjectId == gateway.ProjectId && sensor.GatewayId == gateway.Id);

            gateway.Sensors = sensors.ToList();

            // SubProperties need to be included or Integration with Odata

            if (gateway == null)
                return this.NotFound();

            Gateway output = this.Mapper.Map<GatewayDB, Gateway>(gateway);
            return output;
        }*/
    }
}