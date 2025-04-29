using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace TheLab.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult OurTeam()
        {
            return View();
        }
        //public async Task<IActionResult> Index()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        var token = Request.Cookies["jwtToken"];

        //        client.DefaultRequestHeaders.Authorization =
        //            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        using (var request = await client.GetAsync(""))
        //        {
        //            if (request.StatusCode == HttpStatusCode.Ok)
        //            {
        //                var result = await request.Content.ReadAsStringAsync();
        //                teams = JsonConvert.DeserializeObject<List<Team>>(result);
        //            }
        //            else if (request.StatusCode == HttpStatusCode.Unauthorized)
        //            {
                        
        //            }

        //        }
        //    }
        //}
    }
}
