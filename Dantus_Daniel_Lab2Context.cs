using Microsoft.EntityFrameworkCore;

public class Dantus_Daniel_Lab2Context(DbContextOptions<Dantus_Daniel_Lab2Context> options) : DbContext(options)
{
    public DbSet<Dantus_Daniel_Lab2.Models.Book> Book { get; set; } = default!;
}
