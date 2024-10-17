﻿using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class MilitaryUnitController : Controller
    {
        private readonly IMilitaryUnitService _militaryUnitService;
        public MilitaryUnitController(IMilitaryUnitService militaryUnitservice)
        {
            _militaryUnitService = militaryUnitservice;
        }

        public async Task<IActionResult> Index()
        {
            var militaryUnits = await _militaryUnitService.Get();
            return View(militaryUnits);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var militaryUnit = await _militaryUnitService.GetById(id);
            if (militaryUnit == null)
                return NotFound();
            return View(militaryUnit);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MilitaryUnitEntity militaryUnit)
        {
            if (ModelState.IsValid)
            {
                militaryUnit.Id = Guid.NewGuid();

                await _militaryUnitService.Add(militaryUnit.Id, militaryUnit.ContactPersonId, militaryUnit.Name, militaryUnit.Requests);
                return RedirectToAction(nameof(Index));
            }
            return View(militaryUnit);
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var militaryUnit = await _militaryUnitService.GetById(id);
            if (militaryUnit == null)
                return NotFound();
            return View(militaryUnit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MilitaryUnitEntity militaryUnit)
        {
            if (id != militaryUnit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _militaryUnitService.Update(militaryUnit.Id, militaryUnit.ContactPersonId, militaryUnit.Name, militaryUnit.Requests);
                return RedirectToAction(nameof(Index));
            }
            return View(militaryUnit);
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var militaryUnit = await _militaryUnitService.GetById(id);
            if (militaryUnit == null)
                return NotFound();
            return View(militaryUnit);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _militaryUnitService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
