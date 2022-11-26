using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using Contacts.Common.Queries;
using Contacts.Common.Commands;
using Contacts.Common.Services;
using Core.Contacts.Models;
using Core.Contacts.Requests;

namespace Contacts.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public ContactsController(IMediator mediator, IMapper mapper, IUserInfoService userInfoService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// Returns a collection of all contacts in DB.
        /// </summary>
        [HttpGet("/getall")]
        public async Task<ActionResult<IEnumerable<ContactData>>> GetAllAsync()
        {
            var contacts = await _mediator.Send(new GetAllQuery
            {
                UserId = _userInfoService.UserId
            });
            return Ok(contacts);
        }

        /// <summary>
        /// Returns a contact by id if found.
        /// </summary>
        [HttpGet("/get/{contactId}")]
        public async Task<ActionResult<ContactData>> GetAsync(string contactId)
        {
            var contact = await _mediator.Send(new GetByIdQuery
            {
                Id = contactId,
                UserId = _userInfoService.UserId
            });
            return Ok(contact);
        }

        /// <summary>
        /// Adds a contact to dataset. Saves changes.
        /// </summary>
        [HttpPost("/add/")]
        public async Task<IActionResult> AddAsync([FromBody] AddContactRequest request)
        {
            CreateCommand createCommand = _mapper.Map<CreateCommand>(request);
            createCommand.UserId = _userInfoService.UserId;
            await _mediator.Send(createCommand);
            return Ok();
        }

        /// <summary>
        /// Removes a contact from dataset. Saves changes.
        /// </summary>
        [HttpDelete("/delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteContactRequest request)
        {
            DeleteCommand deleteCommand = new DeleteCommand
            {
                Id = request.Id,
                UserId = _userInfoService.UserId
            };
            await _mediator.Send(deleteCommand);
            return Ok();
        }

        /// <summary>
        /// Updates a contact. Saves changes.
        /// </summary>
        [HttpPut("/update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateContactRequest request)
        {
            UpdateCommand updateCommand = _mapper.Map<UpdateCommand>(request);
            updateCommand.UserId = _userInfoService.UserId;
            await _mediator.Send(updateCommand);
            return Ok();
        }
    }
}
