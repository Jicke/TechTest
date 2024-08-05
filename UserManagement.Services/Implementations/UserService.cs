using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.DTO;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    private readonly IUserLogService _userLogService;
    public UserService(IDataContext dataAccess, IUserLogService userLogService)
    {
        _dataAccess = dataAccess;
        _userLogService = userLogService;
    }

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive) => GetAll().Where(u => u.IsActive == isActive);
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public User GetUser(long id)
    {
        var user = _dataAccess.Find<User>(id);
        if (user == null)
            return new User();
        return user;        
    }

    public void AddUser(User user)
    {
        _dataAccess.Create(user);
        LogAdd(user.Id);
    }
    public void UpdateUser(UserDto user)
    {
        var userToUpdate = GetUser(user.Id);
        LogUpdate(userToUpdate, user);
        userToUpdate.Forename = user.Forename;
        userToUpdate.Surname = user.Surname;
        userToUpdate.Email = user.Email;
        userToUpdate.IsActive = user.IsActive;
        userToUpdate.DateOfBirth = user.DateOfBirth;
        _dataAccess.Update(userToUpdate);
    }
    public void DeleteUser(User user)
    {
        LogDelete(user.Id);
        _dataAccess.Delete(user);
    }
    private void LogUpdate(User oldUser, UserDto newUser)
    {
        var changes = _userLogService.GetLogChanges(oldUser, newUser);
        var changesString = _userLogService.GetChangesAsString(changes);
        _userLogService.AddLog(new UserLog
        {
            UserId = oldUser.Id,
            Message = "User updated",
            Changes = changesString,
            Timestamp = DateTime.Now
        });
    }
    private void LogDelete(long userId)
    {
        _userLogService.AddLog(new UserLog
        {
            UserId = userId,
            Message = "User deleted",
            Changes = string.Empty,
            Timestamp = DateTime.Now
        });
    }
    private void LogAdd(long userId)
    {
        _userLogService.AddLog(new UserLog
        {
            UserId = userId,
            Message = "User created",
            Changes = string.Empty,
            Timestamp = DateTime.Now
        });        
    }

}
