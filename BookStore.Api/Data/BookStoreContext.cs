using BookStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Data;

public class BookStoreContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookCategory> BookCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Author> Authors { get; set; }

    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>()
            .HasOne(book => book.Author)
            .WithMany(Book => Book.Books)
            .HasForeignKey(book => book.AuthorId);

        modelBuilder.Entity<BookCategory>()
            .HasKey(bookCategory => new { bookCategory.BookId, bookCategory.CategoryId });

        modelBuilder.Entity<BookCategory>()
           .HasOne(bookCategory => bookCategory.Book)
           .WithMany(book => book.BookCategories)
           .HasForeignKey(bookCategory => bookCategory.BookId);

        modelBuilder.Entity<BookCategory>()
            .HasOne(bookCategory => bookCategory.Category)
            .WithMany(category => category.BookCategories)
            .HasForeignKey(bookCategory => bookCategory.CategoryId);
    }
}