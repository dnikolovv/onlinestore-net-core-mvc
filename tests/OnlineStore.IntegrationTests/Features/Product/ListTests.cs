namespace OnlineStore.IntegrationTests.Features.Product
{
    using AutoMapper.QueryableExtensions;
    using Data.Models;
    using Infrastructure.ViewModels.Products;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Product;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ListTests
    {
        public async Task PaginationIsCorrect(SliceFixture fixture)
        {
            // Arrange
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

            // Act
            var query = new List.Query() { PageSize = 2, Page = 1 };

            var result = await fixture.SendAsync(query);

            // Assert
            var productsInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .ToListAsync());

            result.PagingInfo.CurrentPage.ShouldBe(query.Page);
            result.PagingInfo.ItemsPerPage.ShouldBe((int)query.PageSize);
            result.PagingInfo.TotalItems.ShouldBe(productsInDb.Count);
            result.Products.Count().ShouldBe((int)query.PageSize);
            result.PagingInfo.TotalPages.ShouldBe((int)Math.Ceiling((decimal)productsInDb.Count / (int)query.PageSize));
        }

        public async Task CanListByCategory(SliceFixture fixture)
        {
            // Arrange
            var category = new Category
            {
                Name = "Category"
            };

            var secondCategory = new Category
            {
                Name = "Category2"
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
                new Create.Command
                {
                    Name = "Product1",
                    Description = "Description",
                    Price = 12.00m,
                    Category = secondCategory,
                },
                new Create.Command
                {
                    Name = "Product2",
                    Description = "Description",
                    Price = 12.00m,
                    Category = secondCategory,
                },
                new Create.Command
                {
                    Name = "Product3",
                    Description = "Description",
                    Price = 12.00m,
                    Category = secondCategory,
                },
            };

            foreach (var command in createCommands)
            {
                await fixture.SendAsync(command);
            }

            // Act
            var query = new List.Query() { PageSize = createCommands.Count, Page = 1, Category = secondCategory.Name };

            var result = await fixture.SendAsync(query);

            // Assert
            var productsInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .Where(p => p.Category.Name == secondCategory.Name)
                .OrderBy(p => p.Name)
                .ProjectTo<ProductViewModel>()
                .ToListAsync());

            var productsList = result.Products.OrderBy(p => p.Name).ToList();

            productsList.Count.ShouldBe(productsInDb.Count);
            productsList[0].Name.ShouldBe(productsInDb[0].Name);
            productsList[1].Name.ShouldBe(productsInDb[1].Name);
            productsList[2].Name.ShouldBe(productsInDb[2].Name);
        }


        public async Task CanListAllProducts(SliceFixture fixture)
        {
            // Arrange
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

            // Act
            var query = new List.Query() { PageSize = createCommands.Count, Page = 1 };

            var result = await fixture.SendAsync(query);
            
            // Assert
            var productsInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Products
                .OrderBy(p => p.Name)
                .ProjectTo<ProductViewModel>()
                .ToListAsync());

            var productsList = result.Products.OrderBy(p => p.Name).ToList();

            productsList.Count.ShouldBe(productsInDb.Count);
            productsList[0].Name.ShouldBe(productsInDb[0].Name);
            productsList[1].Name.ShouldBe(productsInDb[1].Name);
            productsList[2].Name.ShouldBe(productsInDb[2].Name);
        }
    }
}
