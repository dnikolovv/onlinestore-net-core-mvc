namespace OnlineStore.IntegrationTests.Features.Order
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Shouldly;
    using OnlineStore.Features.Order;
    using System.Threading.Tasks;

    public class MarkShippedTests
    {
        public async Task CanMarkAsShipped(SliceFixture fixture)
        {
            // Arrange
            var order = new Order
            {
                Name = "Some name",
                Line1 = "An address",
                City = "City",
                Country = "Country",
                Zip = "Zip",
                State = "State",
                Shipped = false
            };

            await fixture.InsertAsync(order);

            // Act
            var command = new MarkShipped.Command() { OrderId = order.Id };

            await fixture.SendAsync(command);

            // Assert
            var orderInDb = await fixture
                .ExecuteDbContextAsync(db => db
                .Orders
                .FirstOrDefaultAsync(o => o.Name == order.Name));

            orderInDb.Shipped.ShouldBeTrue();
        }
    }
}
