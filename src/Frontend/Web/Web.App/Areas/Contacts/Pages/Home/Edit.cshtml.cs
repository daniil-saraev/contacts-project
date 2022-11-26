using Core.Contacts.Requests;
using Core.Contacts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.App.Areas.Contacts.ViewModels;
using Web.Authentication;

namespace Web.App.Areas.Contacts.Pages.Home;

[Authorize]
[TypeFilter(typeof(RefreshTokenFilter))]
public class EditModel : PageModel
{
    private readonly IContactBookService _contactBook;

    public EditModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    [BindProperty]
    public ContactViewModel ContactViewModel { get; set; }

    public async Task<IActionResult> OnGet(string id)
    {
        var contact = await _contactBook.GetContactById(id);
        ContactViewModel = new ContactViewModel(contact);
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var updateRequest = new UpdateContactRequest
            {
                Id = ContactViewModel.Id,
                FirstName = ContactViewModel.FirstName,
                MiddleName = ContactViewModel.MiddleName,
                LastName = ContactViewModel.LastName,
                PhoneNumber = ContactViewModel.PhoneNumber,
                Address = ContactViewModel.Address,
                Description = ContactViewModel.Description
            };
            await _contactBook.UpdateContact(updateRequest);
            return RedirectToPage("Home");
        }
        else
            return Page();
    }
}