namespace Dantus_Daniel_Lab2.Models
{
    public class BookCategory
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public Book Book { get; set; }
        public int categoryID { get; set; }
        public Category Category { get; set; }
    }
}
