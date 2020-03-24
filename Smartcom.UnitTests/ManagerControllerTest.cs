using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Smartcom.WebApp.Controllers;
using Smartcom.WebApp.UnitOfWork;
using Smartcom.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Smartcom.WebApp.Services.Intefaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smartcom.WebApp.UnitOfWork.Interface;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Smartcom.UnitTests
{
    public class ManagerControllerTest
    {
        [Fact]
        public async Task GetItem_GetOneItem_OneItem()
        {
            //Arrange
            Guid testItemId = Guid.Parse("52937072-3188-4221-ae7b-f2a7633d1e83");
            var mockRepositoriesManager = GetRepositoriesManagerMock();
            mockRepositoriesManager.Setup(manager => manager.Items.Get(testItemId))
                .ReturnsAsync(GetTestItems().Find(item => item.ItemId == testItemId));

            ManagerController managerController = new ManagerController
                (mockRepositoriesManager.Object,
                GetUserManagerMock<Customer>().Object,
                GetEmailSenderMock().Object,
                GetPasswordGeneratorMock().Object);

            //Act
            var result = await managerController.GetItem(testItemId) as JsonResult;

            //Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var model = Assert.IsType<Item>(actionResult.Value);
            Assert.Equal(testItemId, model.ItemId);
            Assert.Equal("Item_4", model.Name);
            Assert.Equal("Category_4", model.Category);
            Assert.Equal("Code_4", model.Code);
            Assert.Equal(4, model.Price);
        }

        [Fact]
        public async Task GetItem_NotFound_NotFountObjectResult()
        {
            Guid testItemId = Guid.Parse("52937072-3188-4221-ae7b-f2a7633d1e80");
            var mockRepositoriesManager = GetRepositoriesManagerMock();
            mockRepositoriesManager.Setup(manager => manager.Items.Get(testItemId))
                .ReturnsAsync(GetTestItems().Find(item => item.ItemId == testItemId));

            ManagerController managerController = new ManagerController
                (mockRepositoriesManager.Object,
                GetUserManagerMock<Customer>().Object,
                GetEmailSenderMock().Object,
                GetPasswordGeneratorMock().Object);

            //Act
            var result = await managerController.GetItem(testItemId) as NotFoundObjectResult;

            //Assert
            var actionResult = Assert.IsType<NotFoundObjectResult>(result);
            var model = Assert.IsType<string>(actionResult.Value);
            Assert.Equal("Item not found", model);
        }

        [Fact]
        public async Task GetAllItems_GetListOfItems_ListOfItems()
        {
            //Arrange
            var mockRepositoriesManager = GetRepositoriesManagerMock();
            mockRepositoriesManager.Setup(manager => manager.Items.GetAll())
                .ReturnsAsync(GetTestItems());

            ManagerController managerController = new ManagerController
                (mockRepositoriesManager.Object,
                GetUserManagerMock<Customer>().Object,
                GetEmailSenderMock().Object,
                GetPasswordGeneratorMock().Object);

            //Act
            var result = await managerController.GetAllItems() as JsonResult;

            //Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var model = Assert.IsType<List<Item>>(actionResult.Value);
            Assert.Equal(GetTestItems().Count, model.Count);
        }


        private List<Item> GetTestItems()
        {
            return new List<Item>
            {
                new Item {ItemId = Guid.Parse("38e025c5-7d07-4001-96e9-27e8d88037b9"), Name = "Item_1", Category = "Category_1", Code = "Code_1", Price = 1 },
                new Item {ItemId = Guid.Parse("d670798c-5f9b-4448-9e0e-9da261ee4dea"), Name = "Item_2", Category = "Category_2", Code = "Code_2", Price = 2 },
                new Item {ItemId = Guid.Parse("82a30b5e-e099-425b-b2d2-6975bfe0eb83"), Name = "Item_3", Category = "Category_3", Code = "Code_3", Price = 3 },
                new Item {ItemId = Guid.Parse("52937072-3188-4221-ae7b-f2a7633d1e83"), Name = "Item_4", Category = "Category_4", Code = "Code_4", Price = 4 },
                new Item {ItemId = Guid.Parse("728480a6-357d-4a8b-a8d8-983cc60fd7bc"), Name = "Item_5", Category = "Category_5", Code = "Code_5", Price = 5 }
            };






        }

        private Mock<UserManager<TIDentityUser>> GetUserManagerMock<TIDentityUser>() where TIDentityUser : IdentityUser<Guid>
        {
            return new Mock<UserManager<TIDentityUser>>(
                    new Mock<IUserStore<TIDentityUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<TIDentityUser>>().Object,
                    new IUserValidator<TIDentityUser>[0],
                    new IPasswordValidator<TIDentityUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<TIDentityUser>>>().Object);
        }
        private Mock<IEmailSender> GetEmailSenderMock()
        {
            return new Mock<IEmailSender>();
        }
        private Mock<IPasswordGenerator> GetPasswordGeneratorMock()
        {
            return new Mock<IPasswordGenerator>();
        }
        private Mock<IRepositoriesManager> GetRepositoriesManagerMock()
        {
            return new Mock<IRepositoriesManager>();
        }
    }
}
