using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Services.DTO;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserLogService 
{
    IEnumerable<UserLog> GetAll();
    List<UserLog> GetLogsForUser(long id);
    UserLog GetLog(long id);
    void AddLog(UserLog log);
    List<UserLogChanges> GetLogChanges(User oldDetails, UserDto newDetails);
    string GetChangesAsString(List<UserLogChanges> changes);
    List<UserLogChanges> GetChangesFromString(string changes);
}
