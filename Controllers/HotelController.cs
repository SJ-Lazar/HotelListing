using AutoMapper;
using HotelListing.Data;
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
        #region Private Variables
        private readonly IUnitOfWork _uow;
        private readonly ILogger<HotelController> _log;
        private readonly IMapper _mapper; 
        #endregion

        #region Constructor
        public HotelController(IUnitOfWork uow, ILogger<HotelController> log, IMapper mapper)
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
        [HttpGet("{id:int}", Name ="GetHotel")]
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
        #endregion

        #region HttpPosts
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {

            if (!ModelState.IsValid)
            {
                _log.LogInformation($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDTO);
                await _uow.Hotels.Insert(hotel);
                await _uow.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id }, hotel);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion

        #region HttpPut
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {

            if (!ModelState.IsValid || id < 1)
            {
                _log.LogInformation($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _uow.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _log.LogInformation($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(hotelDTO, hotel);
                _uow.Hotels.Update(hotel);
                await _uow.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Something went Wrong in the {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        #endregion

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        #region HttpDelete
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _log.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }

            try
            {
                var hotel = await _uow.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _log.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _uow.Hotels.Delete(id);
                await _uow.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _log.LogError($"Something went wrong in {nameof(DeleteHotel)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        } 
        #endregion
    }
}
