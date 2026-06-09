using Dapper;
using NLADotNetInternshipTraining.ApexChartRealtimeSignalr.Hubs;
using NLADotNetInternshipTraining.ApexChartRealtimeSignalr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace NLADotNetInternshipTraining.ApexChartRealtimeSignalr.Controllers;

public class TeamController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHubContext<NotiHub> _hubContext;

    public TeamController(IConfiguration configuration, IHubContext<NotiHub> hubContext)
    {
        _configuration = configuration;
        _hubContext = hubContext;
    }

    public IActionResult Index()
    {
        var lst = GetData();
        return View(lst);
    }

    private List<TeamModel> GetData()
    {
        using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        db.Open();
        var lst = db.Query<TeamModel>("select * from tbl_team with (nolock)").ToList();
        return lst;
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(TeamModel requestModel)
    {
        using IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        db.Open();
        db.Execute("insert into tbl_team (TeamName, TeamValue) values (@TeamName, @TeamValue)", requestModel);

        var lst = GetData();

        //var jsonStr = JsonConvert.SerializeObject(lst);

        List<int> _series = lst.Select(x => x.TeamValue).ToList();
        List<string> _labels = lst.Select(x => x.TeamName).ToList();

        await _hubContext.Clients.All.SendAsync("NotiReceiveMessage", _series, _labels);

        return Json(new
        {
            IsSuccess = true,
            Message = "Team added successfully"
        });
    }
}