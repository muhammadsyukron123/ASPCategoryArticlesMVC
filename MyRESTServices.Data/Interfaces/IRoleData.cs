

using MyRESTServices.Domain;

namespace MyRESTServices.Data.Interfaces
{
    public interface IRoleData : ICrudData<Role>
    {
        Task<Task> AddUserToRole(string username, int roleId);
    }
}
