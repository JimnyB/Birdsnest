using Microsoft.AspNetCore.Mvc;

namespace Birdsnest.Controllers
{
    public class StreamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartLiveFeed()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://192.168.0.34:5000/start_feed").Result;
                ViewBag.Result = response.IsSuccessStatusCode ? "Feed started successfully." : "Failed to start feed.";
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult StopLiveFeed()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://192.168.0.34:5000/stop_feed").Result;
                ViewBag.Result = response.IsSuccessStatusCode ? "Feed stopped successfully." : "Failed to stop feed.";
            }
            return View("Index");
        }
    }
}
