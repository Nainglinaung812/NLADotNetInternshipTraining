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
        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetLeaveType(int id)
    {
        var item = _db.LeaveTypes.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound();
        return Ok(item);
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