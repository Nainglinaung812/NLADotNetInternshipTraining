using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLADotNetInternshipTraining.RealtimeChatApp.Hubs;
using NLADotNetInternshipTraining.RealtimeChatApp.Models;
using System.Collections.Concurrent;

namespace NLADotNetInternshipTraining.RealtimeChatApp.Controllers;
public class PieChartController : Controller
{
    private readonly IHubContext<ChartHub> _hubContext;

    // In-memory data store for dummy data
    private static readonly ConcurrentDictionary<string, double> _chartData = new(new Dictionary<string, double>
    {
        { "Chrome", 61.4 },
        { "Safari", 15.2 },
        { "Firefox", 4.8 },
        { "Edge", 9.6 },
        { "Opera", 2.3 },
        { "Others", 6.7 }
    });

    public PieChartController(IHubContext<ChartHub> hubContext)
    {
        _hubContext = hubContext;
    }

    // GET: PieChart
    public IActionResult Index()
    {
        return View();
    }

    // GET: PieChart/GetData
    [HttpGet]
    public IActionResult GetData()
    {
        var data = _chartData.Select(kvp => new PieChartDataPoint
        {
            Name = kvp.Key,
            Y = kvp.Value
        }).ToList();

        return Json(data);
    }

    // POST: PieChart/UpdateData
    [HttpPost]
    public async Task<IActionResult> UpdateData([FromBody] PieChartDataPoint model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Name))
        {
            return BadRequest(new { success = false, message = "Invalid data. Name is required." });
        }

        // Clean the name
        string name = model.Name.Trim();

        // Update or add the value
        _chartData.AddOrUpdate(name, model.Y, (key, oldValue) => model.Y);

        // Get the full updated list
        var updatedList = _chartData.Select(kvp => new PieChartDataPoint
        {
            Name = kvp.Key,
            Y = kvp.Value
        }).ToList();

        // Broadcast to all connected clients
        await _hubContext.Clients.All.SendAsync("UpdateChartData", updatedList);

        return Json(new { success = true, data = updatedList });
    }

    // POST: PieChart/DeleteData
    [HttpPost]
    public async Task<IActionResult> DeleteData([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { success = false, message = "Invalid data. Name is required." });
        }

        string trimmedName = name.Trim();
        if (_chartData.TryRemove(trimmedName, out _))
        {
            var updatedList = _chartData.Select(kvp => new PieChartDataPoint
            {
                Name = kvp.Key,
                Y = kvp.Value
            }).ToList();

            // Broadcast updated list to all clients
            await _hubContext.Clients.All.SendAsync("UpdateChartData", updatedList);
            return Json(new { success = true, data = updatedList });
        }

        return NotFound(new { success = false, message = "Item not found." });
    }
}