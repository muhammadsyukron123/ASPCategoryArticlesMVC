
using MyRESTServices.BLL.DTOs;
using System.Collections.Generic;

namespace MyRESTServices.BLL.Interfaces
{
    public interface IRoleBLL
    {
        Task<IEnumerable<RoleDTO>> GetAllRoles();
        Task<RoleDTO> AddRole(string roleName);
        Task<Task> AddUserToRole(string username, int roleId);
    }
}
