using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Models;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController : ControllerBase
{
    private readonly EmployeeLeaveDbContext _db;

    public LeaveTypesController(EmployeeLeaveDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetLeaveTypes()
    {
        var lst = _db.LeaveTypes.Where(l => !l.IsDeleted).ToList();
        var responseData = lst.Select(l => new LeaveTypeModel
        {
            Id = l.Id,
            TypeName = l.TypeName,
            MaxDaysAllowed = l.MaxDaysAllowed
        }).ToList();
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public IActionResult GetLeaveType(int id)
    {
        var item = _db.LeaveTypes.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound();
        var responseData = new LeaveTypeModel
        {
            Id = item.Id,
            TypeName = item.TypeName,
            MaxDaysAllowed = item.MaxDaysAllowed
        };
        return Ok(responseData);
    }

    [HttpPost]
    public IActionResult CreateLeaveType(LeaveTypeCreateRequestModel request)
    {
        _db.LeaveTypes.Add(new LeaveType
        {
            TypeName = request.TypeName,
            MaxDaysAllowed = request.MaxDaysAllowed,
            CreatedDateTime = DateTime.Now,
            CreatedBy = request.CreatedBy ?? "Admin",
            IsDeleted = false
        });
        int result = _db.SaveChanges();

        return StatusCode(201, new LeaveTypeCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Leave Type Created Successfully" : "Creation Failed"
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLeaveType(int id, LeaveTypeUpdateRequestModel request)
    {
        var item = _db.LeaveTypes.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new LeaveTypeUpdateResponseModel { IsSuccess = false, Message = "Not found" });

        item.TypeName = request.TypeName;
        item.MaxDaysAllowed = request.MaxDaysAllowed;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();
        return Ok(new LeaveTypeUpdateResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Updated Successfully" : "Failed" });
    }
    [HttpPatch("{id}")]
    public IActionResult PatchLeaveType(int id, LeaveTypePatchRequestModel request)
    {
        var item = _db.LeaveTypes.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new LeaveTypePatchResponseModel { IsSuccess = false, Message = "Leave Type not found" });
        }

        int count = 0;

        if (!string.IsNullOrEmpty(request.TypeName))
        {
            item.TypeName = request.TypeName;
            count++;
        }

        if (request.MaxDaysAllowed.HasValue)
        {
            item.MaxDaysAllowed = request.MaxDaysAllowed.Value;
            count++;
        }

        if (count == 0)
        {
            return BadRequest(new LeaveTypePatchResponseModel { IsSuccess = false, Message = "No fields provided to update" });
        }

        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();

        return Ok(new LeaveTypePatchResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Leave Type Patched Successfully" : "Patch Failed",
            Data = new LeaveTypeModel
            {
                Id = item.Id,
                TypeName = item.TypeName,
                MaxDaysAllowed = item.MaxDaysAllowed
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult SoftDeleteLeaveType(int id, [FromQuery] string deletedBy)
    {
        var item = _db.LeaveTypes.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new LeaveTypeDeleteResponseModel { IsSuccess = false, Message = "Not found" });

        item.IsDeleted = true;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = deletedBy;

        int result = _db.SaveChanges();
        return Ok(new LeaveTypeDeleteResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Soft Deleted Successfully" : "Failed" });
    }
}