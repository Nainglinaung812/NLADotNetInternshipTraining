using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;
using NLADotNetInternshipTraining.WebApi.Models;

namespace NLADotNetInternshipTraining.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _db = new AppDbContext();
    [HttpGet]
    public IActionResult GetBLogs()
    {
        var lst = _db.TblBlogs.ToList();
        return Ok(lst);
    }
    [HttpGet("{id}")]
    public IActionResult GetBlog(int id)
    {
        var item = _db.TblBlogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            return NotFound();
        }
        return Ok(item);
    }
    [HttpPost]
    public IActionResult CreateStudent(BlogCreateRequestModel studentRequestModel)
    {
        _db.TblBlogs.Add(new TblBlog
        {
            BlogAuthor = studentRequestModel.BlogAuthor,
            BlogTitle = studentRequestModel.BlogTitle,
            BlogContent = studentRequestModel.BlogContent,

        });
        int result = _db.SaveChanges();

        return StatusCode(201, new BlogCreateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Saving Successful" : "Saving Failed"
        });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateStudent(BlogUpdateRequestModel request, int id)
    {
        var item = _db.TblBlogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            return NotFound(new BlogUpdateResponseModel
            {
                IsSuccess = false,
                Message = "Blog not found"
            });
        }
        item.BlogAuthor = request.BlogAuthor;
        item.BlogTitle = request.BlogTitle;
        item.BlogContent = request.BlogContent;

        int result = _db.SaveChanges();

        return Ok(new BlogUpdateResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Blog Update Successfully" : "Blog Update Failed",
            Data = new BlogModel
            {
                BlogId = item.BlogId,
                BlogAuthor = item.BlogAuthor,
                BlogTitle = item.BlogTitle,
                BlogContent = item.BlogContent,

            }
        });
    }
    [HttpPatch("{id}")]
    public IActionResult PatchStudent(int id, BlogPatchRequestModel request)
    {
        var item = _db.TblBlogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            return NotFound(new BlogPatchResponseModel
            {
                IsSuccess = false,
                Message = "Blog not found"
            });
        }
        int count = 0;
        if (!string.IsNullOrEmpty(request.BlogAuthor))
        {
            count++;

            item.BlogAuthor = request.BlogAuthor;

        }
        if (!string.IsNullOrEmpty(request.BlogTitle))
        {
            count++;
            item.BlogTitle = request.BlogTitle;

        }
        if (!string.IsNullOrEmpty(request.BlogContent))
        {
            count++;
            item.BlogContent = request.BlogContent;

        }
        if (count == 0)
        {
            return NotFound(new BlogPatchResponseModel
            {
                IsSuccess = false,
                Message = "no need to update"
            });

        }


        int result = _db.SaveChanges();

        return Ok(new BlogPatchResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Blog Update Successfully" : "Blog Update Failed",
            Data = new BlogModel
            {
                BlogId = item.BlogId,
                BlogAuthor = item.BlogAuthor,
                BlogTitle = item.BlogTitle,
                BlogContent = item.BlogContent,

            }
        });

    }
    [HttpDelete("{id}")]
    public IActionResult DeleteStudent(int id)

    {
        var item = _db.TblBlogs.FirstOrDefault(x => x.BlogId == id);
        if (item is null)
        {
            return NotFound(new BlogDeleteResponseModel
            {
                IsSuccess = false,
                Message = "Blog not found"
            });
        }
        _db.TblBlogs.Remove(item);
        int result = _db.SaveChanges();
        return Ok(new BlogDeleteResponseModel
        {
            IsSuccess = result > 0,
            Message = result > 0 ? "Delete Successfully" : "Delete Failed"
        });
    }

}