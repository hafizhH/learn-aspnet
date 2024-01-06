using LearnAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Data
{
    public class ApiContext : DbContext
    {
        public DbSet<Book> BookData { get; set; }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }
    }
}
