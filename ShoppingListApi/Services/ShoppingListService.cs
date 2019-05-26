using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ShoppingListApi.Entities;
using ShoppingListApi.Repositories;
using ShoppingListApi.Responses;

namespace ShoppingListApi.Services
{
    public class ShoppingListService : IShoppingListService<IApiResponse>
    {
        private readonly IRepository<ShoppingList> _repository;

        public ShoppingListService(IRepository<ShoppingList> repository)
        {
            _repository = repository;
        }

        public async Task<IApiResponse> GetAllAsync()
        {
            IEnumerable<ShoppingList> shoppingLists = await _repository.GetAllAsync();
            IApiResponse apiResponse = new ApiResponse(HttpStatusCode.OK)
            {
                Result = shoppingLists
            };

            return apiResponse;
        }

        public async Task<IApiResponse> FindAsync(int id)
        {
            var shoppingList = await _repository.FindAsync(id);

            IApiResponse apiResponse = new ApiResponse(GetStatusCodeForSingleItem(shoppingList))
            {
                Result = shoppingList
            };

            return apiResponse;
        }

        public async Task<IApiResponse> AddAsync(string shoppingListName)
        {
            var currentDate = DateTime.Now;

            var shoppingList = new ShoppingList
            {
                Name = shoppingListName,
                CreatedAt = currentDate,
                LastUpdated = currentDate
            };

            await _repository.AddAsync(shoppingList);

            return new ApiResponse(HttpStatusCode.Created)
            {
                Result = shoppingList
            };
        }

        public async Task<IApiResponse> UpdateAsync(ShoppingList updatedShoppingList)
        {
            await _repository.UpdateAsync(updatedShoppingList);

            return new ApiResponse(HttpStatusCode.OK);
        }

        private HttpStatusCode GetStatusCodeForSingleItem(ShoppingList shoppingList)
        {
            if (shoppingList == null)
            {
                return HttpStatusCode.NotFound;
            }

            return HttpStatusCode.OK;
        }
    }
}