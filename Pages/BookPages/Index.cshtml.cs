using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dantus_Daniel_Lab2.Models;

namespace Dantus_Daniel_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Dantus_Daniel_Lab2Context _context;

    public IndexModel(Dantus_Daniel_Lab2Context context)
    {
        _context = context;
    }

    public BookData BookD { get; set; } = default!;
    public int BookID { get; set; }
    public int CategoryID { get; set; }
    public SelectList Authors { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public int? SelectedAuthorID { get; set; }

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        BookD = new BookData();
        Authors = new SelectList(await _context.Author.ToListAsync(), "ID", "FullName");

        var books = _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .AsQueryable();

        if (SelectedAuthorID.HasValue)
        {
            books = books.Where(b => b.AuthorID == SelectedAuthorID);
        }

        BookD.Books = await books.OrderBy(b => b.Title).ToListAsync();

        if (id != null)
        {
            BookID = id.Value;
            var book = BookD.Books.SingleOrDefault(i => i.ID == id.Value);
            if (book != null)
            {
                BookD.Categories = book.BookCategories!.Select(s => s.Category);
            }
        }
    }
}
