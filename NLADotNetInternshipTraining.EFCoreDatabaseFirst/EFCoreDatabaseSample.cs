using System.Text.Json.Serialization;
using Newtonsoft.Json;
//  EFCoreDatabaseSample.cs ရဲ့ အပေါ်ဆုံးမှာ ဒါလေး ထည့်ပေးပါ-
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;
namespace NLADotNetInternshipTraining.EFCoreDatabaseFirstSample;

public class EFCoreDatabaseSample
{
    private readonly AppDbContext _db;
    public EFCoreDatabaseSample()
    {
        _db = new AppDbContext();
    }
    public void Read()
    {
        // AppDbContext db = new AppDbContext();
        List<TblStudent> lst = _db.TblStudents.ToList();
        foreach (TblStudent item in lst)
        {
            System.Console.WriteLine($"StudentId: {item.StudentId}, StudentNo: {item.StudentNo}, StudentName: {item.StudentName},FatherName: {item.FatherName}");
            System.Console.WriteLine("-----------------");
        }

    }
    public void Edit()
    {
        TblStudent item = _db.TblStudents.FirstOrDefault(x => x.StudentId == 17);
        if (item is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        System.Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));



    }
    public void Create()
    {
        TblStudent student = new TblStudent()
        {
            StudentNo = "S-003",
            StudentName = "Mg Mg",
            FatherName = "U Ba",
            Address = "Yangon",
            DateOfBirth = new DateTime(2000, 1, 1),
            CreatedDateTime = DateTime.Now,
            CreatedBy = "1"
        };
        _db.TblStudents.Add(student);
        int result = _db.SaveChanges();
        System.Console.WriteLine(result > 0 ? "Saving Successful" : "Saving Failed");


    }
    public void Update()
    {
        TblStudent student = _db.TblStudents.FirstOrDefault(x => x.StudentId == 17);
        if (student is null)
        {
            System.Console.WriteLine("Data not found");
            return;
        }
        student.StudentName = "Linn Aung";
        int result = _db.SaveChanges();
        System.Console.WriteLine(result > 0 ? "Updating Successful" : "Updating Failed");

    }
    public void Delete()
    {
        TblStudent student = _db.TblStudents.FirstOrDefault(x => x.StudentId == 19);
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
