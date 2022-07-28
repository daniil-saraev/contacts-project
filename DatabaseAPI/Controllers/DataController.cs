using ContactsWebAPI.Data;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ContactsWebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    [ValidateAntiForgeryToken]
    public class DataController : ControllerBase
    {
		private readonly ApplicationDbContext _dbContext;
        private string UserId => !User.Identity.IsAuthenticated
            ? string.Empty
            : User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public DataController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns a collection of all contacts in DB.
        /// </summary>
        [HttpGet("/getall")]
        public async Task<IEnumerable<Contact>?> GetAll()
        {
            if (_dbContext.Contacts != null)
                return await _dbContext.Contacts.Where(c => c.UserId == UserId).ToListAsync();
            else return null;
        }

        /// <summary>
        /// Returns a contact by id if not null.
        /// </summary>
        [HttpGet("/get/{contactId}")]
        public async Task<Contact?> GetAsync(int contactId)
        {
            var contact = await _dbContext.Contacts.FindAsync(contactId);
            if (contact != null && contact.UserId == UserId)
                return contact;
            else return null;
        }

        /// <summary>
        /// Adds a contact to dataset. Saves changes.
        /// </summary>
        [HttpPost("/add/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddAsync([FromBody] Contact contact)
        {
            try
            {
                await _dbContext.Contacts.AddAsync(contact);
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adds a collection of contacts to dataset. Saves changes.
        /// </summary>
        [HttpPost("/addrange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            try
            {
                await _dbContext.Contacts.AddRangeAsync(contacts);
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Removes a contact from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync([FromBody] Contact contact)
        {
            if (contact.UserId != UserId)
                return BadRequest();
            try
            {
                _dbContext.Contacts.Remove(contact);
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Removes a collection of contacts from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/deleterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            if (!contacts.All(c => c.UserId == UserId))
                return BadRequest();
            try
            {
                _dbContext.Contacts.RemoveRange(contacts);               
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        private async Task<IActionResult> SaveChangesAsync()
        {
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Updates a contact. Saves changes.
        /// </summary>
        [HttpPut("/update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] Contact contact)
        {
            if (contact.UserId != UserId)
                return BadRequest();
            try
            {
                _dbContext.Contacts.Update(contact);               
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a range of contacts. Saves changes.
        /// </summary>
        [HttpPut("/updaterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            if(!contacts.All(c => c.UserId == UserId))
                return BadRequest();
            try
            {
                _dbContext.Contacts.UpdateRange(contacts);
                return Ok(await SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
            }           
        }
    }
}
