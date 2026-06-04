namespace NLADotNetInternshipTraining.WebApi.Models;

public class BlogCreateRequestModel
{
    public string? BlogAuthor { get; set; }
    public string? BlogTitle { get; set; }

    public string? BlogContent { get; set; }

}
public class BlogCreateResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}
public class BlogUpdateRequestModel
{

    public string? BlogAuthor { get; set; }
    public string? BlogTitle { get; set; }

    public string? BlogContent { get; set; }
}

public class BlogUpdateResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public BlogModel? Data { get; set; }
}
public class BlogModel
{
    public int BlogId { get; set; }
    public string? BlogAuthor { get; set; }
    public string? BlogTitle { get; set; }
    public string? BlogContent { get; set; }
}

public class BlogPatchRequestModel
{

    public string? BlogAuthor { get; set; }
    public string? BlogTitle { get; set; }

    public string? BlogContent { get; set; }
}
public class BlogPatchResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public BlogModel? Data { get; set; }

}
public class BlogDeleteResponseModel
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }



}