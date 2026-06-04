using System.Text.Json;
using Newtonsoft.Json;


Console.WriteLine("Hello, World!");
System.Console.WriteLine("Api Waiting...");
Console.ReadLine();

HttpClient client = new HttpClient();
string baseUrl = "https://localhost:7196/api/blog";
await ReadBlogs();
await CreateBlog();
int blogIdToPut = 6007;
await UpdateBlogPut();
int blogIdToPatch = 6007;
await UpdateBlogPatch();
int blogIdToDelete = 6007;
await DeleteBlog();

Console.WriteLine("\n--- All Operations Completed ---");
Console.ReadLine();


#region CRUD Methods

async Task ReadBlogs()
{
    System.Console.WriteLine("\n[READ] Fetching all blogs...");
    var response = await client.GetAsync(baseUrl);
    
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        System.Console.WriteLine("Blogs fetched successfully.");
    }
    else
    {
        System.Console.WriteLine($"GET Failed: {response.StatusCode}");
    }
    Console.ReadLine();
}

async Task CreateBlog()
{
    System.Console.WriteLine("\n[CREATE] Creating a new blog...");
    
    var requestModel = new BlogCreateRequestModel
    {
        BlogTitle = "Programming C#",
        BlogAuthor = "Naing Lin Aung",
        BlogContent = "Testing "
    };
    
    var contentJson = JsonConvert.SerializeObject(requestModel);
    var stringContent = new StringContent(contentJson, System.Text.Encoding.UTF8, "application/json");

    var response = await client.PostAsync(baseUrl, stringContent);
    
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        System.Console.WriteLine("Blog Created Successfully");
    }
    else
    {
        System.Console.WriteLine($"POST Failed: {response.StatusCode}");
    }
    Console.ReadLine();
}

async Task UpdateBlogPut()
{
    System.Console.WriteLine($"\n[PUT] Fully updating blog ID: {blogIdToPut}...");
    
    var putRequestModel = new BlogUpdateRequestModel
    {
        BlogTitle = "Fully Updated Title via PUT",
        BlogAuthor = "Naing Lin Aung (Edited)",
        BlogContent = "Fully updated content body via PUT request."
    };

    var putJson = JsonConvert.SerializeObject(putRequestModel);
    var putStringContent = new StringContent(putJson, System.Text.Encoding.UTF8, "application/json");

    var response = await client.PutAsync($"{baseUrl}/{blogIdToPut}", putStringContent);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        System.Console.WriteLine("Blog Fully Updated Successfully via PUT");
    }
    else
    {
        System.Console.WriteLine($"PUT Failed: {response.StatusCode}");
    }
    Console.ReadLine();
}

async Task UpdateBlogPatch()
{
    System.Console.WriteLine($"\n[PATCH] Partially updating blog ID: {blogIdToPatch}...");
    
    var patchData = new BlogPatchRequestModel
    {
        BlogTitle = "Updated C# Programming Title",
        BlogContent = "Updated Content via PATCH"
    };
    
    var patchJson = JsonConvert.SerializeObject(patchData);
    var patchStringContent = new StringContent(patchJson, System.Text.Encoding.UTF8, "application/json");

    var response = await client.PatchAsync($"{baseUrl}/{blogIdToPatch}", patchStringContent);
    
    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        System.Console.WriteLine("Blog Updated Successfully via PATCH");
    }
    else
    {
        System.Console.WriteLine($"PATCH Failed: {response.StatusCode}");
    }
    Console.ReadLine();
}

async Task DeleteBlog()
{
    System.Console.WriteLine($"\n[DELETE] Preparing to delete blog ID: {blogIdToDelete}...");
    System.Console.WriteLine("Are you sure to delete this blog? (y/n)");
    string? userInput = Console.ReadLine();

    if (userInput?.ToLower() == "y")
    {
        var response = await client.DeleteAsync($"{baseUrl}/{blogIdToDelete}");
        
        if (response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("Blog Deleted Successfully");
        }
        else
        {
            System.Console.WriteLine($"DELETE Failed: {response.StatusCode}");
        }
    }
    else
    {
        System.Console.WriteLine("Delete operation cancelled by user.");
    }
    Console.ReadLine();
}

#endregion


#region Request Models

public class BlogCreateRequestModel
{
    public string? BlogAuthor { get; set; }
    public string? BlogTitle { get; set; }
    public string? BlogContent { get; set; }
}

public class BlogUpdateRequestModel
{
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

#endregion