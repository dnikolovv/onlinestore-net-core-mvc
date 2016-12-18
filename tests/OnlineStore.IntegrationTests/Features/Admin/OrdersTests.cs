namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Shouldly;
    using OnlineStore.Features.Admin;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrdersTests
    {
        public async Task ListsOnlyUnshippedOrders(SliceFixture fixture)
        {
            // Arrange
            // Add some orders
            var firstOrder = new Data.Models.Order
            {
                Name = "Some name",
                Line1 = "An address",
                City = "City",
                Country = "Country",
                Zip = "Zip",
                State = "State",
                Shipped = true
            };

            var secondOrder = new Data.Models.Order
            {
                Name = "Some name2",
                Line1 = "An address2",
                City = "City2",
                Country = "Country2",
                Zip = "Zip2",
                State = "State2",
                Shipped = false,
                OrderedItems = new List<Data.Models.CartItem>
                {
                    new Data.Models.CartItem()
                }
            };

            await fixture.InsertAsync(firstOrder, secondOrder);

            // Act
            var query = new Orders.Query();
            var ordersReceived = await fixture.SendAsync(query);

            // Assert
            ordersReceived.Count().ShouldBe(1);
            ordersReceived.First().Name.ShouldBe(secondOrder.Name);
            ordersReceived.First().Zip.ShouldBe(secondOrder.Zip);
            ordersReceived.First().OrderedItems.Count.ShouldBe(secondOrder.OrderedItems.Count);
        }
    }
}
