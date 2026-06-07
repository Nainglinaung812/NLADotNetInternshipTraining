namespace NLADotNetInternshipTraining.DapperSample;

public class Student
{
    public int StudentId { get; set; }

    public string StudentNo { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string FatherName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? ModifiedDateTime { get; set; }

    public string? ModifiedBy { get; set; }
}
