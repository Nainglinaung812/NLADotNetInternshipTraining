using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Models;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeLeaveDbContext _db;

    public EmployeesController(EmployeeLeaveDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetEmployees()
    {
        var lst = _db.Employees.Where(e => !e.IsDeleted).OrderByDescending(e => e.Id).ToList();
        var responseData = lst.Select(e => new EmployeeModel
        {
            Id = e.Id,
            Name = e.Name,
            Department = e.Department,
            JoinDate = e.JoinDate,
            TotalLeaveBalance = e.TotalLeaveBalance
        }).ToList();
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployee(int id)
    {
        var item = _db.Employees.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound();
        }
        var responseData = new EmployeeModel
        {
            Id = item.Id,
            Name = item.Name,
            Department = item.Department,
            JoinDate = item.JoinDate,
            TotalLeaveBalance = item.TotalLeaveBalance
        };
        return Ok(responseData);
    }


    [HttpPost]
    public IActionResult CreateEmployee(EmployeeCreateRequestModel request)
    {
        _db.Employees.Add(new Employee
        {
            Name = request.Name,
            Department = request.Department,
            JoinDate = request.JoinDate,
            TotalLeaveBalance = request.TotalLeaveBalance,
            CreatedDateTime = DateTime.Now,
            CreatedBy = request.CreatedBy ?? "Admin",
            IsDeleted = false
        });
        int result = _db.SaveChanges();

        return StatusCode(201, new EmployeeCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Employee Created Successfully" : "Creation Failed"
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEmployee(int id, EmployeeUpdateRequestModel request)
    {
        var item = _db.Employees.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new EmployeeUpdateResponseModel { IsSuccess = false, Message = "Employee not found" });
        }

        item.Name = request.Name;
        item.Department = request.Department;
        item.TotalLeaveBalance = request.TotalLeaveBalance;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();

        return Ok(new EmployeeUpdateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Employee Updated Successfully" : "Update Failed"
        });
    }
    [HttpPatch("{id}")]
    public IActionResult PatchEmployee(int id, EmployeePatchRequestModel request)
    {
        var item = _db.Employees.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new EmployeePatchResponseModel { IsSuccess = false, Message = "Employee not found" });
        }

        int count = 0;

        if (!string.IsNullOrEmpty(request.Name))
        {
            item.Name = request.Name;
            count++;
        }
        if (!string.IsNullOrEmpty(request.Department))
        {
            item.Department = request.Department;
            count++;
        }

        if (request.TotalLeaveBalance.HasValue)
        {
            item.TotalLeaveBalance = request.TotalLeaveBalance.Value;
            count++;
        }

        if (count == 0)
        {
            return BadRequest(new EmployeePatchResponseModel { IsSuccess = false, Message = "No fields provided to update" });
        }

        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();

        return Ok(new EmployeePatchResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Employee Patched Successfully" : "Patch Failed",
            Data = new EmployeePatchDataModel
            {
                Id = item.Id,
                Name = item.Name,
                Department = item.Department,
                TotalLeaveBalance = item.TotalLeaveBalance
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult SoftDeleteEmployee(int id, [FromQuery] string deletedBy)
    {
        var item = _db.Employees.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new EmployeeDeleteResponseModel { IsSuccess = false, Message = "Employee not found" });
        }

        item.IsDeleted = true;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = deletedBy;

        int result = _db.SaveChanges();
        return Ok(new EmployeeDeleteResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Employee Soft Deleted Successfully" : "Delete Failed"
        });
    }
}