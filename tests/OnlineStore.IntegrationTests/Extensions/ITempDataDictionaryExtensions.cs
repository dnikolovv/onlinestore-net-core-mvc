namespace OnlineStore.IntegrationTests.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using OnlineStore.Infrastructure.Extensions;
    using Shouldly;

    public static class ITempDataDictionaryExtensions
    {
        public static void ShouldContainSuccessMessage(this ITempDataDictionary tempData, string successMessage)
        {
            var containedSuccessMessage = tempData.GetSuccessMessage();
            containedSuccessMessage.ShouldNotBeNull();
            containedSuccessMessage.ShouldBe(containedSuccessMessage);
        }
    }
}
