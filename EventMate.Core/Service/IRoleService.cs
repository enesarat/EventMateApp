﻿using EventMate.Core.DTO.Concrete.Category;
using EventMate.Core.DTO.Concrete.Response;
using EventMate.Core.DTO.Concrete.Role;
using EventMate.Core.Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Core.Service
{
    public interface IRoleService : IGenericService<Role,RoleDto>
    {
        public Task<CustomResponse<NoContentResponse>> AddAsync(RoleCreateDto roleCreateDto);
        public Task<CustomResponse<NoContentResponse>> UpdateAsync(RoleUpdateDto roleUpdateDto);
    }
}
