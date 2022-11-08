using System.Security.Claims;
using ContactBook;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Areas.Contacts.ViewModels;

namespace Web.Areas.Contacts.Pages.Home;

[Authorize]
public class InfoModel : PageModel
{
    private readonly IContactBookService _contactBook;
    private string _userId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

    public InfoModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    [BindProperty]
    public ContactViewModel ContactViewModel { get; set; }

    public async Task<IActionResult> OnGet(string id)
    {
        var contact = (await _contactBook.GetContacts(_userId, contact => contact.Id == id)).FirstOrDefault(defaultValue: null);
        if (contact == null)
            return NotFound();
        ContactViewModel = new ContactViewModel(contact);
        return Page();
    }
}