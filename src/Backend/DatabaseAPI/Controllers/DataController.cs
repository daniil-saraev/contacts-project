using ContactsDatabaseAPI.Data;
using Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ContactsDatabaseAPI.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DataController> _logger;

        private string _userId => User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public DataController(ApplicationDbContext dbContext, ILogger<DataController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Returns a collection of all contacts in DB.
        /// </summary>
        [HttpGet("/getall")]
        public async Task<ActionResult<IEnumerable<Contact>?>> GetAllAsync()
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(GetAllAsync)}");
            if (_dbContext.Contacts != null)
                return Ok(await _dbContext.Contacts.Where(c => c.UserId == _userId).ToListAsync());
            else return Ok(null);
        }

        /// <summary>
        /// Returns a contact by id if not null.
        /// </summary>
        [HttpGet("/get/{contactId}")]
        public async Task<ActionResult<Contact?>> GetAsync(string contactId)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(GetAsync)}");
            var contact = await _dbContext.Contacts.FindAsync(contactId);
            if (contact.UserId != _userId)
                return BadRequest();
            return contact != null ? Ok(contact) : NotFound();
        }

        /// <summary>
        /// Adds a contact to dataset. Saves changes.
        /// </summary>
        [HttpPost("/add/")]
        public async Task<IActionResult> AddAsync([FromBody] Contact contact)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(AddAsync)}");
            contact.SetUserId(_userId);
            await _dbContext.Contacts.AddAsync(contact);
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Adds a collection of contacts to dataset. Saves changes.
        /// </summary>
        [HttpPost("/addrange")]
        public async Task<IActionResult> AddRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(AddRangeAsync)}");
            foreach (var contact in contacts)
            {
                contact.SetUserId(_userId);
            }
            await _dbContext.Contacts.AddRangeAsync(contacts);
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Removes a contact from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] Contact contact)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(DeleteAsync)}");
            if (contact.UserId != _userId)
                return BadRequest();
            _dbContext.Contacts.Remove(contact);
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Removes a collection of contacts from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/deleterange")]
        public async Task<IActionResult> DeleteRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(DeleteRangeAsync)}");
            if (!contacts.All(c => c.UserId == _userId))
                return BadRequest();
            _dbContext.Contacts.RemoveRange(contacts);
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Updates a contact. Saves changes.
        /// </summary>
        [HttpPut("/update")]
        public async Task<IActionResult> UpdateAsync([FromBody] Contact contact)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(UpdateAsync)}");
            if (contact.UserId != _userId)
                return BadRequest();
            _dbContext.Contacts.Update(contact);
            return Ok(await _dbContext.SaveChangesAsync());
        }

        /// <summary>
        /// Updates a range of contacts. Saves changes.
        /// </summary>
        [HttpPut("/updaterange")]
        public async Task<IActionResult> UpdateRangeAsync([FromBody] IEnumerable<Contact> contacts)
        {
            _logger.LogInformation($"User with id {_userId} invoked {nameof(UpdateRangeAsync)}");
            if (!contacts.All(c => c.UserId == _userId))
                return BadRequest();
            _dbContext.Contacts.UpdateRange(contacts);
            return Ok(await _dbContext.SaveChangesAsync());
        }
    }
}
