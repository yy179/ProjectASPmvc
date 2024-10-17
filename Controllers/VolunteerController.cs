using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly IVolunteerService _volunteerService;
        private readonly IOrganizationService _organizationService;

        public VolunteerController(IVolunteerService volunteerService, IOrganizationService organizationService)
        {
            _volunteerService = volunteerService;
            _organizationService = organizationService;
        }

        public async Task<IActionResult> Index()
        {
            var volunteers = await _volunteerService.Get();
            return View(volunteers);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
                return NotFound();
            return View(volunteer);
        }

        public async Task<IActionResult> Create()
        {
            var organizations = await _organizationService.Get();
            ViewBag.Organizations = new SelectList(organizations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerEntity volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.Id = Guid.NewGuid();

                await _volunteerService.Add(volunteer.Id, volunteer.Name, volunteer.DateOfBirth, volunteer.City, volunteer.Biography, volunteer.Organizations, volunteer.Requests);
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
                return NotFound();
            var organizations = await _organizationService.Get();
            ViewBag.Organizations = new SelectList(organizations, "Id", "Name");
            return View(volunteer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VolunteerEntity volunteer)
        {
            if (id != volunteer.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _volunteerService.Update(volunteer.Id, volunteer.Name, volunteer.DateOfBirth, volunteer.City, volunteer.Biography, volunteer.Organizations, volunteer.Requests);
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
                return NotFound();
            return View(volunteer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _volunteerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
