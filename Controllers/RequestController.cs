using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace YourProject.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IMilitaryUnitService _militaryUnitService;
        private readonly IOrganizationService _organizationService;
        public RequestController(IRequestService requestService, IMilitaryUnitService militaryUnitService, IOrganizationService organizationService)
        {
            _requestService = requestService;
            _militaryUnitService = militaryUnitService;
            _organizationService = organizationService;
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


        public async Task<IActionResult> Create()
        {
            var militaryUnits = await _militaryUnitService.Get();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            var organizationService = await _organizationService.Get();
            ViewBag.Organizations = new SelectList(organizationService, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShortDescription,LongDescription,SubmissionDate,DueDate,IsActive,MilitaryUnitId,OrganizationId,VolunteerId")] RequestEntity request)
        {
            ModelState.Remove("MilitaryUnit");
            if (ModelState.IsValid)
            {
                request.Id = Guid.NewGuid();
                Guid organizationId = request.OrganizationId ?? Guid.Empty;
                Guid volunteerId = request.VolunteerId ?? Guid.Empty;
                Console.WriteLine($"OrganizationId: {request.OrganizationId}");

                await _requestService.Add(
                    request.Id,
                    request.ShortDescription,
                    request.LongDescription,
                    request.SubmissionDate,
                    request.DueDate,
                    request.IsActive,
                    request.MilitaryUnitId,
                    organizationId,
                    volunteerId
                );
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
        public async Task<IActionResult> Edit(Guid id, RequestEntity request)
        {
            if (ModelState.IsValid)
            {
                Guid organizationId = request.OrganizationId ?? Guid.Empty;
                Guid volunteerId = request.VolunteerId ?? Guid.Empty;

                await _requestService.Update(
                    request.Id,
                    request.ShortDescription,
                    request.LongDescription,
                    request.SubmissionDate,
                    request.DueDate,
                    request.IsActive,
                    request.MilitaryUnitId,
                    organizationId,
                    volunteerId
                );
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
