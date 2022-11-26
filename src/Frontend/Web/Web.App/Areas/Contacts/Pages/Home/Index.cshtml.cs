using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Web.Authentication;
using Web.App.Areas.Contacts.ViewModels;
using Core.Contacts.Interfaces;

namespace Web.App.Areas.Contacts.Pages.Home;

[Authorize]
[TypeFilter(typeof(RefreshTokenFilter))]
public class IndexModel : PageModel
{
    private readonly IContactBookService _contactBook;

    public IndexModel(IContactBookService contactBook)
    {
        _contactBook = contactBook;
    }

    [BindProperty]
    public IEnumerable<ContactViewModel> Contacts { get; set; }

    public async Task OnGet()
    {
        Contacts = (await _contactBook.GetAllContacts()).Select(dto => new ContactViewModel(dto));
    }
}