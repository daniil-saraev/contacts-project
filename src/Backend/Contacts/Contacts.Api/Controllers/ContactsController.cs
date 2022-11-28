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
        /// Returns a collection of <see cref="ContactData"/> for all user's contacts.
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
        /// Searches for a user's contact by id.
        /// </summary>
        /// <returns>
        /// <see cref="ContactData"/> for the contact if found.
        /// </returns>
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
        /// Creates a contact from request.
        /// </summary>
        /// <returns>
        /// <see cref="ContactData"/> for the created contact.
        /// </returns>
        [HttpPost("/add/")]
        public async Task<ActionResult<ContactData>> AddAsync([FromBody] AddContactRequest request)
        {
            CreateRequest createRequest = _mapper.Map<CreateRequest>(request);
            createRequest.UserId = _userInfoService.UserId;
            var contact = await _mediator.Send(createRequest);
            return Ok(contact);
        }

        /// <summary>
        /// Removes a contact found from request.
        /// </summary>
        [HttpDelete("/delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteContactRequest request)
        {
            DeleteRequest deleteRequest = new DeleteRequest
            {
                Id = request.Id,
                UserId = _userInfoService.UserId
            };
            await _mediator.Send(deleteRequest);
            return Ok();
        }

        /// <summary>
        /// Updates a contact found from request.
        /// </summary>
        /// <returns>
        /// <see cref="ContactData"/> for the updated contact.
        /// </returns>
        [HttpPut("/update")]
        public async Task<ActionResult<ContactData>> UpdateAsync([FromBody] UpdateContactRequest request)
        {
            UpdateRequest updateRequest = _mapper.Map<UpdateRequest>(request);
            updateRequest.UserId = _userInfoService.UserId;
            var contact = await _mediator.Send(updateRequest);
            return Ok(contact);
        }
    }
}
