using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.MvcApp.Models;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
namespace NLADotNetInternshipTraining.MvcApp.Controllers;

public class BlogAjaxController : Controller
{
    private readonly AppDbContext _db;

    public BlogAjaxController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> List()
    {
        var lst = await _db.TblBlogs.ToListAsync();
        return Json(lst);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(BlogCreateRequestModel requestModel)
    {
        var blog = new TblBlog
        {
            BlogTitle = requestModel.Title,
            BlogAuthor = requestModel.Author,
            BlogContent = requestModel.Content
        };

        await _db.TblBlogs.AddAsync(blog);
        var result = await _db.SaveChangesAsync();

        var model = new BlogCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Saving Successful." : "Saving Failed."
        };
        return Json(model);
    }
    // 1. Edit Page View ကို ပြသရန်
    public IActionResult Edit(int id)
    {
        ViewBag.BlogId = id; // View ထဲမှာ ID ကို သုံးနိုင်အောင် သိမ်းထားခြင်း
        return View();
    }

    // 2. ပြင်ဆင်မည့် Blog ရဲ့ Data ကို Ajax ဖြင့် ယူရန်
    [HttpGet]
    public async Task<IActionResult> GetBlog(int id)
    {
        var blog = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
        if (blog == null)
        {
            return Json(new { IsSuccess = false, Message = "Data not found." });
        }

        var model = new BlogEditResponseModel
        {
            Data = new BlogModel
            {
                Id = blog.BlogId,
                Title = blog.BlogTitle,
                Author = blog.BlogAuthor,
                Content = blog.BlogContent
            }
        };
        return Json(model);
    }

    // 3. ပြင်ဆင်ထားသော Data ကို Update လုပ်ရန်
    [HttpPost]
    public async Task<IActionResult> Update(BlogUpdateRequestModel requestModel)
    {
        var blog = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == requestModel.Id);
        if (blog == null)
        {
            return Json(new { IsSuccess = false, Message = "Data not found." });
        }

        blog.BlogTitle = requestModel.Title;
        blog.BlogAuthor = requestModel.Author;
        blog.BlogContent = requestModel.Content;

        _db.TblBlogs.Update(blog);
        var result = await _db.SaveChangesAsync();
        var model = new BlogCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Updating Successful." : "Updating Failed."
        };
        return Json(model);
    }

    // 4. Data ကို ဖျက်ရန် (Delete)
    [HttpPost]
    public async Task<IActionResult> Delete(BlogEditRequestModel requestModel)
    {
        var blog = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == requestModel.Id);
        if (blog == null)
        {
            return Json(new { IsSuccess = false, Message = "Data not found." });
        }

        _db.TblBlogs.Remove(blog);
        var result = await _db.SaveChangesAsync();
        var model = new BlogCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Deleting Successful." : "Deleting Failed."
        };
        return Json(model);
    }
}
