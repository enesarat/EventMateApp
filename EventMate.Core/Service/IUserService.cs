using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface IUserService : IGenericService<User,UserDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(UserCreateDto userCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(UserUpdateDto userUpdateDto);
        public Task<CustomResponse<IEnumerable<UserDto>>> GetUsersWithRoleAsync();
        public Task<CustomResponse<UserDto>> GetUserWithRoleAsync(int id);

    }
}
