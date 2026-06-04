using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.MvcApp.Models;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
namespace NLADotNetInternshipTraining.MvcApp.Controllers;

public class BlogController : Controller
{
    private readonly AppDbContext _db;


    public BlogController(AppDbContext db)
    {
        _db = db;
    }


    public IActionResult Generate()
    {
        for (int i = 1; i <= 1000; i++)
        {
            TblBlog blog = new TblBlog
            {
                BlogTitle = $"Blog Title {i}",
                BlogAuthor = $"Author {i}",
                BlogContent = $"Blog Content {i}"
            };
            _db.TblBlogs.Add(blog);
        }
        _db.SaveChanges();

        return Redirect("/blog");
    }

    public async Task<ActionResult> Index([FromQuery] BlogListRequestModel requestModel)
    {
        //requestModel = new BlogListRequestModel
        //{

        //};

        var query = _db.TblBlogs.AsQueryable();

        int rowCount = await query.CountAsync();

        int pageNo = requestModel.PageNo;
        int pageSize = requestModel.PageSize;
        var lst = query
            .OrderByDescending(x => x.BlogId)
            .Skip((pageNo - 1) * pageSize) // 3-1 = 2*10=20, 21-30
            .Take(pageSize)
            .ToList();

        BlogListResponseModel model = new BlogListResponseModel();
        model.PageCount = rowCount / pageSize; // 12/10 = 1, 2 // 120/10 = 12 // 119/10 = 11
        if (rowCount % pageSize > 0)
        {
            //model.PageCount += 1;
            model.PageCount++;
        }

        model.TotalRecords = rowCount;
        model.PageNo = pageNo;
        model.PageSize = pageSize;

        model.Data = lst.Select(x => new BlogModel
        {
            Id = x.BlogId,
            Title = x.BlogTitle,
            Author = x.BlogAuthor,
            Content = x.BlogContent
        }).ToList();

        ////model.Data = new List<BlogModel>();

        //List<BlogModel> data = new List<BlogModel>();

        //foreach (var item in lst)
        //{
        //    //model.Data.Add(new BlogModel
        //    //{
        //    //    Id = item.BlogId,
        //    //    Title = item.BlogTitle,
        //    //    Author = item.BlogAuthor,
        //    //    Content = item.BlogContent
        //    //});

        //    data.Add(new BlogModel
        //    {
        //        Id = item.BlogId,
        //        Title = item.BlogTitle,
        //        Author = item.BlogAuthor,
        //        Content = item.BlogContent
        //    });
        //}

        //model.Data = data;

        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(BlogCreateRequestModel requestModel)
    {
        _db.TblBlogs.Add(new TblBlog
        {
            BlogTitle = requestModel.Title,
            BlogAuthor = requestModel.Author,
            BlogContent = requestModel.Content
        });
        var result = await _db.SaveChangesAsync();

        TempData["IsSuccess"] = result > 0;
        TempData["Message"] = result > 0 ? "Blog created successfully." : "Failed to create blog.";

        return Redirect("/blog");
    }

    public async Task<IActionResult> Edit([FromQuery] BlogEditRequestModel requestModel)
    {
        var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == requestModel.Id);
        if (item is null)
        {
            TempData["IsSuccess"] = false;
            TempData["Message"] = "Blog not found.";
            return Redirect("/blog");
        }

        BlogEditResponseModel model = new BlogEditResponseModel
        {
            Data = new BlogModel
            {
                Author = item.BlogAuthor,
                Content = item.BlogContent,
                Title = item.BlogTitle,
                Id = item.BlogId
            }
        };
        return View(model);
    }

#region Update Blog

[HttpPost]
public async Task<IActionResult> Update(BlogUpdateRequestModel requestModel)
{
    var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == requestModel.Id);
    if (item is null)
    {
        TempData["IsSuccess"] = false;
        TempData["Message"] = "Blog not found.";
        return Redirect("/blog");
    }

    
    item.BlogTitle = requestModel.Title;
    item.BlogAuthor = requestModel.Author;
    item.BlogContent = requestModel.Content;

    _db.TblBlogs.Update(item);
    var result = await _db.SaveChangesAsync();

    TempData["IsSuccess"] = result > 0;
    TempData["Message"] = result > 0 ? "Blog updated successfully." : "Failed to update blog.";

    return Redirect("/blog");
}

#endregion

#region Delete Blog (Yes / No Confirmation Screen)
public async Task<IActionResult> Delete([FromQuery] BlogEditRequestModel requestModel)
{
    var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == requestModel.Id);
    if (item is null)
    {
        TempData["IsSuccess"] = false;
        TempData["Message"] = "Blog not found.";
        return Redirect("/blog");
    }

    BlogEditResponseModel model = new BlogEditResponseModel
    {
        Data = new BlogModel
        {
            Id = item.BlogId,
            Title = item.BlogTitle,
            Author = item.BlogAuthor,
            Content = item.BlogContent
        }
    };
    
    return View(model);
}
[HttpPost]
public async Task<IActionResult> DeleteConfirm(int id)
{
    var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
    if (item is null)
    {
        TempData["IsSuccess"] = false;
        TempData["Message"] = "Blog not found.";
        return Redirect("/blog");
    }

    _db.TblBlogs.Remove(item);
    var result = await _db.SaveChangesAsync();

    TempData["IsSuccess"] = result > 0;
    TempData["Message"] = result > 0 ? "Blog deleted successfully." : "Failed to delete blog.";

    return Redirect("/blog");
}

#endregion

}