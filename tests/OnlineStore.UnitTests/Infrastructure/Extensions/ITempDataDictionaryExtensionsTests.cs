namespace OnlineStore.UnitTests.Infrastructure.Extensions
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using OnlineStore.Infrastructure.Extensions;
    using Shouldly;
    using Xunit;

    public class ITempDataDictionaryExtensionsTests
    {
        [Fact]
        public void CanGetSuccessMessage()
        {
            // Arrange
            var expectedMessage = "Success message!";
            var tempData = A.Fake<ITempDataDictionary>();
            tempData["successMessage"] = expectedMessage;

            // Act
            var result = tempData.GetSuccessMessage();

            // Assert
            result.ShouldBe(expectedMessage);
        }

        [Fact]
        public void CanSetSuccessMessage()
        {
            // Arrange
            var expectedMessage = "Success message!";
            var tempData = A.Fake<ITempDataDictionary>();

            // Act
            tempData.SetSuccessMessage(expectedMessage);

            // Assert
            tempData.GetSuccessMessage().ShouldBe(expectedMessage);
        }
    }
}
