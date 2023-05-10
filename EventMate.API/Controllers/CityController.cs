using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class CityController : CustomBaseController
    {
        private readonly ICityService _service;
        public CityController(ICityService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _service.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CityCreateDto cityCreateDto)
        {
            return CustomActionResult(await _service.AddAsync(cityCreateDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<City, CityUpdateDto>))]
        [ServiceFilter(typeof(CreatedBySafetyFilter<City, CityUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] CityUpdateDto cityUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(cityUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
