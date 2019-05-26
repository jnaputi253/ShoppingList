using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Entities;

namespace ShoppingListApi.Database
{
    public class ShoppingListContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }

        public ShoppingListContext(DbContextOptions options) : base(options) { }
    }
}
