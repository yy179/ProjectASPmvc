using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class AnalyticController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProcessedRequestsCount(TimeSpan period)
        {
            var count = await _analyticsService.GetProcessedRequestsCount(period);
            return View("ProcessedRequestsCount", count);
        }

        public async Task<IActionResult> AverageRequestCompletionTime(TimeSpan period)
        {
            var averageTime = await _analyticsService.GetAverageRequestCompletionTime(period);
            return View("AverageRequestCompletionTime", averageTime);
        }

        public async Task<IActionResult> TopVolunteers(int topCount = 5)
        {
            var volunteers = await _analyticsService.GetTopVolunteers(topCount);
            return View("TopVolunteers", volunteers);
        }

        public async Task<IActionResult> VolunteerCount()
        {
            var count = await _analyticsService.GetVolunteerCount();
            return View("VolunteerCount", count);
        }

        public async Task<IActionResult> OrganizationCount()
        {
            var count = await _analyticsService.GetOrganizationCount();
            return View("OrganizationCount", count);
        }
    }
}
