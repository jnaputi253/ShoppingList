using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingListApi.Database;
using ShoppingListApi.Entities;

namespace ShoppingListApi.Repositories
{
    public class ShoppingListRepository : IRepository<ShoppingList>
    {
        private readonly ShoppingListContext _context;

        public ShoppingListRepository(ShoppingListContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShoppingList>> GetAllAsync()
        {
            return await _context.ShoppingLists.ToListAsync();
        }

        public async Task<ShoppingList> FindAsync(int id)
        {
            return await _context.ShoppingLists.FindAsync(id);
        }

        public async Task<int> AddAsync(ShoppingList entity)
        {
            await _context.ShoppingLists.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ShoppingListId;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ShoppingLists.AnyAsync(
                shoppingList => shoppingList.ShoppingListId == id);
        }

        public async Task UpdateAsync(ShoppingList entity)
        {
            _context.ShoppingLists.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ShoppingList entity)
        {
            _context.ShoppingLists.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
