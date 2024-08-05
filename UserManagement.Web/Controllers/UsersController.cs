using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.Services.DTO;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserLogService _userLogService;
    public UsersController(IUserService userService, IUserLogService userLogService)
    {
        _userService = userService;
        _userLogService = userLogService;
    }  

    [HttpGet]
    [Route("List")]
    public ViewResult List(bool? active = null)
    {
        IEnumerable<User> users;
        if (active.HasValue)
            users = _userService.FilterByActive(active.Value);
        else
            users = _userService.GetAll();
        
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet]
    [Route("ViewUser")]
    public ViewResult ViewUser(int id = 0)
    {
        if (id > 0)
        {
            var user = _userService.GetUser(id);
            var logs = _userLogService.GetLogsForUser(id);
            if (user != null)
            {
                var item = new UserListItemViewModel
                {
                    Id = user.Id,
                    Forename = user.Forename,
                    Surname = user.Surname,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    DateOfBirth = user.DateOfBirth                    
                };
                if (logs != null)
                {
                    item.Logs = new UserLogViewModel()
                    {
                        Items = logs.Select(p => new UserLogItemViewModel
                        {
                            Id = p.Id,
                            UserId = p.UserId,
                            Message = p.Message,
                            Timestamp = p.Timestamp,
                            HasChanges = !string.IsNullOrEmpty(p.Changes)
                        }).ToList()
                    };
                }
                return View(item);
            }
        }
        return View(new UserListItemViewModel());
    }

    [HttpGet]
    [Route("EditUser")]
    public ViewResult EditUser(int id = 0)
    {
        if (id > 0)
        {
            var user = _userService.GetUser(id);
            if (user != null)
            {
                var item = new UserListItemViewModel
                {
                    Id = user.Id,
                    Forename = user.Forename,
                    Surname = user.Surname,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    DateOfBirth = user.DateOfBirth
                };
                return View(item);
            }
        }        
        return View(new UserListItemViewModel());
    }
    [HttpPost]
    [Route("EditUser")]
    [ValidateAntiForgeryToken]
    public JsonResult EditUser(UserListItemViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id > 0)
            {
                _userService.UpdateUser(new UserDto
                {
                    Id = model.Id,
                    Forename = model.Forename.Trim(),
                    Surname = model.Surname.Trim(),
                    Email = model.Email.Trim(),
                    IsActive = model.IsActive,
                    DateOfBirth = model.DateOfBirth
                });
            }            
            else
            {
                _userService.AddUser(new User
                {
                    Id = model.Id,
                    Forename = model.Forename.Trim(),
                    Surname = model.Surname.Trim(),
                    Email = model.Email.Trim(),
                    IsActive = model.IsActive,
                    DateOfBirth = model.DateOfBirth
                });
            }

            return Json(new { ok = true });
        }
        else
        {
            var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { ok = false, errorMessage = string.Join(", ", errorMessages) });
        }
    }

    [HttpPost]
    [Route("DeleteUser")]
    [ValidateAntiForgeryToken]
    public JsonResult DeleteUser(IFormCollection postedInfo)
    {
        long id = 0;
        if (postedInfo.ContainsKey("DeleteUserId"))
            long.TryParse(postedInfo["DeleteUserId"], out id);

        if (id > 0)
        {
            var user = _userService.GetUser(id);
            if (user != null)
            {                
                _userService.DeleteUser(user);
            }

            return Json(new { ok = true });
        }
        else
        {
            return Json(new { ok = false, errorMessage = "An error occurred when deleting the user." });
        }
    }

    [HttpGet]
    [Route("ViewLogs")]
    public ViewResult ViewLogs()
    {
        var logs = _userLogService.GetAll();
        var logItems =
            logs.OrderByDescending(l => l.Timestamp).
            Select(p => new UserLogItemViewModel
        {
            Id = p.Id,
            UserId = p.UserId,
            Message = p.Message,
            Timestamp = p.Timestamp,
            HasChanges = !string.IsNullOrEmpty(p.Changes)
        });

        var model = new UserLogViewModel
        {
            Items = logItems.ToList()
        };
        return View(model);
    }

    [HttpGet]
    [Route("ViewLog")]
    public ViewResult ViewLog(long id)
    {
        var referrer = Request.Headers["Referer"].ToString();
        var log = _userLogService.GetLog(id);
        var changes = _userLogService.GetChangesFromString(log.Changes);
        var model = new UserLogDetailItemViewModel
        {
            Id = id,
            UserId = log.UserId,
            Changes = changes,
            Timestamp = log.Timestamp,
            ReferrerUrl = referrer
        };
        return View(model);
    }
}
