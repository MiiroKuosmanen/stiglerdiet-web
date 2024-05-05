using Microsoft.EntityFrameworkCore;
using stiglerdiet.Controllers;

namespace stiglerdiet;

public class FoodsContext : DbContext
{
    public FoodsContext(DbContextOptions<FoodsContext> options) : base(options)
    {
        
    }

    public DbSet<Food> Foods { get; set; } = null!;
}