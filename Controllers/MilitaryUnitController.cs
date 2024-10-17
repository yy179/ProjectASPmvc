using Microsoft.AspNetCore.Mvc;
using ProjectLibrary.Models;
using ProjectLibrary.Services;
using ProjectLibrary.Services.Interfaces;
using System.Threading.Tasks;

namespace Project.Controllers
{
    public class MilitaryUnitController : Controller
    {
        private readonly IMilitaryUnitService _militaryUnitService;
        private readonly IRequestService _requestService;

        public MilitaryUnitController(IMilitaryUnitService militaryUnitservice, IRequestService requestService)
        {
            _militaryUnitService = militaryUnitservice;
            _requestService = requestService;
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


            var requests = await _militaryUnitService.GetRequestsByMilitaryUnit(id);
            ViewBag.Requests = requests;

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
                Guid contactPersonId = militaryUnit.ContactPersonId ?? Guid.Empty;

                await _militaryUnitService.Add(militaryUnit.Id, contactPersonId, militaryUnit.Name, militaryUnit.Requests);
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
                Guid contactPersonId = militaryUnit.ContactPersonId ?? Guid.Empty;

                await _militaryUnitService.Update(militaryUnit.Id, contactPersonId, militaryUnit.Name, militaryUnit.Requests);
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
            var militaryUnit = await _militaryUnitService.GetById(id); 

            await _militaryUnitService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
