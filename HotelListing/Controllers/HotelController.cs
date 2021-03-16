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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public HotelController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            var hotels = await _unitOfWork.Hotels.GetPagedList(requestParams);
            var result = _mapper.Map<List<HotelDTO>>(hotels);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = nameof(GetHotel))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { nameof(Hotel.Country) });
            var result = _mapper.Map<HotelDTO>(hotel);
            return Ok(result);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
        {
            _logger.LogInformation("Creating new Hotel!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hotel = _mapper.Map<Hotel>(hotelDTO);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();
            var result = _mapper.Map<HotelDTO>(hotel);
            return CreatedAtRoute(nameof(GetHotel), new { id = hotel.Id }, result);
        }

        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            _logger.LogInformation("Updating Hotel!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Hotel not found {nameof(UpdateHotel)}");
                return NotFound();
            }

            _mapper.Map(hotelDTO, hotel);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();
            return NoContent();
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            _logger.LogInformation("Deleting Hotel!");
            var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError("Hotel not found");
                return NotFound();
            }

            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();
            return Ok();
        }
    }
}
