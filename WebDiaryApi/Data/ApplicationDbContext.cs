using Microsoft.EntityFrameworkCore;
using WebDiaryApi.Model;

namespace WebDiaryApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<DiaryEntry> DiaryEntries { get; set; }
    }
}
