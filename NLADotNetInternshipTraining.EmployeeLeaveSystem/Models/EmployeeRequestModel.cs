namespace NLADotNetInternshipTraining.EmployeeLeaveSystem.Models;

public class EmployeeLeaveModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public string? ModifiedBy { get; set; }
}

public class LeaveCreateRequestModel
{
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Reason { get; set; }
}

public class LeaveCreateResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}

public class LeaveApproveRequestModel
{
    public string? Status { get; set; }
    public string? ManagerName { get; set; }
    public string? Remarks { get; set; }
}

public class LeaveApproveResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public EmployeeLeaveModel? Data { get; set; }
}

public class LeaveDeleteResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}


public class EmployeeCreateRequestModel
{
    public string Name { get; set; } = null!;
    public string Department { get; set; } = null!;
    public DateTime JoinDate { get; set; }
    public int TotalLeaveBalance { get; set; }
    public string? CreatedBy { get; set; }
}
public class EmployeeCreateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class EmployeeUpdateRequestModel
{
    public string Name { get; set; } = null!;
    public string Department { get; set; } = null!;
    public int TotalLeaveBalance { get; set; }
    public string? ModifiedBy { get; set; }
}
public class EmployeeUpdateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class EmployeeDeleteResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }

public class LeaveTypeCreateRequestModel
{
    public string TypeName { get; set; } = null!;
    public int MaxDaysAllowed { get; set; }
    public string? CreatedBy { get; set; }
}
public class LeaveTypeCreateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class LeaveTypeUpdateRequestModel
{
    public string TypeName { get; set; } = null!;
    public int MaxDaysAllowed { get; set; }
    public string? ModifiedBy { get; set; }
}
public class LeaveTypeUpdateResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class LeaveTypeDeleteResponseModel { public bool IsSuccess { get; set; } public string? Message { get; set; } }
public class EmployeePatchRequestModel
{
    public string? Name { get; set; }
    public string? Department { get; set; }
    public int? TotalLeaveBalance { get; set; } 
    public string? ModifiedBy { get; set; }
}

public class EmployeePatchResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public EmployeePatchDataModel? Data { get; set; } 
}

public class EmployeePatchDataModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Department { get; set; } = null!;
    public int TotalLeaveBalance { get; set; }
}
public class EmployeeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Department { get; set; } = null!;
    public DateTime JoinDate { get; set; }
    public int TotalLeaveBalance { get; set; }
}
public class LeaveTypeModel
{
    public int Id { get; set; }
    public string TypeName { get; set; } = null!;
    public int MaxDaysAllowed { get; set; }
}
public class LeaveTypePatchRequestModel
{
    public string? TypeName { get; set; }
    public int? MaxDaysAllowed { get; set; }
    public string? ModifiedBy { get; set; }
}

public class LeaveTypePatchResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public LeaveTypeModel? Data { get; set; } 
}
