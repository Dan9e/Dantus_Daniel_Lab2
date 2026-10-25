using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dantus_Daniel_Lab2.Models
{
    public class BookCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList = new();

        public void PopulateAssignedCategoryData(Dantus_Daniel_Lab2Context context, Book book)
        {
            var allCategories = context.Category;
            var bookCategories = new HashSet<int>(
                (book.BookCategories ?? new List<BookCategory>()).Select(c => c.CategoryID));
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = bookCategories.Contains(cat.ID)
                });
            }
        }

        public void UpdateBookCategories(Dantus_Daniel_Lab2Context context,
            string[]? selectedCategories, Book bookToUpdate)
        {
            bookToUpdate.BookCategories ??= new List<BookCategory>();

            if (selectedCategories == null || selectedCategories.Length == 0)
            {
                // nicio categorie bifata: le stergem pe cele existente
                context.RemoveRange(bookToUpdate.BookCategories.ToList());
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>(
                bookToUpdate.BookCategories.Select(c => c.CategoryID));

            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if (!bookCategories.Contains(cat.ID))
                    {
                        bookToUpdate.BookCategories.Add(new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (bookCategories.Contains(cat.ID))
                    {
                        var bookToRemove = bookToUpdate.BookCategories
                            .FirstOrDefault(i => i.CategoryID == cat.ID);
                        if (bookToRemove != null)
                        {
                            context.Remove(bookToRemove);
                        }
                    }
                }
            }
        }
    }
}
