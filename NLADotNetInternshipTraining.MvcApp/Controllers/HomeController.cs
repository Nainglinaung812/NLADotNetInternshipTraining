using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NLADotNetInternshipTraining.MvcApp.Models;

namespace NLADotNetInternshipTraining.MvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index([FromQuery] HomeRequestModel requestModel)
    {
        HomeResponseModel model = new HomeResponseModel()
        {
            id = requestModel.PageNo,
            name = $"Name {requestModel.PageNo}"
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

