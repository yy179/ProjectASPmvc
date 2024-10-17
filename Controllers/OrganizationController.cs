using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectLibrary.Models;
using ProjectLibrary.Services.Interfaces;

namespace Project.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IVolunteerService _volunteerService; 
        private readonly IRequestService _requestService;

        public OrganizationController(IOrganizationService organizationService, IVolunteerService volunteerService, IRequestService requestService)
        {
            _organizationService = organizationService;
            _volunteerService = volunteerService;
            _requestService = requestService;
        }

        public async Task<IActionResult> Index()
        {
            var organizations = await _organizationService.Get();
            return View(organizations);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var organization = await _organizationService.GetById(id);
            if (organization == null)
                return NotFound();
            return View(organization);
        }

        public async Task<IActionResult> Create()
        {
            var volunteers = await _volunteerService.Get();
            var requests = await _requestService.Get();
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name");
            ViewBag.Requests = new SelectList(requests, "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationEntity organization)
        {
            if (ModelState.IsValid)
            {
                organization.Id = Guid.NewGuid();
                await _organizationService.Add(
                    organization.Id,
                    organization.Name,
                    organization.City,
                    organization.Description,
                    organization.Volunteers,
                    organization.Requests);
                return RedirectToAction(nameof(Index));
            }
            var volunteers = await _volunteerService.Get();
            var requests = await _requestService.Get();
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name");
            ViewBag.Requests = new SelectList(requests, "Id", "Title");
            return View(organization);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var organization = await _organizationService.GetById(id);
            if (organization == null)
                return NotFound();

            var volunteers = await _volunteerService.Get();
            var requests = await _requestService.Get();
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name", organization.Volunteers);
            ViewBag.Requests = new SelectList(requests, "Id", "Title", organization.Requests);
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OrganizationEntity organization)
        {
            if (id != organization.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _organizationService.Update(
                    organization.Id,
                    organization.Name,
                    organization.City,
                    organization.Description,
                    organization.Volunteers,
                    organization.Requests);
                return RedirectToAction(nameof(Index));
            }

            var volunteers = await _volunteerService.Get();
            var requests = await _requestService.Get();
            ViewBag.Volunteers = new SelectList(volunteers, "Id", "Name", organization.Volunteers);
            ViewBag.Requests = new SelectList(requests, "Id", "Title", organization.Requests);
            return View(organization);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var organization = await _organizationService.GetById(id);
            if (organization == null)
                return NotFound();
            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _organizationService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
