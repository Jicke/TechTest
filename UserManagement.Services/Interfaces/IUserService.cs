using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Services.DTO;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();
    User GetUser(long id);
    void AddUser(User user);
    void UpdateUser(UserDto user);
    void DeleteUser(User user);
}
