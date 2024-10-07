using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Services.Interfaces;
using ProjectLibrary.Models;
namespace Project.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly IVolunteerService _volunteerService;
        public VolunteersController(IVolunteerService volunteerService)
        {
            _volunteerService = volunteerService;
        }
        public async Task<IActionResult> Index()
        {
            var volunteers = await _volunteerService.Get();
            return View(volunteers);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateOfBirth,City,Biography")] VolunteerEntity volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.Id = Guid.NewGuid();
                await _volunteerService.Add(volunteer.Id, volunteer.Name, volunteer.DateOfBirth, volunteer.City, volunteer.Biography, new List<OrganizationEntity>(), new List<RequestEntity>());
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DateOfBirth,City,Biography")] VolunteerEntity volunteer)
        {
            if (id != volunteer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _volunteerService.Update(volunteer.Id, volunteer.Name, volunteer.DateOfBirth, volunteer.City, volunteer.Biography, new List<OrganizationEntity>(), new List<RequestEntity>());
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var volunteer = await _volunteerService.GetById(id);
            if (volunteer == null)
            {
                return NotFound();
            }

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
