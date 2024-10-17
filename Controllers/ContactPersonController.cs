using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Models;
using ProjectLibrary.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class ContactPersonController : Controller
    {
        private readonly IContactPersonService _contactPersonService;

        public ContactPersonController(IContactPersonService contactPersonService)
        {
            _contactPersonService = contactPersonService;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactPersonEntity contactPerson)
        {
            if (ModelState.IsValid)
            {
                await _contactPersonService.Add(contactPerson.Id, contactPerson.MilitaryUnitId, contactPerson.Name, contactPerson.Surname, contactPerson.DateOfBirth, contactPerson.Address);
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
                await _contactPersonService.Update(contactPerson.Id, contactPerson.MilitaryUnitId, contactPerson.Name, contactPerson.Surname, contactPerson.DateOfBirth, contactPerson.Address);
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
