using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRESTServices.Data
{
    public class RoleData : IRoleData
    {
        private AppDbContext _context;

        public RoleData(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
                role.Usernames.Add(user);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while adding user to role", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var roleDelete = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
                if (roleDelete == null)
                {
                    throw new Exception("Role not found");
                }
                _context.Roles.Remove(roleDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while deleting role", ex);
            }
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                if (roles == null)
                {
                    throw new Exception("No roles found");
                }
                return roles;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting roles", ex);
            }
        }

        public async Task<Role> GetById(int id)
        {
            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
                if (role == null)
                {
                    throw new Exception("Role not found");
                }
                return role;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting role by ID", ex);
            }
        }

        public async Task<Role> Insert(Role entity)
        {
            try
            {
                _context.Roles.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while inserting role", ex);
            }
        }

        public async Task<Role> Update(Role entity)
        {
            try
            {
                _context.Roles.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while updating role", ex);
            }
        }
    }
}
