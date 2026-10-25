using Microsoft.AspNetCore.Mvc.RazorPages;
using Dantus_Daniel_Lab2.Migrations;

namespace Dantus_Daniel_Lab2.Models
{
    public class BookCategoriesPageModel: PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList;
        public void PopulateAssignedCategoryData(Dantus_Daniel_Lab2Context context, Book book)
        {
            var allCategories = context.Category;
            var bookCategories = new HashSet<int>(book.BookCategories.Select(c => c.CategoryID));
            AssignedCategoryDataList = new List<AssignedCategoryData>();
            foreach (var category in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = category.ID,
                    Name = category.Name,
                    Assigned = bookCategories.Contains(category.ID)
                });
            }
        }

        public void UpdateBookCategories(Dantus_Daniel_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>(bookToUpdate.BookCategories.Select(c => c.CategoryID));
            foreach (var category in context.Category)
            {
                if (selectedCategoriesHS.Contains(category.ID.ToString()))
                {
                    if (!bookCategories.Contains(category.ID))
                    {
                        bookToUpdate.BookCategories.Add(new BookCategory { BookID = bookToUpdate.ID, CategoryID = category.ID });
                    }
                }
                else
                {
                    if (bookCategories.Contains(category.ID))
                    {
                        BookCategory bookToRemove = bookToUpdate.BookCategories.FirstOrDefault(c => c.CategoryID == category.ID);
                        context.Remove(bookToRemove);
                    }
                }
            }
        }
    }
}
