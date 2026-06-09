using NLADotNetInternshipTraining.HighChartApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace NLADotNetInternshipTraining.HighChartApp.Controllers;

public class PieChartController : Controller
{
    public IActionResult Index()
    {
        var salesData = new List<SalesCategoryData>
{
new()
{
    name = "Electronics",
    y = 125000
},
new()
{
    name = "Mobile Phones",
    y = 285000,
    sliced = true,
    selected = true
},
new()
{
    name = "Computers",
    y = 95000
},
new()
{
    name = "Accessories",
    y = 45000
},
new()
{
    name = "Home Appliances",
    y = 78000
},
new()
{
    name = "Others",
    y = 22000
}
};
        return View(salesData);
    }
}
