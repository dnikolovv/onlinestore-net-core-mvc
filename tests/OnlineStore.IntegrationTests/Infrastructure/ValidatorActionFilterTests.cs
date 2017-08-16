namespace OnlineStore.IntegrationTests.Infrastructure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using OnlineStore.Infrastructure;
    using Shouldly;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class ValidatorActionFilterTests
    {
        public void ReturnsBadRequestOnInvalidModelStateWithGetRequest(SliceFixture fixture)
        {
            // Arrange
            var filter = new ValidatorActionFilter();
            var filterContext = fixture.GetActionExecutingContext("GET");

            filterContext.ModelState.AddModelError("Error", "An error has occured!");

            // Act
            filter.OnActionExecuting(filterContext);

            // Assert
            filterContext.Result.ShouldBeOfType<BadRequestResult>();
        }

        public void ReturnsBadRequestOnInvalidModelStateWithOtherThanGetRequest(SliceFixture fixture)
        {
            // Arrange
            var filter = new ValidatorActionFilter();
            var filterContext = fixture.GetActionExecutingContext("POST");

            filterContext.ModelState.AddModelError("Error", "An error has occured!");

            // Act
            filter.OnActionExecuting(filterContext);

            // Assert
            filterContext.HttpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        }

        public void ResultContainsErrorMessagesInModelState(SliceFixture fixture)
        {
            // Arrange
            var filter = new ValidatorActionFilter();
            var filterContext = fixture.GetActionExecutingContext("POST");
            Dictionary<string, CustomModelStateEntry> errorsToExpect = ArrangeExpectedErrors(filterContext);

            // Act
            filter.OnActionExecuting(filterContext);

            // Assert
            var contentResult = filterContext.Result as ContentResult;

            contentResult.ShouldNotBeNull();
            contentResult.ContentType.ShouldBe("application/json");
            AssertThatReturnedErrorsMatchExpected(errorsToExpect, contentResult);
        }

        private static void AssertThatReturnedErrorsMatchExpected(Dictionary<string, CustomModelStateEntry> errorsToExpect, ContentResult contentResult)
        {
            var returnedErrors = JsonConvert.DeserializeObject<Dictionary<string, CustomModelStateEntry>>(contentResult.Content,
                            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            foreach (var returnedError in returnedErrors)
            {
                errorsToExpect.ShouldContainKey(returnedError.Key);

                var expectedEntryErrorMessages = errorsToExpect
                    .First(e => e.Key == returnedError.Key)
                    .Value
                    .Errors
                    .Select(e => e.ErrorMessage);

                foreach (var errorMessage in returnedError.Value.Errors)
                {
                    expectedEntryErrorMessages.ShouldContain(errorMessage.ToString());
                }
            }
        }

        private static Dictionary<string, CustomModelStateEntry> ArrangeExpectedErrors(ActionExecutingContext filterContext)
        {
            var errorsToExpect = new Dictionary<string, CustomModelStateEntry>
            {
                { "Error1", new CustomModelStateEntry { Errors = new List<CustomModelError> { "First validation message", "Second validation message" } } },
                { "Error2", new CustomModelStateEntry { Errors = new List<CustomModelError> { "Third validation message", "Fourth validation message" } } }
            };

            foreach (var error in errorsToExpect)
            {
                foreach (var errorMessage in error.Value.Errors)
                {
                    filterContext.ModelState.AddModelError(error.Key, errorMessage);
                }
            }

            return errorsToExpect;
        }

        private class CustomModelStateEntry
        {
            public ICollection<CustomModelError> Errors { get; set; }
        }

        private class CustomModelError
        {
            public string ErrorMessage { get; set; }

            public static implicit operator CustomModelError(string message)
            {
                return new CustomModelError { ErrorMessage = message };
            }

            public static implicit operator string(CustomModelError error)
            {
                return error.ErrorMessage;
            }

            public override string ToString()
            {
                return this.ErrorMessage;
            }
        }
    }
}
