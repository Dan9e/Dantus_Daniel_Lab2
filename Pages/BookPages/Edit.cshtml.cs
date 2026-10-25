using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dantus_Daniel_Lab2.Models;

namespace Dantus_Daniel_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Dantus_Daniel_Lab2Context _context;

    public EditModel(Dantus_Daniel_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    private void LoadLists()
    {
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "FullName");
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Book=await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(m => m.ID == id);

        var book = await _context.Book.FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        PopulateAssignedCategoryData(_context, Book);
        Book = book;
        LoadLists();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
    {
        if (id is null)
        {
            return NotFound();
        }

        var bookToUpdate = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.Author)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (bookToUpdate is null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            b => b.Title, b => b.Price, b => b.PublishingDate, b => b.PublisherID, b => b.AuthorID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData(_context, bookToUpdate);
        return Page();
        if (!ModelState.IsValid)
        {
            LoadLists();
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Book.Any(e => e.ID == Book.ID))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("./Index");
    }
}
