using Microsoft.Extensions.DependencyInjection;
using ShoppingListApi.Database;
using ShoppingListApi.Entities;
using ShoppingListApi.Repositories;
using ShoppingListApi.Responses;
using ShoppingListApi.Services;

namespace ShoppingListApi
{
    public class DependencyContainer
    {
        private readonly IServiceCollection _services;

        public DependencyContainer(IServiceCollection services)
        {
            _services = services;
        }
        
        public void RegisterDependencies()
        {
            _services.AddScoped<ShoppingListContext>();
            _services.AddScoped<IRepository<ShoppingList>, ShoppingListRepository>();
            _services.AddScoped<IShoppingListService<IApiResponse>, ShoppingListService>();
        }
    }
}
