using AutoMapper;
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
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CountryController> _log;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork uow, ILogger<CountryController> log, IMapper mapper)
        {
            _uow = uow;
            _log = log;
            _mapper = mapper;
        }

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
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                var country = await _uow.Countries.Get(q => q.Id == id, new List<string> {"Hotels"});
                var result = _mapper.Map<CountryDTO>(country);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }



    }
}
