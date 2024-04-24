using Equipment__Management.Models;
using Microsoft.EntityFrameworkCore;

//using Microsoft.EntityFrameworkCore;
using System.Data;


namespace Equipment__Management
{
    public class EMDBContext : DbContext
    {
        public EMDBContext() 
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = testDB.db");
            //optionsBuilder.UseSqlite($"{ AppDomain.CurrentDomain.BaseDirectory  }testDB.db");
            //base.OnConfiguring(optionsBuilder);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Site>()
                .HasOne(a => a.User)
                .WithMany(m => m.Sites)
                .HasForeignKey(f => f.User_Id);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Registered_Equipment> Registered_Equipment { get; set; }
    }
}
