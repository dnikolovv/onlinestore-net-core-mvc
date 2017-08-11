namespace OnlineStore.IntegrationTests.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Shouldly;

    public static class ITempDataDictionaryExtensions
    {
        public static void ShouldContainSuccessMessage(this ITempDataDictionary tempData, string successMessage)
        {
            tempData.ShouldContainKey("successMessage");
            tempData["successMessage"].ShouldBe(successMessage);
        }
    }
}
