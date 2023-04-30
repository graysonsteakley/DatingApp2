using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    // DbSets represent Users, so in this case the table is called "Users"
    public DbSet<AppUser> Users { get; set; }
  }
}
