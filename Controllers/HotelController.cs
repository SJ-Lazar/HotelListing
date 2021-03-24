using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<HotelController> _log;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork uow, ILogger<HotelController> log, IMapper mapper)
        {
            _uow = uow;
            _log = log;
            _mapper = mapper;
        }

        //Get All
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _uow.Hotels.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(hotels);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }

        //Get By Id
        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]       
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _uow.Hotels.Get(q => q.Id == id);
                var result = _mapper.Map<HotelDTO>(hotel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
    }
}
