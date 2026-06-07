using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models; 
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Models;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeLeaveController : ControllerBase
{
    private readonly EmployeeLeaveDbContext _db;

    public EmployeeLeaveController(EmployeeLeaveDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetLeaveRequests()
    {
        var lst = _db.LeaveRequests
            .Include(r => r.Employee)
            .Include(r => r.LeaveType)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .Select(x => new
            {
                x.Id,
                x.EmployeeId,
                EmployeeName = x.Employee.Name, 
                x.LeaveTypeId,
                LeaveTypeName = x.LeaveType.TypeName,
                x.StartDate,
                x.EndDate,
                LeaveDays = (x.EndDate.Date - x.StartDate.Date).Days + 1, 
                x.Status,
                x.CreatedDateTime,
                x.CreatedBy
            })
            .ToList();

        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetLeaveRequest(int id)
    {
        var item = _db.LeaveRequests
            .Include(r => r.Employee)       
            .Include(r => r.LeaveType)      
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

        if (item is null)
        {
            return NotFound(new { Message = "Leave request not found or has been deleted." });
        }

        var responseData = new EmployeeLeaveModel
        {
            Id = item.Id,
            EmployeeId = item.EmployeeId,
            LeaveTypeId = item.LeaveTypeId,
            StartDate = item.StartDate,
            EndDate = item.EndDate,
            Reason = item.Reason,
            Status = item.Status,
            CreatedDateTime = item.CreatedDateTime,
            CreatedBy = item.CreatedBy,
            ModifiedDateTime = item.ModifiedDateTime,
            ModifiedBy = item.ModifiedBy
        };

        return Ok(responseData);
    }

    [HttpPost]
    public IActionResult RequestLeave(LeaveCreateRequestModel requestModel)
    {
        var employee = _db.Employees.FirstOrDefault(e => e.Id == requestModel.EmployeeId && !e.IsDeleted);
        var leaveType = _db.LeaveTypes.FirstOrDefault(l => l.Id == requestModel.LeaveTypeId && !l.IsDeleted);

        if (employee is null || leaveType is null)
        {
            return BadRequest(new LeaveCreateResponseModel { IsSuccess = false, Message = "Employee or Leave Type not found." });
        }

        int leaveDays = (requestModel.EndDate.Date - requestModel.StartDate.Date).Days + 1;
        if (leaveDays <= 0)
        {
            return BadRequest(new LeaveCreateResponseModel { IsSuccess = false, Message = "Invalid Date Range." });
        }

        if (employee.TotalLeaveBalance < leaveDays)
        {
            return BadRequest(new LeaveCreateResponseModel
            {
                IsSuccess = false,
                Message = $"Insufficient leave balance. You only have {employee.TotalLeaveBalance} days left."
            });
        }

        _db.LeaveRequests.Add(new LeaveRequest
        {
            EmployeeId = requestModel.EmployeeId,
            LeaveTypeId = requestModel.LeaveTypeId,
            StartDate = requestModel.StartDate,
            EndDate = requestModel.EndDate,
            Reason = requestModel.Reason ?? string.Empty,
            Status = "Pending",
            CreatedDateTime = DateTime.Now,
            CreatedBy = employee.Name, 
            IsDeleted = false
        });

        int result = _db.SaveChanges();

        return StatusCode(201, new LeaveCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Leave Request Submitted Successfully" : "Submission Failed"
        });
    }

    [HttpPatch("{id}/approve")]
    public IActionResult ApproveLeave(int id, LeaveApproveRequestModel request)
    {
        if (request.Status != "Approved" && request.Status != "Rejected")
        {
            return BadRequest(new { Message = "Status must be 'Approved' or 'Rejected'." });
        }

        using var transaction = _db.Database.BeginTransaction();

        try
        {
            var item = _db.LeaveRequests.Include(r => r.Employee).FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (item is null)
            {
                return NotFound(new LeaveApproveResponseModel { IsSuccess = false, Message = "Leave request not found" });
            }

            if (item.Status != "Pending")
            {
                return BadRequest(new LeaveApproveResponseModel { IsSuccess = false, Message = "This request has already been processed" });
            }

            item.Status = request.Status;
            item.ModifiedDateTime = DateTime.Now;
            item.ModifiedBy = request.ManagerName;

            int leaveDays = (item.EndDate.Date - item.StartDate.Date).Days + 1;

            if (request.Status == "Approved")
            {
                if (item.Employee.TotalLeaveBalance < leaveDays)
                {
                    return BadRequest(new LeaveApproveResponseModel { IsSuccess = false, Message = "Employee does not have enough leave balance anymore." });
                }
                item.Employee.TotalLeaveBalance -= leaveDays;
                item.Employee.ModifiedDateTime = DateTime.Now;
                item.Employee.ModifiedBy = request.ManagerName;
            }

            _db.LeaveApprovals.Add(new LeaveApproval
            {
                LeaveRequestId = item.Id,
                ApprovedBy = request.ManagerName ?? "Manager",
                ActionDate = DateTime.Now,
                Remarks = request.Remarks,
                CreatedDateTime = DateTime.Now,
                CreatedBy = request.ManagerName ?? "Manager",
                IsDeleted = false
            });

            _db.SaveChanges();
            transaction.Commit(); 

            return Ok(new LeaveApproveResponseModel
            {
                IsSuccess = true,
                Message = $"Leave request has been {request.Status} successfully.",
                Data = new EmployeeLeaveModel
                {
                    Id = item.Id,
                    EmployeeId = item.EmployeeId,
                    LeaveTypeId = item.LeaveTypeId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Reason = item.Reason,
                    Status = item.Status,
                    CreatedDateTime = item.CreatedDateTime,
                    CreatedBy = item.CreatedBy,
                    ModifiedDateTime = item.ModifiedDateTime,
                    ModifiedBy = item.ModifiedBy
                }
            });
        }
        catch (Exception)
        {
            transaction.Rollback(); 
            return StatusCode(500, new LeaveApproveResponseModel { IsSuccess = false, Message = "An error occurred during database transaction." });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLeaveRequest(int id, [FromQuery] string deletedBy)
    {
        var item = _db.LeaveRequests.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new LeaveDeleteResponseModel
            {
                IsSuccess = false,
                Message = "Leave request not found or already deleted"
            });
        }

        item.IsDeleted = true;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = deletedBy; 

        int result = _db.SaveChanges();

        return Ok(new LeaveDeleteResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Soft Delete Successfully" : "Soft Delete Failed"
        });
    }
}