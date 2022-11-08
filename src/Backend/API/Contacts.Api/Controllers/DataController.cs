using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Contacts.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class DataController : ControllerBase
    {
        private readonly IContactsRepository _repository;

        private string _userId => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public DataController(IContactsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns a collection of all contacts in DB.
        /// </summary>
        [HttpGet("/getall")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllAsync()
        {
            return Ok(await _repository.GetAllAsync(_userId));
        }

        /// <summary>
        /// Returns a contact by id if not null.
        /// </summary>
        [HttpGet("/get/{contactId}")]
        public async Task<ActionResult<Contact?>> GetAsync(string contactId)
        {
            var contact = await _repository.GetAsync(contactId, _userId);
            return Ok(contact);
        }

        /// <summary>
        /// Adds a contact to dataset. Saves changes.
        /// </summary>
        [HttpPost("/add/")]
        public async Task<IActionResult> AddAsync([FromBody] Contact contact)
        {
            contact.UserId = _userId;
            await _repository.AddAsync(contact);
            return Ok();
        }

        /// <summary>
        /// Adds a collection of contacts to dataset. Saves changes.
        /// </summary>
        [HttpPost("/addrange")]
        public async Task<IActionResult> AddRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            foreach (var contact in contacts)
            {
                contact.UserId = _userId;
            }
            await _repository.AddRangeAsync(contacts);
            return Ok();
        }

        /// <summary>
        /// Removes a contact from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] Contact contact)
        {
            await _repository.DeleteAsync(contact);
            return Ok();
        }

        /// <summary>
        /// Removes a collection of contacts from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/deleterange")]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            await _repository.DeleteRangeAsync(contacts);
            return Ok();
        }

        /// <summary>
        /// Updates a contact. Saves changes.
        /// </summary>
        [HttpPut("/update")]
        public async Task<IActionResult> UpdateAsync([FromBody] Contact contact)
        {
            await _repository.UpdateAsync(contact);
            return Ok();
        }

        /// <summary>
        /// Updates a range of contacts. Saves changes.
        /// </summary>
        [HttpPut("/updaterange")]
        public async Task<IActionResult> UpdateRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            await _repository.UpdateRangeAsync(contacts);
            return Ok();
        }
    }
}
