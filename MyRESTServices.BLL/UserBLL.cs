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
    public class UserBLL : IUserBLL
    {
        private IUserData _userData;
        private IMapper _mapper;

        public UserBLL(IUserData userData, IMapper mapper)
        {
            _userData = userData;
            _mapper = mapper;

        }
        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            try
            {
                var user = await _userData.GetByUsername(username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                await _userData.ChangePassword(username, newPassword);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the exception or perform any other necessary actions
                throw new Exception("Error occurred while changing password", ex);
            }
        }

        public Task<bool> Delete(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            try
            {
                var users = await _userData.GetAll();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting all users: " + ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            try
            {
                var users = await _userData.GetAllWithRoles();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting all users with roles: " + ex.Message);
                throw;
            }
        }

        public async Task<UserDTO> GetByUsername(string username)
        {
            try
            {
                var user = await _userData.GetByUsername(username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting user by username: " + ex.Message);
                throw;
            }
        }

        public async Task<UserDTO> GetUserWithRoles(string username)
        {
            try
            {
                var user = await _userData.GetUserWithRoles(username);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while getting user with roles: " + ex.Message);
                throw;
            }
        }

        public async Task<Task> Insert(UserCreateDTO entity)
        {
            try
            {
                var user = _mapper.Map<User>(entity);
                await _userData.Insert(user);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while inserting a user: " + ex.Message);
                throw;
            }
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            try
            {
                var user = await _userData.Login(username, password);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while logging in: " + ex.Message);
                throw;
            }
        }

        public async Task<UserDTO> LoginMVC(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userData.Login(loginDTO.Username, loginDTO.Password);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred while logging in: " + ex.Message);
                throw;
            }
        }
    }
}
