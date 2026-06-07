using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Database.Models; 
using NLADotNetInternshipTraining.EmployeeLeaveSystem.Models;

namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveApprovalsController : ControllerBase
{
    private readonly EmployeeLeaveDbContext _db;

    public LeaveApprovalsController(EmployeeLeaveDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetApprovalLogs()
    {
        var lst = _db.LeaveApprovals.Where(a => !a.IsDeleted).OrderByDescending(a => a.Id).ToList();
        return Ok(lst);
    }

    [HttpGet("request/{requestId}")]
    public IActionResult GetApprovalByRequestId(int requestId)
    {
        var item = _db.LeaveApprovals.FirstOrDefault(a => a.LeaveRequestId == requestId && !a.IsDeleted);
        if (item is null)
        {
            return NotFound(new { Message = "No approval record found for this request." });
        }
        return Ok(item);
    }
}