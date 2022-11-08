using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Web.Areas.Contacts.ViewModels;
using ContactBook;

namespace Web.Areas.Contacts.Pages.Home;

[Authorize]
public class AddModel : PageModel
{
    private readonly IContactBookService _contactBook;
    private string _userId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

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
            var contact = new Contact()
            {
                UserId = _userId,
                FirstName = ContactViewModel.FirstName,
                MiddleName = ContactViewModel.MiddleName,
                LastName = ContactViewModel.LastName,
                PhoneNumber = ContactViewModel.PhoneNumber,
                Address = ContactViewModel.Address,
                Description = ContactViewModel.Description
            };
            await _contactBook.AddContact(contact);
            return RedirectToPage("Home");
        }
        else
            return Page();
    }
}