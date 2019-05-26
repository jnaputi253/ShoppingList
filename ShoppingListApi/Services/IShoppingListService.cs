using System.Threading.Tasks;
using ShoppingListApi.Entities;
using ShoppingListApi.Responses;

namespace ShoppingListApi.Services
{
    public interface IShoppingListService<TApiResponse> where TApiResponse : IApiResponse
    {
        Task<TApiResponse> GetAllAsync();
        Task<TApiResponse> FindAsync(int id);
        Task<TApiResponse> AddAsync(string shoppingListName);
        Task<TApiResponse> UpdateAsync(ShoppingList updatedShoppingList);
    }
}