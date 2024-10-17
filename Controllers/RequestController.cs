using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace YourProject.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }


        public async Task<IActionResult> Index()
        {
            var requests = await _requestService.Get();
            return View(requests);
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _requestService.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShortDescription,LongDescription,SubmissionDate,DueDate,IsActive,MilitaryUnitId,OrganizationId,VolunteerId")] RequestEntity request)
        {
            if (ModelState.IsValid)
            {
                await _requestService.Add(request.Id, request.ShortDescription, request.LongDescription, request.SubmissionDate, request.DueDate, request.IsActive, request.MilitaryUnitId, request.OrganizationId, request.VolunteerId);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var request = await _requestService.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ShortDescription,LongDescription,SubmissionDate,DueDate,IsActive,MilitaryUnitId,OrganizationId,VolunteerId")] RequestEntity request)
        {
            if (ModelState.IsValid)
            {
                await _requestService.Update(request.Id, request.ShortDescription, request.LongDescription, request.SubmissionDate, request.DueDate, request.IsActive, request.MilitaryUnitId, request.OrganizationId, request.VolunteerId);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var request = await _requestService.GetById(id);
            if (request == null)
                return NotFound();

            return View(request);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _requestService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
