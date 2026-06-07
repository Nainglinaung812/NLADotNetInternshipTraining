namespace NLADotNetInternshipTraining.HospitalAppointmentSystem.Models;
public class AppointmentModel
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Status { get; set; } 
    public DateTime CreatedDateTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public string? ModifiedBy { get; set; }
}

public class DoctorCreateRequestModel
{
    public string Name { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string? CreatedBy { get; set; }
}
public class DoctorCreateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class DoctorUpdateRequestModel
{
    public string Name { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string? ModifiedBy { get; set; }
}
public class DoctorUpdateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class DoctorDeleteResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }

// --- PATIENT MODELS ---
public class PatientCreateRequestModel
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? CreatedBy { get; set; }
}
public class PatientCreateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class PatientUpdateRequestModel
{
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? ModifiedBy { get; set; }
}
public class PatientUpdateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class PatientDeleteResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }

public class AppointmentCreateRequestModel
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
}
public class AppointmentCreateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class AppointmentStatusUpdateRequestModel
{
    public string Status { get; set; } = null!; 
    public string? ModifiedBy { get; set; }
}
public class AppointmentStatusUpdateResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public AppointmentModel? Data { get; set; }
}
public class DoctorPatchRequestModel
{
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public string? ModifiedBy { get; set; }
}

public class DoctorPatchResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public DoctorModel? Data { get; set; }
}

public class DoctorModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Specialization { get; set; } = null!;
}
public class PatientPatchRequestModel
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? ModifiedBy { get; set; }
}

public class PatientPatchResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public PatientModel? Data { get; set; } 
}

public class PatientModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
}