namespace OnlineStore.IntegrationTests.Infrastructure.Extensions
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Http;
    using OnlineStore.Infrastructure.Extensions;
    using Shouldly;

    public class HttpRequestExtensionsTests
    {
        public void PathAndQueryReturnsCorrectResultWithQueryString(SliceFixture fixture)
        {
            // Arrange
            var path = "/Some/Path/";
            var query = "?value=1234";

            var httpRequest = A.Fake<HttpRequest>();
            httpRequest.Path = new PathString(path);
            httpRequest.QueryString = new QueryString(query);

            // Act
            var pathAndQuery = httpRequest.PathAndQuery();

            // Assert
            pathAndQuery.ShouldBe($"{path}{query}");
        }

        public void PathAndQueryReturnsCorrectResultWithoutQueryString(SliceFixture fixture)
        {
            // Arrange
            var path = "/Some/Path/";
            
            var httpRequest = A.Fake<HttpRequest>();
            httpRequest.Path = new PathString(path);
            httpRequest.QueryString = new QueryString();

            // Act
            var pathAndQuery = httpRequest.PathAndQuery();

            // Assert
            pathAndQuery.ShouldBe(path.ToString());
        }
    }
}
