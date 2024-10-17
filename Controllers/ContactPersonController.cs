using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ContactPersonController : Controller
    {
        private readonly IContactPersonService _contactPersonService;
        private readonly IMilitaryUnitService _militaryUnitService;

        public ContactPersonController(IContactPersonService contactPersonService, IMilitaryUnitService militaryUnitService)
        {
            _contactPersonService = contactPersonService;
            _militaryUnitService = militaryUnitService;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactPersonService.Get();
            return View(contacts);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var contactPerson = await _contactPersonService.GetById(id);
            if (contactPerson == null)
                return NotFound();
            return View(contactPerson);
        }
        public async Task<IActionResult> Create()
        {
            var militaryUnits = await _militaryUnitService.Get();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactPersonEntity contactPerson)
        {
            if (ModelState.IsValid)
            {
                contactPerson.Id = Guid.NewGuid();
                Guid militaryUnitId = contactPerson.MilitaryUnitId ?? Guid.Empty;

                await _contactPersonService.Add(contactPerson.Id, militaryUnitId, contactPerson.Name, contactPerson.Surname, contactPerson.DateOfBirth, contactPerson.Address);
                return RedirectToAction(nameof(Index));
            }
            return View(contactPerson);
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var contactPerson = await _contactPersonService.GetById(id);
            if (contactPerson == null)
            {
                return NotFound();
            }
            var militaryUnits = await _militaryUnitService.Get();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            return View(contactPerson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContactPersonEntity contactPerson)
        {
            if (id != contactPerson.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Guid militaryUnitId = contactPerson.MilitaryUnitId ?? Guid.Empty;

                await _contactPersonService.Update(contactPerson.Id, militaryUnitId, contactPerson.Name, contactPerson.Surname, contactPerson.DateOfBirth, contactPerson.Address);
                return RedirectToAction(nameof(Index));
            }
            return View(contactPerson);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var contactPerson = await _contactPersonService.GetById(id);
            if (contactPerson == null)
            {
                return NotFound();
            }
            return View(contactPerson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _contactPersonService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
