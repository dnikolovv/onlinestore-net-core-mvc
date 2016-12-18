namespace OnlineStore.IntegrationTests.Features.Cart
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Account;
    using OnlineStore.Features.Cart;
    using OnlineStore.Features.Product;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class IndexTests
    {
        public async Task IndexIsCorrect(SliceFixture fixture)
        {
            // Arrange
            // Create a user whose cart we are going to request
            var registerUserCommand = new Register.Command
            {
                FirstName = "John",
                UserName = "user",
                Password = "password123"
            };

            await fixture.SendAsync(registerUserCommand);

            // Add him some items
            var category = new Category
            {
                Name = "Category"
            };

            await fixture.InsertAsync(category);

            var createCommands = new List<Create.Command>
            {
                new Create.Command
                {
                    Name = "Product1",
                    Description = "Description",
                    Price = 12.00m,
                    Category = category,
                },
                new Create.Command
                {
                    Name = "Product2",
                    Description = "Description",
                    Price = 12.00m,
                    Category = category,
                },
                new Create.Command
                {
                    Name = "Product3",
                    Description = "Description",
                    Price = 12.00m,
                    Category = category,
                },
            };

            foreach (var command in createCommands)
            {
                await fixture.SendAsync(command);
            }

            var productsInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .OrderBy(p => p.DateAdded)
                .ToListAsync());

            foreach (var product in productsInDb)
            {
                var addToCartCommand = new AddToCart.Command
                {
                    CurrentUserName = registerUserCommand.UserName,
                    ProductId = product.Id
                };

                await fixture.SendAsync(addToCartCommand);
            }

            // Act

            var indexQuery = new Index.Query
            {
                CurrentUserName = registerUserCommand.UserName,
                ReturnUrl = "/"
            };

            var result = await fixture.SendAsync(indexQuery);

            // Assert
            // Sort them in the same way the products in db are sorted so I can assert by index
            result.OrderedItems
                .OrderBy(i => i.Product.DateAdded);

            result.OrderedItems.Count.ShouldBe(productsInDb.Count);
            result.OrderedItems[0].Product.Id.ShouldBe(productsInDb[0].Id);
            result.OrderedItems[1].Product.Id.ShouldBe(productsInDb[1].Id);
            result.OrderedItems[2].Product.Id.ShouldBe(productsInDb[2].Id);
        }
    }
}
