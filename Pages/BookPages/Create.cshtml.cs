using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dantus_Daniel_Lab2.Models;

namespace Dantus_Daniel_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Dantus_Daniel_Lab2Context _context;

    public CreateModel(Dantus_Daniel_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    private void LoadLists()
    {
        ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "FullName");
    }

    public IActionResult OnGet()
    {
        LoadLists();

        var book = new Book();
        book.BookCategories = new List<BookCategory>();
        PopulateAssignedCategoryData(_context, book);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        Book.BookCategories = new List<BookCategory>();
        if (selectedCategories != null)
        {
            foreach (var cat in selectedCategories)
            {
                Book.BookCategories.Add(new BookCategory { CategoryID = int.Parse(cat) });
            }
        }

        if (!ModelState.IsValid)
        {
            LoadLists();
            PopulateAssignedCategoryData(_context, Book);
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
