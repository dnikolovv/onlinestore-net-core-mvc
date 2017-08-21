namespace OnlineStore.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class ITempDataDictionaryExtensions
    {
        public static string GetSuccessMessage(this ITempDataDictionary tempData)
        {
            if (tempData["successMessage"] is string && (!string.IsNullOrEmpty((string)tempData["successMessage"])))
            {
                return tempData["successMessage"] as string;
            }

            return null;
        }

        public static void SetSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData["successMessage"] = message;
        }
    }
}
