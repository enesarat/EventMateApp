using AutoMapper;
using BCrypt.Net;
using EventMate.Core.DTO.Concrete.Account;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.DTO.Concrete.Token;
using EventMate.Core.DTO.Concrete.User;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Model.Token;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class AccountService : GenericService<User, UserDto>, IAccountService
    {
        private readonly IUserRepository _userRepository;
        private IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        public AccountService(IGenericRepository<User> repository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository, IConfiguration config, IHttpContextAccessor contextAccessor) : base(repository, unitOfWork, mapper)
        {
            _userRepository = userRepository;
            _config = config;
            _contextAccessor = contextAccessor;
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsAdminAsync(UserUpdateAsAdminDto userUpdateDto)
        {
            if (await _userRepository.AnyAsync(x => x.Id == userUpdateDto.Id && x.IsActive == true))
            {
                if (await EmailVerifierAsync(userUpdateDto.Email))
                {
                    return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, "The e-mail address is registered in the system. Please try another e-mail address.");
                }
                var entity = _mapper.Map<User>(userUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _userRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(User).Name} ({userUpdateDto.Id}) not found. Updete operation is not successfull. ");

        }
        private string PasswordHasher(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool PasswordVerifier(string inputPassword, string currentPassword)
        {
            if(BCrypt.Net.BCrypt.Verify(inputPassword, currentPassword))
            {
                return true;
            }
            return false;
        }
        private async Task<bool> EmailVerifierAsync(string email)
        {
            if(await _userRepository.AnyAsync(x => x.Email == email))
            {
                return true;
            }
            return false;
        }
        public async Task<CustomResponse<NoContentResponse>> AddAdminAsync(UserCreateDto dto)
        {
            if (await EmailVerifierAsync(dto.Email))
            {
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, "The e-mail address is registered in the system. Please try another e-mail address.");
            }
            var hashedPassword = PasswordHasher(dto.Password);
            var newEntity = _mapper.Map<User>(dto);
            var currentAccount = GetCurrentAccount();
            newEntity.CreatedBy = currentAccount.Result.Email;
            newEntity.Password = hashedPassword;
            newEntity.CreatedDate = DateTime.Now;
            newEntity.RoleId = 1;
            await _userRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var refObj = _unitOfWork.RoleRepository.Where(x => x.Id == newEntity.RoleId).FirstOrDefault();
            var newDto = _mapper.Map<UserDto>(newEntity);
            newDto.Role = refObj.Name;
            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }

        public async Task<CustomResponse<NoContentResponse>> AddParticipantAsync(UserCreateDto dto)
        {
            if (await EmailVerifierAsync(dto.Email))
            {
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, "The e-mail address is registered in the system. Please try another e-mail address.");
            }
            var hashedPassword = PasswordHasher(dto.Password);
            var newEntity = _mapper.Map<User>(dto);
            var currentAccount = GetCurrentAccount();
            newEntity.CreatedBy = currentAccount.Result.Email;
            newEntity.Password = hashedPassword;
            newEntity.CreatedDate = DateTime.Now;
            newEntity.RoleId = 3;
            await _userRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var refObj = _unitOfWork.RoleRepository.Where(x => x.Id == newEntity.RoleId).FirstOrDefault();
            var newDto = _mapper.Map<UserDto>(newEntity);
            newDto.Role = refObj.Name;
            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<CustomResponse<NoContentResponse>> AddPersonnelAsync(UserCreateDto dto)
        {
            if (await EmailVerifierAsync(dto.Email))
            {
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, "The e-mail address is registered in the system. Please try another e-mail address.");
            }
            var hashedPassword = PasswordHasher(dto.Password);
            var newEntity = _mapper.Map<User>(dto);
            var currentAccount = GetCurrentAccount();
            newEntity.CreatedBy = currentAccount.Result.Email;
            newEntity.Password = hashedPassword;
            newEntity.CreatedDate= DateTime.Now;
            newEntity.RoleId = 2;
            await _userRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var refObj = _unitOfWork.RoleRepository.Where(x => x.Id == newEntity.RoleId).FirstOrDefault();
            var newDto = _mapper.Map<UserDto>(newEntity);
            newDto.Role = refObj.Name;
            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }
        public UserDto Authenticate(TokenRequest userLogin)
        {

            var currentAccount = _userRepository.Where(o => o.Email.ToLower() == userLogin.Email.ToLower()).FirstOrDefault();

            var isValidPassword = PasswordVerifier(userLogin.Password, currentAccount.Password);

            if (currentAccount != null && isValidPassword == true)
            {
                var refObj = _unitOfWork.RoleRepository.Where(x => x.Id == currentAccount.RoleId).FirstOrDefault();

                var accountDto = _mapper.Map<UserDto>(currentAccount);
                accountDto.Role = refObj.Name;

                return accountDto;
            }

            return null;
        }

        public TokenDto GenerateToken(UserDto user)
        {
            TokenDto tokenModel = new TokenDto();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role),
            };

            tokenModel.Expiration = DateTime.Now.AddMinutes(5);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: tokenModel.Expiration,
            signingCredentials: credentials);

            tokenModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            tokenModel.RefreshToken = CreateRefreshToken();

            return tokenModel;
        }
        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        public async Task<ActiveAccountDto> GetCurrentAccount()
        {
            var identity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                ActiveAccountDto currentaccount = new ActiveAccountDto
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value

                };
                return currentaccount;
            }
            else
                throw new InvalidOperationException("Could not access active user information.");
        }

        public async Task<TokenDto> Login(TokenRequest userLogin)
        {
            var userDto = Authenticate(userLogin);
            if (userDto != null)
            {
                var user = _userRepository.Where(x => x.Id == userDto.Id).FirstOrDefault();
                var token = GenerateToken(userDto);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(3);
                user.UpdatedDate = DateTime.Now;
                _userRepository.Update(user);
                _unitOfWork.Commit();
                return token;
            }
            else
                throw new InvalidOperationException("Email or password is invalid.");
        }

        public async Task<TokenDto> RefreshToken(string tokenStr)
        {
            var account = _userRepository.Where(o => o.RefreshToken == tokenStr).FirstOrDefault();
            if (account is null)
            {
                throw new InvalidOperationException("No account found matching the Refresh token");
            }

            var currentAccount = _userRepository.Where(o => o.RefreshTokenExpireDate > DateTime.Now).FirstOrDefault();
            var role = _unitOfWork.RoleRepository.Where(x => x.Id == currentAccount.RoleId).FirstOrDefault();
            var accountDto = _mapper.Map<UserDto>(currentAccount);
            accountDto.Role = role.Name;
            if (currentAccount is not null)
            {
                TokenDto token = GenerateToken(accountDto);
                currentAccount.RefreshToken = token.RefreshToken;
                currentAccount.RefreshTokenExpireDate = DateTime.Now.AddMinutes(3);
                _userRepository.Update(currentAccount);
                _unitOfWork.Commit();

                return token;
            }
            else
                throw new InvalidOperationException("No Valid refresh tokens were found.");
        }

        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            if (await _userRepository.AnyAsync(x => x.Id == userUpdateDto.Id && x.IsActive == true))
            {
                var entity = _mapper.Map<User>(userUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _userRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(User).Name} ({userUpdateDto.Id}) not found. Updete operation is not successfull. ");
        }

        public async Task<CustomResponse<NoContentResponse>> Logout()
        {
            var identity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                string accountEmail = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
                var user = _userRepository.Where(x => x.Email == accountEmail).FirstOrDefault();

                //var user = _userRepository.Where(x => x.RefreshToken == refreshToken).FirstOrDefault();
                if (user != null && user.RefreshToken != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpireDate = null;
                    user.LastActivity = DateTime.Now;
                    _userRepository.Update(user);
                    await _unitOfWork.CommitAsync();

                    var claimTypesToDelete = new List<string>
                {
                    ClaimTypes.Email,
                    ClaimTypes.Name,
                    ClaimTypes.Surname,
                    ClaimTypes.Role
                };



                    foreach (var claim in identity.Claims.ToList())
                    {
                        identity.RemoveClaim(claim);
                    }
                    await _contextAccessor.HttpContext.SignOutAsync();

                    return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
                }
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, "  No active session found. ");

        }
    }
}
