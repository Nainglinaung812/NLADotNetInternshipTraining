using Microsoft.AspNetCore.Mvc;

namespace NLADotNetInternshipTraining.MvcThemeApp.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}