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
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
        {
            var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
            var result = _mapper.Map<List<CountryDTO>>(countries);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = nameof(GetCountry))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { nameof(Country.Hotels) });
            var result = _mapper.Map<CountryDTO>(country);
            return Ok(result);
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            _logger.LogInformation("Creating new Country!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = _mapper.Map<Country>(countryDTO);
            await _unitOfWork.Countries.Insert(country);
            await _unitOfWork.Save();
            var result = _mapper.Map<CountryDTO>(country);
            return CreatedAtRoute(nameof(countryDTO), new { id = country.Id }, result);
        }

        //[Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            _logger.LogInformation("Updating Country!");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError($"country not found {nameof(UpdateCountry)}");
                return NotFound();
            }

            _mapper.Map(countryDTO, country);
            _unitOfWork.Countries.Update(country);
            await _unitOfWork.Save();
            return NoContent();
        }

        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            _logger.LogInformation("Deleting Country!");
            var country = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (country == null)
            {
                _logger.LogError("Country not found");
                return NotFound();
            }

            await _unitOfWork.Countries.Delete(id);
            await _unitOfWork.Save();
            return Ok();
        }
    }
}
