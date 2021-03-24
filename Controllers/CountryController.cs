using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        #region Private Variables
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CountryController> _log;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CountryController(IUnitOfWork uow, ILogger<CountryController> log, IMapper mapper)
        {
            _uow = uow;
            _log = log;
            _mapper = mapper;
        } 
        #endregion

        #region HttpGets

        //Get All
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _uow.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }

        //Get By Id
        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _uow.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion

        #region HttpPosts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {

            if (!ModelState.IsValid)
            {
                _log.LogInformation($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = _mapper.Map<Country>(countryDTO);
                await _uow.Countries.Insert(country);
                await _uow.Save();

                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion

        #region HttpPut
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO CountryDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                _log.LogInformation($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                var country = await _uow.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _log.LogInformation($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(CountryDTO, country);
                _uow.Countries.Update(country);
                await _uow.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion

        #region HttpDelete
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _log.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                return BadRequest();
            }

            try
            {
                var country = await _uow.Countries.Get(q => q.Id == id);
                if (country == null)
                {
                    _log.LogError($"Invalid Delete attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _uow.Countries.Delete(id);
                await _uow.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _log.LogError($"Something went wrong in {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion
    }
}
