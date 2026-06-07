using System.Data;
using Microsoft.Data.SqlClient;

namespace NLADotNetInternshipTraining.AdoDotNetSample2;

public class AdoDotNetSample
{
    private readonly SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
    {
        DataSource = "localhost",
        InitialCatalog = "NLADotNetInternshipTraining",
        UserID = "sa",
        Password = "Linn@81220015228",
        TrustServerCertificate = true
    };

    public void Read()
    {

        SqlConnection connection = new SqlConnection(builder.ConnectionString);

        connection.Open();
        string sql = @"SELECT TOP (1000) [StudentId]
      ,[StudentNo]
      ,[StudentName]
      ,[FatherName]
      ,[Address]
      ,[DateOfBirth]
      ,[IsDelete]
      ,[CreatedDateTime]
      ,[CreatedBy]
      ,[ModifiedDateTime]
      ,[ModifiedBy]
  FROM [NLADotNetInternshipTraining].[dbo].[Tbl_Student] Where IsDelete = 0";

        SqlCommand command = new SqlCommand(sql, connection);
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
        connection.Close();

        List<Student> students = new List<Student>();
        foreach (DataRow row in dataTable.Rows)
        {
            Student student = new Student()
            {
                StudentId = Convert.ToInt32(row["StudentId"]),
                StudentNo = Convert.ToString(row["StudentNo"]),
                StudentName = Convert.ToString(row["StudentName"]),
                FatherName = Convert.ToString(row["FatherName"]),
                Address = Convert.ToString(row["Address"]),
                DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                IsDelete = Convert.ToBoolean(row["IsDelete"]),
                CreatedDateTime = Convert.ToDateTime(row["CreatedDateTime"]),
                CreatedBy = Convert.ToString(row["CreatedBy"]),
                ModifiedDateTime = row["ModifiedDateTime"] == DBNull.Value ? null : Convert.ToDateTime(row["ModifiedDateTime"]),
                ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : Convert.ToString(row["ModifiedBy"]),
            };
            students.Add(student);

            System.Console.WriteLine($"StudentID: {student.StudentId},StudentNo: {student.StudentNo}, StudentName: {student.StudentName},FatherName: {student.FatherName},Address: {student.Address},IsDelete: {student.IsDelete},CreatedDateTime: {student.CreatedDateTime},CreatedBy: {student.CreatedBy},ModifiedDateTime: {student.ModifiedDateTime},ModifiedBy: {student.ModifiedBy}, DateOfBirth: {student.DateOfBirth.ToString("dd/MMM/yyyy")}");
            System.Console.WriteLine("--------------------------------------------------");
        }

    }
    public void Edit()
    {
        string sql = $@"SELECT TOP (1000) [StudentId]
      ,[StudentNo]
      ,[StudentName]
      ,[FatherName]
      ,[Address]
      ,[DateOfBirth]
      ,[IsDelete]
      ,[CreatedDateTime]
      ,[CreatedBy]
      ,[ModifiedDateTime]
      ,[ModifiedBy]
  FROM [NLADotNetInternshipTraining].[dbo].[Tbl_Student] Where StudentId = @StudentId and IsDelete = 0";

        SqlConnection connection = new SqlConnection(builder.ConnectionString);

        connection.Open();
        int id = 14;


        SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentId", id);
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        DataTable dataTable = new DataTable();
        adapter.Fill(dataTable);
        connection.Close();

        if (dataTable.Rows.Count == 0)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        DataRow row = dataTable.Rows[0];
        Student student = new Student()
        {
            StudentId = Convert.ToInt32(row["StudentId"]),
            StudentNo = Convert.ToString(row["StudentNo"]),
            StudentName = Convert.ToString(row["StudentName"]),
            FatherName = Convert.ToString(row["FatherName"]),
            Address = Convert.ToString(row["Address"]),
            DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
            IsDelete = Convert.ToBoolean(row["IsDelete"]),
            CreatedDateTime = Convert.ToDateTime(row["CreatedDateTime"]),
            CreatedBy = Convert.ToString(row["CreatedBy"]),
            ModifiedDateTime = row["ModifiedDateTime"] == DBNull.Value ? null : Convert.ToDateTime(row["ModifiedDateTime"]),
            ModifiedBy = row["ModifiedBy"] == DBNull.Value ? null : Convert.ToString(row["ModifiedBy"]),
        };


        System.Console.WriteLine($"StudentID: {student.StudentId},StudentNo: {student.StudentNo}, StudentName: {student.StudentName},FatherName: {student.FatherName},Address: {student.Address},IsDelete: {student.IsDelete},CreatedDateTime: {student.CreatedDateTime},CreatedBy: {student.CreatedBy},ModifiedDateTime: {student.ModifiedDateTime},ModifiedBy: {student.ModifiedBy}, DateOfBirth: {student.DateOfBirth.ToString("dd/MMM/yyyy")}");
        System.Console.WriteLine("--------------------------------------------------");
    }

