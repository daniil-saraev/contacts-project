using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Interfaces;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactController : Controller
    {
        private readonly IRepository<Contact> _contactsDbApi;

        private string _userId => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

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
        public async Task<IActionResult> Info(string id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) 
                return NotFound();
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) 
                return NotFound();
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
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }
            await _contactsDbApi.AddAsync(contactModel);
            return RedirectToAction("Index");              
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            Contact? contactModel = await _contactsDbApi.GetAsync(id);
            if (contactModel == null) return NotFound();
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Contact contactModel)
        {
            if (!ModelState.IsValid)
            {
                return View(contactModel);               
            }
            await _contactsDbApi.UpdateAsync(contactModel);
            return RedirectToAction("Index");    
        }
    }
}
