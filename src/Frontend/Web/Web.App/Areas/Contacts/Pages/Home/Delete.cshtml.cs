using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Authentication;
using Core.Contacts.Requests;
using Core.Contacts.Interfaces;

namespace Web.App.Areas.Contacts.Pages.Home;

[Authorize]
[TypeFilter(typeof(RefreshTokenFilter))]
public class DeleteModel : PageModel
{
    private readonly IContactBookService _contactBook;

    public DeleteModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        await _contactBook.DeleteContact(new DeleteContactRequest
        {
            Id = id
        });
        return RedirectToPage("Home");
    }
}