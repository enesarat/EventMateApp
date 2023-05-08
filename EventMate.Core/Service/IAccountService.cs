using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Token;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Model.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface IAccountService : IGenericService<User, UserDto>
    {
        Task<CustomResponse<NoContentResponse>> UpdateAsync(UserUpdateDto dto);
        Task<CustomResponse<UserDto>> AddParticipantAsync(UserCreateDto dto);
        Task<CustomResponse<UserDto>> AddPersonnelAsync(UserCreateDto dto);
        Task<CustomResponse<UserDto>> AddAdminAsync(UserCreateDto dto);
        Task<CustomResponse<UserDto>> GetByIdAsync(int id);
        UserDto Authenticate(TokenRequest userLogin);
        TokenDto GenerateToken(UserDto user);
        Task<TokenDto> Login(TokenRequest userLogin);
        Task<TokenDto> RefreshToken(string tokenStr);
        Task<UserDto> GetCurrentAccount();
    }
}
