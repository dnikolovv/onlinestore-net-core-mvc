namespace OnlineStore.IntegrationTests.Infrastructure.Attributes
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Newtonsoft.Json;
    using OnlineStore.Infrastructure.Attributes;
    using Shouldly;
    using System.Security.Claims;

    public class JsonAuthorizeAttributeTests
    {
        public void RedirectsIfUnauthorized(SliceFixture fixture)
        {
            // Arrange
            var attribute = new JsonAuthorizeAttribute();

            var filterContext = fixture.GetActionExecutingContext("GET");

            var urlHelper = A.Fake<IUrlHelper>();
            
            A.CallTo(() => urlHelper
                .Action(new UrlActionContext()))
                .WithAnyArguments()
                .Returns("Login/Account/");

            var controller = A.Fake<Controller>();
            controller.Url = urlHelper;

            A.CallTo(() => filterContext.Controller).Returns(controller);

            var user = A.Fake<ClaimsPrincipal>();
            A.CallTo(() => user.Identity.IsAuthenticated).Returns(false);

            // Act
            attribute.OnActionExecuting(filterContext);

            // Assert
            filterContext.Result.ShouldBeOfType<ContentResult>();

            var result = filterContext.Result as ContentResult;
            result.ContentType.ShouldBe("application/json");

            HelperRedirectResult deserializedContent = JsonConvert.DeserializeObject<HelperRedirectResult>(result.Content);
            deserializedContent.Redirect.ShouldBe("Login/Account/");
        }
        
        private class HelperRedirectResult
        {
            public string Redirect { get; set; }
        }
    }
}
