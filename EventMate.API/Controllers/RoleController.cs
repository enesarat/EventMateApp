using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class RoleController : CustomBaseController
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
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
        public async Task<IActionResult> Create(RoleCreateDto roleCreateDto)
        {
            return CustomActionResult(await _service.AddAsync(roleCreateDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<Role, RoleUpdateDto>))]
        [ServiceFilter(typeof(CreatedBySafetyFilter<Role, RoleUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto roleUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(roleUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
