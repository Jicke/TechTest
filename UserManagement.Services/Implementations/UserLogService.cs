using System.Collections.Generic;
using System.Text.Json;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.DTO;

namespace UserManagement.Services.Domain.Implementations;

public class UserLogService : IUserLogService
{
    private readonly IDataContext _dataAccess;
    public UserLogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public IEnumerable<UserLog> GetAll() => _dataAccess.GetAll<UserLog>();
    public List<UserLog> GetLogsForUser(long id)
    {
        var logs = new List<UserLog>();
        foreach (var log in _dataAccess.GetUserLogs(id))
        {
            logs.Add(log);
        }
        return logs;
    }
    public void AddLog(UserLog log)
    {
        _dataAccess.Create(log);
    }

    public UserLog GetLog(long id)
    {
        var log = _dataAccess.Find<UserLog>(id);
        if (log == null)
            return new UserLog();

        return log;
    }


    public string GetChangesAsString(List<UserLogChanges> changes)
    {
        return JsonSerializer.Serialize(changes);
    }

    public List<UserLogChanges> GetChangesFromString(string changes)
    {
        try
        {
            var userLogChanges = JsonSerializer.Deserialize<List<UserLogChanges>>(changes);
            if (userLogChanges != null)
                return userLogChanges;            
        } catch
        {
            return new List<UserLogChanges>();
        }
        return new List<UserLogChanges>();
    }

    public List<UserLogChanges> GetLogChanges(User oldDetails, UserDto newDetails)
    {
        var changes = new List<UserLogChanges>();
        if (oldDetails.Id == 0 || oldDetails.Id != newDetails.Id)
        {
            return changes;
        }
        if (oldDetails.Forename != newDetails.Forename)
        {
            changes.Add(new UserLogChanges
            {
                PropertyName = "Forename",
                OldValue = oldDetails.Forename,
                NewValue = newDetails.Forename
            });
        }
        if (oldDetails.Surname != newDetails.Surname)
        {
            changes.Add(new UserLogChanges
            {
                PropertyName = "Surname",
                OldValue = oldDetails.Surname,
                NewValue = newDetails.Surname
            });
        }
        if (oldDetails.Email != newDetails.Email)
        {
            changes.Add(new UserLogChanges
            {
                PropertyName = "Email",
                OldValue = oldDetails.Email,
                NewValue = newDetails.Email
            });
        }
        if (oldDetails.IsActive != newDetails.IsActive)
        {
            changes.Add(new UserLogChanges
            {
                PropertyName = "Account Active",
                OldValue = oldDetails.IsActive.ToString(),
                NewValue = newDetails.IsActive.ToString()
            });
        }
        if (oldDetails.DateOfBirth != newDetails.DateOfBirth)
        {
            changes.Add(new UserLogChanges
            {
                PropertyName = "Date of Birth",
                OldValue = oldDetails.DateOfBirth.ToString(),
                NewValue = newDetails.DateOfBirth.ToString()
            });
        }
        return changes;
    }


}
