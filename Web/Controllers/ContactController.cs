using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DatabaseApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactController : Controller
    {
        private readonly IRepository<Contact> _contactsDbApi;
        private readonly ILogger<ContactController> _logger;

        private string _userId => !User.Identity.IsAuthenticated
            ? string.Empty
            : User.FindFirst(c => c.Type == "id").Value;

        public ContactController(IRepository<Contact> contactsDbApi, ILogger<ContactController> logger)
        {
            _contactsDbApi = contactsDbApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var contacts = await _contactsDbApi.GetAllAsync();
                contacts ??= new List<Contact>();
                return View(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }        
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
            try
            {
                await _contactsDbApi.DeleteAsync(contactModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }         
        }

        [HttpGet]
        public IActionResult Add()
        {
            Contact contactModel = new Contact
            {
                UserId = _userId
            };       
            return View(contactModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Contact contactModel)
        {
            if (!ModelState.IsValid)
            {
                return View(contactModel);
            }
            try
            {           
                await _contactsDbApi.AddAsync(contactModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(contactModel);
            }                  
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
            if (!ModelState.IsValid)
            {
                return View(contactModel);               
            }
            try
            {
                await _contactsDbApi.UpdateAsync(contactModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(contactModel);
            }       
        }
    }
}
