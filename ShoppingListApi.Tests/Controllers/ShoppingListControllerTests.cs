using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListApi.Controllers;
using ShoppingListApi.Entities;
using ShoppingListApi.Repositories;
using ShoppingListApi.Responses;
using ShoppingListApi.Services;
using Xunit;

namespace ShoppingListApi.Tests.Controllers
{
    public class ShoppingListControllerTests
    {
        private ShoppingListController _controller;
        private IShoppingListService<IApiResponse> _service;
        private IRepository<ShoppingList> _repository;

        [Fact]
        public async void ShouldReturnAllShoppingLists()
        {
            IEnumerable<ShoppingList> shoppingLists = new List<ShoppingList>
            {
                Mock.Of<ShoppingList>(),
                Mock.Of<ShoppingList>(),
                Mock.Of<ShoppingList>()
            };

            var mockRepository = new Mock<IRepository<ShoppingList>>();
            mockRepository.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(shoppingLists);

            _repository = mockRepository.Object;
            _service = new ShoppingListService(_repository);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.GetAllShoppingListsAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Result.Should().BeAssignableTo<IEnumerable<ShoppingList>>()
                .Which.Count().Should().Be(3);
        }

        [Fact]
        public async void ShouldReturnAnEmptyCollectionWhenNoDataIsStored()
        {
            IEnumerable<ShoppingList> shoppingLists = new List<ShoppingList>();

            var mockRepository = new Mock<IRepository<ShoppingList>>();
            mockRepository.Setup(mock => mock.GetAllAsync())
                .ReturnsAsync(shoppingLists);

            _repository = mockRepository.Object;
            _service = new ShoppingListService(_repository);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.GetAllShoppingListsAsync();


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Result.Should().BeAssignableTo<IEnumerable<ShoppingList>>()
                .Which.Count().Should().Be(0);
        }

        [Fact]
        public async void ShouldReturnABadRequestResultWhenTheIdIsNull()
        {
            _repository = Mock.Of<IRepository<ShoppingList>>();
            _service = new ShoppingListService(_repository);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.FindShoppingListAsync(null);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Result == null &&
                    response.Message.Equals("The shopping list you are looking for is not valid."));
        }

        [Fact]
        public async void ShouldReturnANotFoundResultWhenTheShoppingListDoesNotExist()
        {
            var mockRepository = new Mock<IRepository<ShoppingList>>();
            mockRepository.Setup(mock => mock.FindAsync(3))
                .ReturnsAsync(value: null);

            _repository = mockRepository.Object;
            _service = new ShoppingListService(_repository);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.FindShoppingListAsync(3);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.NotFound &&
                    response.Message == null &&
                    response.Result == null);
        }

        [Fact]
        public async void ShouldReturnAShoppingListWhenItExists()
        {
            var shoppingList = new ShoppingList
            {
                ShoppingListId = 1,
                Name = "Test",
                CreatedAt = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow
            };

            var mockRepository = new Mock<IRepository<ShoppingList>>();
            mockRepository.Setup(mock => mock.FindAsync(3))
                .ReturnsAsync(shoppingList);

            _repository = mockRepository.Object;
            _service = new ShoppingListService(_repository);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.FindShoppingListAsync(3);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.OK &&
                    response.Message == null &&
                    response.Result != null);
        }

        [Theory]
        [InlineData("")]
        [InlineData("                      ")]
        [InlineData("\t\t\t\t    \t\t     \t")]
        public async void ShouldReturnABadRequestResultWhenAnEmptyStringIsPassed(string data)
        {
            _service = Mock.Of<IShoppingListService<IApiResponse>>();
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.AddNewShoppingList(data);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Message.Equals("The shopping list must have a name.") &&
                    response.Result == null);
        }

        [Fact]
        public async void ShouldReturnTheANewlyCreatedShoppingList()
        {
            var shoppingList = new ShoppingList
            {
                ShoppingListId = 1,
                Name = "Test",
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            var apiResponse = new ApiResponse(HttpStatusCode.Created)
            {
                Result = shoppingList
            };

            var mockService = new Mock<IShoppingListService<IApiResponse>>();
            mockService.Setup(mock => mock.AddAsync(shoppingList.Name))
                .ReturnsAsync(apiResponse);

            _service = mockService.Object;
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.AddNewShoppingList(shoppingList.Name);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.Created &&
                    response.Message == null &&
                    response.Result != null);
        }

        [Fact]
        public async void ShouldReturnABadRequestWhenTheUpdatedShoppingListIsNull()
        {
            _service = Mock.Of<IShoppingListService<IApiResponse>>();
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.UpdateShoppingList(null);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Result == null &&
                    response.Message.Equals("Cannot update an invalid shopping list."));
        }

        [Fact]
        public async void ShouldCallTheRepositoryOnceWhenUpdatingAShoppingList()
        {
            var updatedShoppingList = new ShoppingList
            {
                ShoppingListId = 1,
                Name = "Test",
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            var mockRepository = new Mock<IRepository<ShoppingList>>();
            _service = new ShoppingListService(mockRepository.Object);
            _controller = new ShoppingListController(_service);


            await _controller.UpdateShoppingList(updatedShoppingList);


            mockRepository.Verify(mock => mock.UpdateAsync(updatedShoppingList), Times.Once);
        }

        [Fact]
        public async void ShouldReturnAnOkResultWhenTheUpdateIsSuccessful()
        {
            var updatedShoppingList = new ShoppingList
            {
                ShoppingListId = 1,
                Name = "Test",
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            var mockRepository = new Mock<IRepository<ShoppingList>>();
            _service = new ShoppingListService(mockRepository.Object);
            _controller = new ShoppingListController(_service);


            IActionResult result = await _controller.UpdateShoppingList(updatedShoppingList);


            result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().BeAssignableTo<IApiResponse>()
                .Which.Should().Match<IApiResponse>(response =>
                    response.StatusCode == HttpStatusCode.OK &&
                    response.Message == null &&
                    response.Result == null);
        }
    }
}
