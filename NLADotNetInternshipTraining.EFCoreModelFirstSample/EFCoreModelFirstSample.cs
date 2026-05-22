using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace NLADotNetInternshipTraining.EFCoreModelFirstSample;

public class EFCoreModelSample
{
    private readonly AppDbContext _db;
    public EFCoreModelSample()
    {
        _db = new AppDbContext();
    }
    public void Read()
    {
        // AppDbContext db = new AppDbContext();
        List<Student> lst = _db.Students.ToList();
        foreach (Student item in lst)
        {
            System.Console.WriteLine($"StudentId: {item.StudentId}, StudentNo: {item.StudentNo}, StudentName: {item.StudentName},FatherName: {item.FatherName}");
            System.Console.WriteLine("-----------------");
        }

    }
    public void Edit()
    {
        Student item = _db.Students.FirstOrDefault(x => x.StudentId == 17);
        if (item is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        System.Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));



    }
    public void Create()
    {
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
        _db.Students.Add(student);
        int result = _db.SaveChanges();
        System.Console.WriteLine(result > 0 ? "Saving Successful" : "Saving Failed");


    }
    public void Update()
    {
        Student student = _db.Students.FirstOrDefault(x => x.StudentId == 17);
        if (student is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        student.StudentName = "Linn Linn";
        int result = _db.SaveChanges();
        System.Console.WriteLine(result > 0 ? "Updating Successful" : "Updating Failed");

    }
    public void Delete()
    {
        Student student = _db.Students.FirstOrDefault(x => x.StudentId == 37);
        if (student is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        student.IsDelete = true;
        int result = _db.SaveChanges();
        System.Console.WriteLine(result > 0 ? "Deleting Successful" : "Deleting Failed");


    }

}
