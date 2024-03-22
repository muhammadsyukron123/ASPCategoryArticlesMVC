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
    public class UserData : IUserData
    {
        private AppDbContext _context;

        public UserData(AppDbContext appDbContext)
        {
            _context = appDbContext;
            
        }
        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.Password = Helpers.Md5Hash.GetHash(newPassword);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var userDelete = await _context.Users.FirstOrDefaultAsync(u => u.Username == id.ToString());
                if (userDelete == null)
                {
                    throw new Exception("User not found");
                }
                _context.Users.Remove(userDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while deleting user", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users == null)
                {
                    throw new Exception("No users found");
                }
                return users;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting users", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllWithRoles()
        {
            try
            {
                var users = await _context.Users.Include(u => u.Roles).ToListAsync();
                if (users == null)
                {
                    throw new Exception("No users found");
                }
                return users;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting users", ex);
            }
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByUsername(string username)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting user", ex);
            }
        }

        public async Task<User> GetUserWithRoles(string username)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while getting user", ex);
            }
        }

        public async Task<User> Insert(User entity)
        {
            try
            {
                var user = await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                return user.Entity;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while inserting user", ex);
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                var hashedPassword = Helpers.Md5Hash.GetHash(password);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == hashedPassword);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while logging in", ex);
            }
        }

        public async Task<User> Update(User entity)
        {
            try
            {
                var user = await GetByUsername(entity.Username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.Address = entity.Address;
                user.Telp = entity.Telp;
                user.SecurityQuestion = entity.SecurityQuestion;
                user.SecurityAnswer = entity.SecurityAnswer;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while updating user", ex);
            }
        }
    }
}
