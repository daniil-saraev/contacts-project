using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.App.Areas.Contacts.ViewModels;
using Web.Authentication;
using Core.Contacts.Requests;
using Core.Contacts.Interfaces;

namespace Web.App.Areas.Contacts.Pages.Home;

[Authorize]
[TypeFilter(typeof(RefreshTokenFilter))]
public class AddModel : PageModel
{
    private readonly IContactBookService _contactBook;

    public AddModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    [BindProperty]
    public ContactViewModel ContactViewModel { get; set; }

    public void OnGet()
    {
        ContactViewModel = new ContactViewModel();
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var addRequest = new AddContactRequest()
            {
                FirstName = ContactViewModel.FirstName,
                MiddleName = ContactViewModel.MiddleName,
                LastName = ContactViewModel.LastName,
                PhoneNumber = ContactViewModel.PhoneNumber,
                Address = ContactViewModel.Address,
                Description = ContactViewModel.Description
            };
            await _contactBook.AddContact(addRequest);
            return RedirectToPage("Home");
        }
        else
            return Page();
    }
}