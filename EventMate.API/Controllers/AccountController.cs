using AutoMapper;
using EventMate.API.Filters;
using EventMate.Core.DTO.Concrete.Account;
using EventMate.Core.DTO.Concrete.Event;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Model.Token;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMate.API.Controllers
{

    public class AccountController : CustomBaseController
    {
        private readonly IAccountService _service;
        private readonly IUserService _userService;

        public AccountController(IAccountService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("connect/token")]
        public async Task<IActionResult> Login([FromBody] TokenRequest userLogin)
        {
            return Ok(await _service.Login(userLogin));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return CustomActionResult(await _service.Logout());
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            return Ok(await _service.RefreshToken(token));
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentAccount()
        {
            return CustomActionResult(CustomResponse<ActiveAccountDto>.Success(StatusCodes.Status200OK, await _service.GetCurrentAccount()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CustomActionResult(await _userService.GetUserWithRoleAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomActionResult(await _userService.GetUsersWithRoleAsync());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateParticipant(UserCreateDto participantRoleDto)
        {
            return CustomActionResult(await _service.AddParticipantAsync(participantRoleDto));
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreatePersonnel(UserCreateDto personnelRoleDto)
        {
            return CustomActionResult(await _service.AddPersonnelAsync(personnelRoleDto));
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(UserCreateDto adminRoleDto)
        {
            return CustomActionResult(await _service.AddAdminAsync(adminRoleDto));
        }

        [HttpPut]
        [ServiceFilter(typeof(CreateDateSafetyFilter<User, UserUpdateAsAdminDto>))]
        public async Task<IActionResult> UpdateAsAdmin([FromBody] UserUpdateAsAdminDto userUpdateDto)
        {
            return CustomActionResult(await _service.UpdateAsAdminAsync(userUpdateDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CustomActionResult(await _service.DeleteAsync(id));
        }
    }
}
