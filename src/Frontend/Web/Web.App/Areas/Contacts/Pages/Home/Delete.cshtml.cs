using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ContactBook;

namespace Web.Areas.Contacts.Pages.Home;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly IContactBookService _contactBook;
    private string _userId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

    public DeleteModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        var contact = (await _contactBook.GetContacts(_userId, contact => contact.Id == id)).FirstOrDefault(defaultValue: null);
        if (contact == null)
            return NotFound();
        await _contactBook.DeleteContact(contact);
        return RedirectToPage("Home");
    }
}