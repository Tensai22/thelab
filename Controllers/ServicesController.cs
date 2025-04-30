using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheLab.Filters;
using TheLab.Models;
using TheLab.Service;
using TheLab.Services; 

namespace TheLab.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(CacheResultFilter))]
    public class ServicesController : Controller
    {
        private readonly ReviewService _reviewService; 

        public ServicesController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Testimonials()
        {
            var reviews = _reviewService.GetAllReviews();

            return View(reviews);
        }

        public IActionResult Pricing()
        {
            return View();
        }
    }
}
