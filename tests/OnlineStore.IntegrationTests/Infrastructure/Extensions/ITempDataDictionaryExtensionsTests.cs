namespace OnlineStore.IntegrationTests.Infrastructure.Extensions
{
    using FakeItEasy;
    using OnlineStore.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Shouldly;

    public class ITempDataDictionaryExtensionsTests
    {
        public void CanGetSuccessMessage(SliceFixture fixture)
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

        public void CanSetSuccessMessage(SliceFixture fixture)
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
