using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IOrganizationService _organizationService;
        public MilitaryUnitController(IMilitaryUnitService militaryUnitservice, IRequestService requestService, IOrganizationService organizationService)
        {
            _militaryUnitService = militaryUnitservice;
            _requestService = requestService;
            _organizationService = organizationService;

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


        public async Task<IActionResult> Create()
        {
            var militaryUnits = await _militaryUnitService.Get();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            var organizations = await _organizationService.Get();
            ViewBag.Organizations = new SelectList(organizations, "Id", "Name");
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
            var request = await _requestService.GetById(id);
            if (request == null)
                return NotFound();

            var militaryUnits = await _militaryUnitService.Get();
            ViewBag.MilitaryUnits = new SelectList(militaryUnits, "Id", "Name");
            var organizations = await _organizationService.Get();
            ViewBag.Organizations = new SelectList(organizations, "Id", "Name");

            return View(request);
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
