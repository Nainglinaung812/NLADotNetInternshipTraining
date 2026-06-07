using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Database.Models;
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Models;

namespace NLADotNetInternshipTraining.HospitalAppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly HospitalAppointmentDbContext _db;

    public PatientsController(HospitalAppointmentDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetPatients()
    {
        var lst = _db.Patients.Where(p => !p.IsDeleted).OrderByDescending(p => p.Id).ToList();
        var responseData = lst.Select(p => new PatientModel
        {
            Id = p.Id,
            Name = p.Name,
            Phone = p.Phone
        }).ToList();
        return Ok(responseData);
    }

    [HttpGet("{id}")]
    public IActionResult GetPatient(int id)
    {
        var item = _db.Patients.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound();
        PatientModel responseData = new PatientModel
        {
            Id = item.Id,
            Name = item.Name,
            Phone = item.Phone
        };
        return Ok(responseData);
    }

    [HttpPost]
    public IActionResult CreatePatient(PatientCreateRequestModel request)
    {
        _db.Patients.Add(new Patient
        {
            Name = request.Name,
            Phone = request.Phone,
            CreatedDateTime = DateTime.Now,
            CreatedBy = request.CreatedBy ?? "Admin",
            IsDeleted = false
        });
        int result = _db.SaveChanges();

        return StatusCode(201, new PatientCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Patient Registered Successfully" : "Registration Failed"
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePatient(int id, PatientUpdateRequestModel request)
    {
        var item = _db.Patients.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new PatientUpdateResponseModel { IsSuccess = false, Message = "Patient not found" });

        item.Name = request.Name;
        item.Phone = request.Phone;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();
        return Ok(new PatientUpdateResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Patient Updated Successfully" : "Update Failed" });
    }

    [HttpPatch("{id}")]
    public IActionResult PatchPatient(int id, PatientPatchRequestModel request)
    {
        var item = _db.Patients.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new PatientPatchResponseModel { IsSuccess = false, Message = "Patient not found" });
        }

        int count = 0;

        if (!string.IsNullOrEmpty(request.Name))
        {
            item.Name = request.Name;
            count++;
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            item.Phone = request.Phone;
            count++;
        }

        if (count == 0)
        {
            return BadRequest(new PatientPatchResponseModel { IsSuccess = false, Message = "No fields provided to update" });
        }

        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();

        return Ok(new PatientPatchResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Patient Patched Successfully" : "Patch Failed",
            Data = new PatientModel
            {
                Id = item.Id,
                Name = item.Name,
                Phone = item.Phone
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult SoftDeletePatient(int id, [FromQuery] string deletedBy)
    {
        var item = _db.Patients.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new PatientDeleteResponseModel { IsSuccess = false, Message = "Patient not found" });

        item.IsDeleted = true;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = deletedBy;

        int result = _db.SaveChanges();
        return Ok(new PatientDeleteResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Patient Soft Deleted Successfully" : "Delete Failed" });
    }
}