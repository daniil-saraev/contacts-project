using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.App.Areas.Contacts.ViewModels;
using Web.Authentication;
using Core.Contacts.Interfaces;

namespace Web.App.Areas.Contacts.Pages.Home;

[Authorize]
[TypeFilter(typeof(RefreshTokenFilter))]
public class InfoModel : PageModel
{
    private readonly IContactBookService _contactBook;

    public InfoModel(IContactBookService contactBookService)
    {
        _contactBook = contactBookService;
    }

    [BindProperty]
    public ContactViewModel ContactViewModel { get; set; }

    public async Task<IActionResult> OnGet(string id)
    {
        var contact = await _contactBook.GetContactById(id);
        ContactViewModel = new ContactViewModel(contact);
        return Page();
    }
}