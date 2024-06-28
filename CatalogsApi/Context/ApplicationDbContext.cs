using CatalogsApi.Entites;
using CatalogsApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogsApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AuthorBook> AuthorsBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AuthorBook>()
                .ToTable("AuthorsBooks")
                .HasKey(k => new { k.AuthorId, k.BookId });

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Author)
                .WithMany(a => a.AuthorsBooks)
                .HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Book)
                .WithMany(b => b.AuthorsBooks)
                .HasForeignKey(b => b.BookId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
