using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IRepository<Contact> _contactsDbApi;

        public ContactController(IRepository<Contact> contactsDbApi)
        {
            _contactsDbApi = contactsDbApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactsDbApi.GetAllAsync();
            contacts ??= new List<Contact>();
            return View(contacts);
        }

        [HttpGet]
        public async Task<IActionResult> Info(int id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) return NotFound();
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) return NotFound();
            await _contactsDbApi.DeleteAsync(contactModel);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            Contact contactModel = new Contact();
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Contact contactModel)
        {
            if (ModelState.IsValid)
            {
                contactModel.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _contactsDbApi.AddAsync(contactModel);
                return RedirectToAction("Index");
            }
            return View(contactModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) return NotFound();
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contact contactModel)
        {
            if (ModelState.IsValid)
            {
                await _contactsDbApi.UpdateAsync(contactModel);
                return RedirectToAction("Index");
            }
            return View(contactModel);
        }
    }
}
