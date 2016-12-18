namespace OnlineStore.IntegrationTests.Features.Admin
{
    using Shouldly;
    using OnlineStore.Features.Admin;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryTests
    {
        public async Task ListsAllCategories(SliceFixture fixture)
        {
            // Arrange
            var categories = new List<Data.Models.Category>
            {
                new Data.Models.Category
                {
                    Name = "Category1"
                },
                new Data.Models.Category
                {
                    Name = "Category2"
                },
                new Data.Models.Category
                {
                    Name = "Category3"
                },
            };

            foreach (var category in categories)
            {
                await fixture.InsertAsync(category);
            }

            // Act
            var query = new Categories.Query();

            var result = await fixture.SendAsync(query);

            // Assert
            result.Count().ShouldBe(categories.Count);

            for (int i = 0; i < categories.Count; i++)
            {
                result.ElementAt(i).Name.ShouldBe(categories[i].Name);
            }
        }
    }
}
