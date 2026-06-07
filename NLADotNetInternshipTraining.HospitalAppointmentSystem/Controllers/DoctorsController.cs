using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Database.Models; 
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Models;

namespace NLADotNetInternshipTraining.HospitalAppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly HospitalAppointmentDbContext _db;

    public DoctorsController(HospitalAppointmentDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetDoctors()
    {
        var lst = _db.Doctors
            .Include(d => d.Appointments)
            .Where(d => !d.IsDeleted)
            .OrderByDescending(d => d.Id)
            .ToList();
        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetDoctor(int id)
    {
        var item = _db.Doctors
            .Include(d => d.Appointments) 
            .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

        if (item is null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateDoctor(DoctorCreateRequestModel request)
    {
        _db.Doctors.Add(new Doctor
        {
            Name = request.Name,
            Specialization = request.Specialization,
            CreatedDateTime = DateTime.Now,
            CreatedBy = request.CreatedBy ?? "Admin",
            IsDeleted = false
        });
        int result = _db.SaveChanges();

        return StatusCode(201, new DoctorCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Doctor Added Successfully" : "Failed to Add Doctor"
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDoctor(int id, DoctorUpdateRequestModel request)
    {
        var item = _db.Doctors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new DoctorUpdateResponseModel { IsSuccess = false, Message = "Doctor not found" });

        item.Name = request.Name;
        item.Specialization = request.Specialization;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();
        return Ok(new DoctorUpdateResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Doctor Updated Successfully" : "Update Failed" });
    }

    [HttpPatch("{id}")]
    public IActionResult PatchDoctor(int id, DoctorPatchRequestModel request)
    {
        var item = _db.Doctors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null)
        {
            return NotFound(new DoctorPatchResponseModel { IsSuccess = false, Message = "Doctor not found" });
        }

        int count = 0;

        if (!string.IsNullOrEmpty(request.Name))
        {
            item.Name = request.Name;
            count++;
        }

        if (!string.IsNullOrEmpty(request.Specialization))
        {
            item.Specialization = request.Specialization;
            count++;
        }

        if (count == 0)
        {
            return BadRequest(new DoctorPatchResponseModel { IsSuccess = false, Message = "No fields provided to update" });
        }

        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Admin";

        int result = _db.SaveChanges();

        return Ok(new DoctorPatchResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Doctor Patched Successfully" : "Patch Failed",
            Data = new DoctorModel
            {
                Id = item.Id,
                Name = item.Name,
                Specialization = item.Specialization
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult SoftDeleteDoctor(int id, [FromQuery] string deletedBy)
    {
        var item = _db.Doctors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new DoctorDeleteResponseModel { IsSuccess = false, Message = "Doctor not found" });

        item.IsDeleted = true;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = deletedBy;

        int result = _db.SaveChanges();
        return Ok(new DoctorDeleteResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Doctor Soft Deleted Successfully" : "Delete Failed" });
    }
}