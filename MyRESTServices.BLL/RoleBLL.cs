using AutoMapper;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.BLL
{
    public class RoleBLL : IRoleBLL
    {
        private IRoleData _roleData;
        private IMapper _mapper;

        public RoleBLL(IRoleData roleData, IMapper mapper)
        {
            _roleData = roleData;
            _mapper = mapper;
        }
        public async Task<RoleDTO> AddRole(string roleName)
        {
            try
            {
                var roleModel = new Role
                {
                    RoleName = roleName
                };
                var role = await _roleData.Insert(roleModel);
                return _mapper.Map<RoleDTO>(role);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while adding role: " + ex.Message);
                throw;
            }
        }

        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            try
            {
                var user = await _roleData.AddUserToRole(username, roleId);
                return user;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while adding user to role: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRoles()
        {
            try
            {
                var roles = await _roleData.GetAll();
                return _mapper.Map<IEnumerable<RoleDTO>>(roles);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting all roles: " + ex.Message);
                throw;
            }
        }
    }
}
