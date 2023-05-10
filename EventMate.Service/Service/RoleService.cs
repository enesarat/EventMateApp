using AutoMapper;
using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.City;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.Model.Concrete;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Service.Service
{
    public class RoleService : GenericService<Role, RoleDto>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IGenericRepository<Role> repository, IUnitOfWork unitOfWork, IMapper mapper, IRoleRepository roleRepository) : base(repository, unitOfWork, mapper)
        {
            _roleRepository = roleRepository;
        }

        public async Task<CustomResponse<NoContentResponse>> AddAsync(RoleCreateDto roleCreateDto)
        {
            if (await RoleVerifier(roleCreateDto.Name))
            {
                return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status400BadRequest, "This role name is registered in the system. Please specify another role name.");
            }
            var item = _mapper.Map<Role>(roleCreateDto);
            item.CreatedDate = DateTime.Now;
            item.CreatedBy = "SYSTEM";
            await _roleRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
        }
        public async Task<bool> RoleVerifier(string name)
        {
            if (await _roleRepository.AnyAsync(x => x.Name == name))
            {
                return true;
            }
            return false;
        }
        public async Task<CustomResponse<NoContentResponse>> UpdateAsync(RoleUpdateDto roleUpdateDto)
        {
            if (await _roleRepository.AnyAsync(x => x.Id == roleUpdateDto.Id && x.IsActive == true))
            {
                var entity = _mapper.Map<Role>(roleUpdateDto);

                entity.UpdatedDate = DateTime.Now;
                _roleRepository.Update(entity);
                await _unitOfWork.CommitAsync();
                return CustomResponse<NoContentResponse>.Success(StatusCodes.Status204NoContent);
            }
            return CustomResponse<NoContentResponse>.Fail(StatusCodes.Status404NotFound, $" {typeof(Category).Name} ({roleUpdateDto.Id}) not found. Updete operation is not successfull. ");
        }
    }
}
