using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class UserController : CustomBaseController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _service.GetUserWithRoleAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _service.GetUsersWithRoleAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto userCreateDto)
        {
            return CustomActionResult(await _service.AddAsync(userCreateDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<User, UserUpdateDto>))]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto userUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsync(userUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
