using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace NLADotNetInternshipTraining.DapperSample2;

public class DapperSample
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

        using IDbConnection sqlConnection = new SqlConnection(builder.ConnectionString);
        sqlConnection.Open();
        List<Student> lst = sqlConnection.Query<Student>(sql).ToList();
        foreach (Student item in lst)
        {
            System.Console.WriteLine($"StudentId: {item.StudentId}, StudentNo: {item.StudentNo}, StudentName: {item.StudentName},FatherName: {item.FatherName}");

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


        using IDbConnection sqlConnection = new SqlConnection(builder.ConnectionString);
        sqlConnection.Open();
        Student item = sqlConnection.Query<Student>(sql, new Student { StudentId = 17 }).FirstOrDefault();
        if (item is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        System.Console.WriteLine($"StudentId: {item.StudentId}, StudentNo: {item.StudentNo}, StudentName: {item.StudentName},FatherName: {item.FatherName}");

    }
    // CREATE
    public void Create()
    {
        string sql = @"INSERT INTO Tbl_Student
                       (
                           StudentNo,
                           StudentName,
                           FatherName,
                           Address,
                           DateOfBirth,
                           IsDelete,
                           CreatedDateTime,
                           CreatedBy
                       )
                       VALUES
                       (
                           @StudentNo,
                           @StudentName,
                           @FatherName,
                           @Address,
                           @DateOfBirth,
                           0,
                           @CreatedDateTime,
                           @CreatedBy
                       )";

        Student student = new Student()
        {
            StudentNo = "S-003",
            StudentName = "Mg Mg",
            FatherName = "U Ba",
            Address = "Yangon",
            DateOfBirth = new DateTime(2000, 1, 1),
            CreatedDateTime = DateTime.Now,
            CreatedBy = "1"
        };

        using IDbConnection sqlConnection = new SqlConnection(builder.ConnectionString);
        sqlConnection.Open();

        int result = sqlConnection.Execute(sql, student);

        Console.WriteLine(result > 0
            ? "Saving Successful"
            : "Saving Failed");
    }

    // UPDATE
    public void Update()
    {
        string sql = @"UPDATE Tbl_Student
                       SET
                           StudentNo = @StudentNo,
                           StudentName = @StudentName,
                           FatherName = @FatherName,
                           Address = @Address,
                           DateOfBirth = @DateOfBirth,
                           ModifiedDateTime = @ModifiedDateTime,
                           ModifiedBy = @ModifiedBy
                       WHERE StudentId = @StudentId
                       AND IsDelete = 0";

        Student student = new Student()
        {
            StudentId = 20,
            StudentNo = "S-016",
            StudentName = "Aung Aung",
            FatherName = "U Tun",
            Address = "Mandalay",
            DateOfBirth = new DateTime(2001, 2, 2),
            ModifiedDateTime = DateTime.Now,
            ModifiedBy = "1",
            
        };

        using IDbConnection sqlConnection = new SqlConnection(builder.ConnectionString);
        sqlConnection.Open();

        int result = sqlConnection.Execute(sql, student);

        Console.WriteLine(result > 0
            ? "Updating Successful"
            : "Updating Failed");
    }

    // DELETE (Soft Delete)
    public void Delete()
    {
        string sql = @"UPDATE Tbl_Student
                       SET
                           IsDelete = 1,
                           ModifiedDateTime = @ModifiedDateTime,
                           ModifiedBy = @ModifiedBy
                       WHERE StudentId = @StudentId";

        var student = new
        {
            StudentId = 16,
            ModifiedDateTime = DateTime.Now,
            ModifiedBy = "1"
        };

        using IDbConnection sqlConnection = new SqlConnection(builder.ConnectionString);
        sqlConnection.Open();

        int result = sqlConnection.Execute(sql, student);

        Console.WriteLine(result > 0
            ? "Deleting Successful"
            : "Deleting Failed");
    }
}


