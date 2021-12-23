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
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Database.Context;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.DTOs;
using ExchangeLibrary.DataBaseData.Interfaces.Models;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using FluentValidation;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers.REST.Base
{
    using System;

    /// <summary>
    ///     Represents the base-controller, implementing CRUD-functionality.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TInput">The type of the input-DTO.</typeparam>
    /// <typeparam name="TOutput">The type of the output-DTO.</typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class RestBaseController<TEntity, TInput, TOutput> : ControllerBase
        where TEntity : class, IEntity
        where TInput : class, IInputDTO<TEntity>
        where TOutput : class, IOutputDTO<TEntity>
    {
        /// <summary>
        ///     The database context.
        /// </summary>
        private readonly DatabaseContext _dbContext;

        /// <summary>
        ///     The (auto-)mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        ///     The validation visitor.
        /// </summary>
        private readonly IValidationVisitor _validationVisitor;


        /*private void DbContext_SavedChanges(object sender, Microsoft.EntityFrameworkCore.SavedChangesEventArgs e)
        {
            var z = 0;
        }

        private void ChangeTracker_Tracked(object sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e)
        {
            var t = 0;
        }*/

        /// <summary>
        ///     Initializes a new instance of the <see cref="RestBaseController{TEntity, TInput, TOutput}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The (auto-)mapper.</param>
        /// <param name="validationVisitor">The validation visitor for the entities</param>
        public RestBaseController(DatabaseContext dbContext, IMapper mapper, IValidationVisitor validationVisitor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _validationVisitor = validationVisitor;
        }

        #region Properties

        /// <summary>
        ///     Gets the database context.
        /// </summary>
        /// <value>
        ///     The database context.
        /// </value>
        protected DatabaseContext DatabaseContext => _dbContext;

        /// <summary>
        ///     Gets the mapper.
        /// </summary>
        /// <value>
        ///     The (auto-)mapper.
        /// </value>
        protected IMapper Mapper => _mapper;

        #endregion

        /// <summary>
        ///     Gets all <see cref="TEntity" />s.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public virtual ActionResult<IQueryable<TOutput>> GetAll()
        {
            var t = DatabaseContext.TblPublishedCompanies;
            var te = DatabaseContext.TblUserCompanies;
            var tee = DatabaseContext.TblPublishedSensors;
            var werret = DatabaseContext.TblUserSensors;
            var rewt = DatabaseContext.TblPublishedGateways;
            var wert = DatabaseContext.TblUserGateways;
            var rrrt = DatabaseContext.TblPublishedProjects;
            var rrt = DatabaseContext.TblUserProjects;


            return Ok(DatabaseContext
                .Set<TEntity>()
                .AsNoTracking()
                .ProjectTo<TOutput>(Mapper.ConfigurationProvider));
        }


        /// <summary>
        ///     Gets all <see cref="TEntity" />s with their database data.
        /// </summary>
        /// <returns>The <see cref="TEntity" />s with their database data</returns>
        [HttpGet("dbData")]
        public virtual ActionResult<IQueryable<TOutput>> GetDBDataAll() =>
            Ok(DatabaseContext
                .Set<TEntity>()
                .AsNoTracking());


        // Evntl. muss hier https://github.com/AutoMapper/AutoMapper.Extensions.OData verwendet werden, um den OData-Query von DTO->Entity und das Ergebnis von Entity->DTO mappen zu können.

        /// <summary>
        ///     Gets the <see cref="TEntity" /> with the given id.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <returns>The <see cref="TEntity" />.</returns>
        [HttpGet("{id}")]
        [EnableQuery]
        public virtual ActionResult<IQueryable<TOutput>> Get(long id)
        {
            if (typeof(TEntity) == typeof(TableMeasurementData))
            {
                return Ok(DatabaseContext
                    .Set<TableMeasurementData>()
                    .AsNoTracking()
                    .Where(x => x.MeasurementId == id)
                    .ProjectTo<TOutput>(Mapper.ConfigurationProvider));
            }

            return Ok(DatabaseContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<TOutput>(Mapper.ConfigurationProvider));
        }

        // Evntl. muss hier https://github.com/AutoMapper/AutoMapper.Extensions.OData verwendet werden, um den OData-Query von DTO->Entity und das Ergebnis von Entity->DTO mappen zu können.

        /// <summary>
        ///     Creates an entry by processing the given <see cref="TInput" />.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TInput input)
        {
            // TODO del project because only 1 should be online??

            var entity = Mapper.Map<TInput, TEntity>(input);

            //var t = "";

            //((Project)Convert.ChangeType(input, typeof(Project))).Gateways.ForEach(g => g.Sensors.ForEach(s => t=s.Interfaces.ToJson()));

            try
            {
                // ################# Istvan Galfi ########################
                if (entity is IValidationVisitable visitable)
                {
                    await visitable.AcceptAsync(_validationVisitor);
                }
                // ################# Istvan Galfi ########################

                DatabaseContext.Set<TEntity>().Add(entity);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                // ################# Istvan Galfi ########################
                /// TODO return validationresult; 
                /// Done(?) - If possible, uniting <see cref="IEntity"/> and <see cref="IValidationVisitable"/> would be prefferable in some way over the above...

                /// Tipp: <see cref="StringBuilder"/> and String Interpolation ($"{variable}") are usually considerably faster than => "some Text" + "some other Text" + Variable
                var errorMessageBuilder = new StringBuilder();
                foreach (var error in e.Errors)
                {
                    errorMessageBuilder.AppendLine($"Code: {error.ErrorCode} => Message: {error.ErrorMessage}");
                }

                return BadRequest(errorMessageBuilder.ToString());
                // ################# Istvan Galfi ########################
            }
            catch (DbUpdateException e)
            {
                // ################# Istvan Galfi ########################
                return BadRequest($"Data Collision: {e.InnerException.Message}"); // This is a quick fix... // TODO find a good way to resolve collisions (Id, Name etc.)
                // ################# Istvan Galfi ########################
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            // ################# Istvan Galfi ########################
            var result = Mapper.Map<TEntity, TInput>(entity);
            return CreatedAtAction(nameof(Post), new {entity.Id}, result); // TODO createdResponse // Done
            // ################# Istvan Galfi ########################
        }

        /// <summary>
        ///     Updates the specified entry, by processing the given <see cref="TInput" />.
        /// </summary>
        /// <param name="id">The id of the entry to be updated.</param>
        /// <param name="input">The input data.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(long id, [FromBody] TInput input)
        {
            var entity = await DatabaseContext.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            Mapper.Map(input, entity);

            try
            {
                await DatabaseContext.SaveChangesAsync();
            }
            catch (ValidationException)
            {
                // TODO return validationresult;
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            // ################# Istvan Galfi ########################
            var result = Mapper.Map<TEntity, TInput>(entity);
            return Ok(result);
            // ################# Istvan Galfi ########################
        }

        /// <summary>
        ///     Deletes the entry with the given id.
        /// </summary>
        /// <param name="id">The id of the entry to be deleted.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(long id)
        {
            var entity = await DatabaseContext.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                DatabaseContext.Set<TEntity>().Remove(entity);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}