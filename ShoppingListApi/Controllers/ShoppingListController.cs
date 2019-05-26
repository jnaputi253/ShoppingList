using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingListApi.Entities;
using ShoppingListApi.Responses;
using ShoppingListApi.Services;

namespace ShoppingListApi.Controllers
{
    [Route("/api/v1/[controller]")]
    public class ShoppingListController
    {
        private readonly IShoppingListService<IApiResponse> _service;

        public ShoppingListController(IShoppingListService<IApiResponse> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShoppingListsAsync()
        {
            IApiResponse response = await _service.GetAllAsync();

            return new ObjectResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindShoppingListAsync(int? id)
        {
            IApiResponse response;

            if (!id.HasValue)
            {
                response = new ApiResponse(HttpStatusCode.BadRequest)
                {
                    Message = "The shopping list you are looking for is not valid."
                };

                return new ObjectResult(response);
            }

            response = await _service.FindAsync(id.Value);

            return new ObjectResult(response);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddNewShoppingList([FromForm] string shoppingListName)
        {
            shoppingListName = shoppingListName.Trim();

            IApiResponse response;

            if (string.IsNullOrEmpty(shoppingListName))
            {
                response = new ApiResponse(HttpStatusCode.BadRequest)
                {
                    Message = "The shopping list must have a name."
                };

                return new ObjectResult(response);
            }

            response = await _service.AddAsync(shoppingListName);

            return new ObjectResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateShoppingList(ShoppingList updatedShoppingList)
        {
            IApiResponse response;

            if (updatedShoppingList == null)
            {
                response = new ApiResponse(HttpStatusCode.BadRequest)
                {
                    Message = "Cannot update an invalid shopping list."
                };

                return new ObjectResult(response);
            }

            response = await _service.UpdateAsync(updatedShoppingList);

            return new ObjectResult(response);
        }
    }
}
