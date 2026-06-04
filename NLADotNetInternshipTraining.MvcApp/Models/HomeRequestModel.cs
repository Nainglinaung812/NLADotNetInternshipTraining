namespace NLADotNetInternshipTraining.MvcApp.Models;

public class HomeRequestModel
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

public class HomeResponseModel
{
    public int id { get; set; }
    public string name { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

public class BlogListResponseModel
{
    public List<BlogModel> Data { get; set; }
    public int PageCount { get; set; }
    public int TotalRecords { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

public class BlogListRequestModel
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class BlogEditRequestModel
{
    public int Id { get; set; }
}

public class BlogEditResponseModel
{
    public BlogModel Data { get; set; }
}

public class BlogModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Content { get; set; } = null!;
}

public class BlogCreateRequestModel
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Content { get; set; } = null!;
}

public class BlogCreateResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
}

public class BlogUpdateRequestModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Content { get; set; } = null!;
}

public class PaginationModel
{
    public int PageCount { get; set; }
    public int TotalRecords { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}