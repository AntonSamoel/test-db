using Microsoft.EntityFrameworkCore;
using PokemonReview.Models;

namespace PokemonReview.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new {pc.CategoryId,pc.PokemonId});
            modelBuilder.Entity<PokemonCategory>().HasOne<Pokemon>(pc => pc.Pokemon)
                .WithMany(p => p.PokemonCategories)
                .HasForeignKey(pc => pc.PokemonId);
            modelBuilder.Entity<PokemonCategory>().HasOne<Category>(pc => pc.Category)
                .WithMany(p => p.PokemonCategories)
                .HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<PokemonOwner>().HasKey(pc => new { pc.OwnerId, pc.PokemonId });
            modelBuilder.Entity<PokemonOwner>().HasOne<Owner>(pc => pc.Owner)
                .WithMany(p => p.PokemonOwners)
                .HasForeignKey(pc => pc.OwnerId);
            modelBuilder.Entity<PokemonOwner>().HasOne<Pokemon>(pc => pc.Pokemon)
                .WithMany(p => p.PokemonOwners)
                .HasForeignKey(pc => pc.PokemonId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
