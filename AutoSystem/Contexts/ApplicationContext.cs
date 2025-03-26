using AutoSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoSystem.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserDataModel> Users { get; set; }
        public DbSet<StepDataModel> Steps { get; set; }
        public DbSet<ModeDataModel> Modes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Auto.db");
        }
    }
}
