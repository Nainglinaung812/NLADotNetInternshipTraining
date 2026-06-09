using NLADotNetInternshipTraining.BlazorServer.Frontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using NLADotNetInternshipTraining.EFCoreDatabaseFirst.Database.AppDbContextModels;

namespace NLADotNetInternshipTraining.BlazorServer.Frontend.Components.Pages;

public partial class Page_Blog
{
    [Inject]
    public AppDbContext _db { get; set; }
    [Inject]
    public HttpClientService HttpClientService { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    private BlogModel data = new BlogModel();

    private List<BlogModel> blogs { get; set; } = new List<BlogModel>();

    protected override async Task OnInitializedAsync()
    {
        await BindData();
    }

    private async Task BindData()
    {
        //var a = "1000".ToDecimal();

        //using var client = HttpClientFactory.CreateClient();
        //client.GetAsync();


        var lstBlog = await HttpClientService.ExecuteAsync<List<BlogDto>>("blog", EnumHttpMethod.Get);
        var lst = await _db.TblBlogs
            .AsNoTracking()
            .OrderByDescending(x => x.BlogId)
            .ToListAsync();
        blogs = lst.Select(x => new BlogModel
        {
            Author = x.BlogAuthor,
            Content = x.BlogContent,
            Id = x.BlogId.ToString(),
            Title = x.BlogTitle
        }).ToList();
    }

    private async Task Save()
    {
        if (data.Id != null)
        {
            int blogId = Convert.ToInt32(data.Id);
            var item = await _db.TblBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == blogId);
            if (item is null)
            {
                return;
            }

            item.BlogTitle = data.Title;
            item.BlogAuthor = data.Author;
            item.BlogContent = data.Content;

            _db.Entry(item).State = EntityState.Modified;

            await _db.SaveChangesAsync();
        }
        else
        {
            _db.TblBlogs.Add(new TblBlog
            {
                BlogAuthor = data.Author,
                BlogContent = data.Content,
                BlogTitle = data.Title
            });
            await _db.SaveChangesAsync();
        }
        data = new BlogModel();
        await BindData();
    }

    private async Task Edit(string id)
    {
        int blogId = Convert.ToInt32(id);
        var item = await _db.TblBlogs.AsNoTracking().FirstOrDefaultAsync(x => x.BlogId == blogId);
        if (item is null)
        {
            return;
        }

        data = new BlogModel
        {
            Title = item.BlogTitle,
            Author = item.BlogAuthor,
            Content = item.BlogContent,
            Id = item.BlogId.ToString()
        };
    }

    private async Task Delete(string id)
    {
        int blogId = Convert.ToInt32(id);
        var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == blogId);
        if (item is null)
        {
            return;
        }

        string message = $"Are you sure you want to delete the blog '{item.BlogTitle}'?";
        var isConfirm = await JSRuntime.InvokeAsync<bool>("confirm", message);
        if (isConfirm)
        {
            // _db.Entry(item).State = EntityState.Deleted;
            _db.TblBlogs.Remove(item);
            await _db.SaveChangesAsync();
            await BindData();
        }
    }
}

public class BlogModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
}

public class BlogDto
{
    public string BlogId { get; set; }
    public string BlogTitle { get; set; }
    public string BlogAuthor { get; set; }
    public string BlogContent { get; set; }
}