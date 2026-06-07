using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Database.Models;
using NLADotNetInternshipTraining.HospitalAppointmentSystem.Models;

namespace NLADotNetInternshipTraining.HospitalAppointmentSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly HospitalAppointmentDbContext _db;

    public AppointmentsController(HospitalAppointmentDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public IActionResult GetAppointments()
    {
        var lst = _db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.Id)
            .Select(a => new
            {
                a.Id,
                a.DoctorId,
                DoctorName = a.Doctor.Name,
                a.PatientId,
                PatientName = a.Patient.Name,
                a.AppointmentDate,
                a.Status,
                a.CreatedDateTime
            }).ToList();

        return Ok(lst);
    }
    [HttpGet("today")]
    public IActionResult GetTodayAppointments()
    {
        var today = DateTime.Today;
        var lst = _db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.Patient)
            .Where(a => a.AppointmentDate.Date == today && !a.IsDeleted)
            .Select(a => new {
                a.Id,
                DoctorName = a.Doctor.Name,
                PatientName = a.Patient.Name,
                a.AppointmentDate,
                a.Status
            }).ToList();

        return Ok(lst);
    }
    [HttpGet("doctor/{doctorId}/schedule")]
    public IActionResult GetDoctorSchedule(int doctorId)
    {
        var lst = _db.Appointments
            .Include(a => a.Patient)
            .Where(a => a.DoctorId == doctorId && !a.IsDeleted)
            .Select(a => new {
                a.Id,
                PatientName = a.Patient.Name,
                a.AppointmentDate,
                a.Status
            }).ToList();

        return Ok(lst);
    }
    [HttpPost]
    public IActionResult CreateAppointment(AppointmentCreateRequestModel request)
    {
        var doctor = _db.Doctors.FirstOrDefault(d => d.Id == request.DoctorId && !d.IsDeleted);
        var patient = _db.Patients.FirstOrDefault(p => p.Id == request.PatientId && !p.IsDeleted);

        if (doctor is null || patient is null)
        {
            return BadRequest(new AppointmentCreateResponseModel { IsSuccess = false, Message = "Doctor or Patient not found." });
        }

        // *** CORE BUSINESS LOGIC: ဆရာဝန်တစ်ယောက်အတွက် ထိုနေ့၌ လူနာ အယောက် ၃၀ ပြည့်မပြည့် စစ်ဆေးခြင်း ***
        var targetDate = request.AppointmentDate.Date;
        var currentBookingCount = _db.Appointments
            .Count(a => a.DoctorId == request.DoctorId && a.AppointmentDate.Date == targetDate && a.Status != "Cancelled" && !a.IsDeleted);

        if (currentBookingCount >= 30)
        {
            return BadRequest(new AppointmentCreateResponseModel 
            { 
                IsSuccess = false, 
                Message = "This doctor has already reached the maximum limit of 30 appointments for this day." 
            });
        }

        _db.Appointments.Add(new Appointment
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            AppointmentDate = request.AppointmentDate,
            Status = "Pending",
            CreatedDateTime = DateTime.Now,
            CreatedBy = patient.Name, 
            IsDeleted = false
        });

        int result = _db.SaveChanges();
        return StatusCode(201, new AppointmentCreateResponseModel { IsSuccess = result > 0, Message = result > 0 ? "Appointment Booked Successfully" : "Booking Failed" });
    }

    [HttpPut("{id}/status")]
    public IActionResult UpdateStatus(int id, AppointmentStatusUpdateRequestModel request)
    {
        var item = _db.Appointments.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
        if (item is null) return NotFound(new AppointmentStatusUpdateResponseModel { IsSuccess = false, Message = "Appointment not found" });

        item.Status = request.Status;
        item.ModifiedDateTime = DateTime.Now;
        item.ModifiedBy = request.ModifiedBy ?? "Staff";

        int result = _db.SaveChanges();

        return Ok(new AppointmentStatusUpdateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Status Updated Successfully" : "Update Failed",
            Data = new AppointmentModel
            {
                Id = item.Id,
                DoctorId = item.DoctorId,
                PatientId = item.PatientId,
                AppointmentDate = item.AppointmentDate,
                Status = item.Status,
                CreatedDateTime = item.CreatedDateTime,
                CreatedBy = item.CreatedBy,
                ModifiedDateTime = item.ModifiedDateTime,
                ModifiedBy = item.ModifiedBy
            }
        });
    }
}