    public void Create()
    {
        Student student = new Student()
        {
            StudentNo = "S-002",
            StudentName = "Kyaw Kyaw",
            FatherName = "U Mya",
            Address = "Yangon",
            DateOfBirth = new DateTime(2002, 5, 10),
            IsDelete = false,
            CreatedDateTime = DateTime.Now,
            CreatedBy = "Admin"
        };
        SqlConnection connection = new SqlConnection(builder.ConnectionString);

        connection.Open();


        string sql = @"INSERT INTO [dbo].[Tbl_Student]
           ([StudentNo]
           ,[StudentName]
           ,[FatherName]
           ,[Address]
           ,[DateOfBirth]
           ,[IsDelete]
           ,[CreatedDateTime]
           ,[CreatedBy])
     VALUES
           (@StudentNo
           ,@StudentName
           ,@FatherName
           ,@Address
           ,@DateOfBirth
           ,@IsDelete
           ,@CreatedDateTime
           ,@CreatedBy)";

        SqlCommand command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@StudentNo", student.StudentNo);
        command.Parameters.AddWithValue("@StudentName", student.StudentName);
        command.Parameters.AddWithValue("@FatherName", student.FatherName);
        command.Parameters.AddWithValue("@Address", student.Address);
        command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
        command.Parameters.AddWithValue("@IsDelete", student.IsDelete);
        command.Parameters.AddWithValue("@CreatedDateTime", student.CreatedDateTime);
        command.Parameters.AddWithValue("@CreatedBy", student.CreatedBy);
        int result = command.ExecuteNonQuery();
        connection.Close();
        Console.WriteLine(result > 0
            ? "Saving Successful."
            : "Saving Failed.");
        System.Console.WriteLine("--------------------------------------------------");

    }
    public void Update()
    {
        Student student = new Student()
        {
            StudentId = 14,
            StudentNo = "S-001",
            StudentName = "Aung Aung",
            FatherName = "U Tun",
            Address = "Mandalay",
            DateOfBirth = new DateTime(2001, 2, 2),
            IsDelete = false,
            ModifiedBy = "Admin"
        };
        SqlConnection connection = new SqlConnection(builder.ConnectionString);

        connection.Open();

        string sql = @"UPDATE [dbo].[Tbl_Student]
                   SET [StudentNo] = @StudentNo
                      ,[StudentName] = @StudentName
                      ,[FatherName] = @FatherName
                      ,[Address] = @Address
                      ,[DateOfBirth] = @DateOfBirth
                      ,[IsDelete] = @IsDelete
                      ,[ModifiedDateTime] = @ModifiedDateTime
                      ,[ModifiedBy] = @ModifiedBy
                 WHERE StudentId = @StudentId";

        SqlCommand command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@StudentId", student.StudentId);
        command.Parameters.AddWithValue("@StudentNo", student.StudentNo);
        command.Parameters.AddWithValue("@StudentName", student.StudentName);
        command.Parameters.AddWithValue("@FatherName", student.FatherName);
        command.Parameters.AddWithValue("@Address", student.Address);
        command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
        command.Parameters.AddWithValue("@IsDelete", student.IsDelete);
        command.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
        command.Parameters.AddWithValue("@ModifiedBy", student.ModifiedBy);

        int result = command.ExecuteNonQuery();

        connection.Close();

        Console.WriteLine(result > 0
            ? "Updating Successful."
            : "Updating Failed.");
        System.Console.WriteLine("--------------------------------------------------");
    }
    public void Delete()
    {
        using SqlConnection connection = new SqlConnection(builder.ConnectionString);
        connection.Open();
        int id = 32;
        // Soft delete: We just flip the IsDelete flag to true
        string sql = @"UPDATE [dbo].[Tbl_Student] 
                   SET [IsDelete] = @IsDelete, 
                       [ModifiedDateTime] = @ModifiedDateTime, 
                       [ModifiedBy] = @ModifiedBy 
                   WHERE [StudentId] = @StudentId";

        using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentId", id);
        command.Parameters.AddWithValue("@IsDelete", true);
        command.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
        command.Parameters.AddWithValue("@ModifiedBy", "SystemAdmin"); // Or dynamic user login info

        int result = command.ExecuteNonQuery();

        connection.Close();

        if (result > 0)
        {
            System.Console.WriteLine("Student marked as deleted successfully.");
        }
        else
        {
            System.Console.WriteLine("Data not found or delete failed.");
        }
        System.Console.WriteLine("--------------------------------------------------");
    }
}
