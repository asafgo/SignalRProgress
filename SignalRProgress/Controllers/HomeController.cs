using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRProgress.Models;
using System.Diagnostics;

namespace SignalRProgress.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext<JobProgressHub> _hubContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHubContext<JobProgressHub> hubContext)
        {
            _logger = logger;

            //SignalR
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            string jobId = Guid.NewGuid().ToString("N");
            ViewBag.JobId = jobId;
            return View();
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


        [HttpPost]
        public async Task<IActionResult> InvokeSignalR(string jobId)
        {
            await Task.Delay(100).ConfigureAwait(false);
            ViewBag.JobId = jobId;
            Task task = Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    await _hubContext.Clients.Group(jobId).SendAsync("progress", i).ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                }
            });
            return View("Index");
        }

    }
}