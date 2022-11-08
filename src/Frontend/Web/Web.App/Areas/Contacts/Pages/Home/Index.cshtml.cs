using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ContactBook;
using Core.Entities;

namespace Web.Areas.Contacts.Pages.Home;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IContactBookService _contactBook;
    private string _userId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

    public IndexModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    [BindProperty]
    public IEnumerable<Contact> Contacts { get; set; }

    public async Task OnGet()
    {
        Contacts = await _contactBook.GetContacts(_userId);
    }
}