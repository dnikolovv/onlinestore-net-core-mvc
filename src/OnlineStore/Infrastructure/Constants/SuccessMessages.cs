namespace OnlineStore.Infrastructure.Constants
{
    public static class SuccessMessages
    {
        // Categories
        public static string SuccessfullyCreatedCategory(string categoryName)
        {
            return $"Successfuly created the category {categoryName}!";
        }

        public static string SuccessfullyEditedCategory(string categoryName)
        {
            return $"Successfully edited {categoryName}!";
        }

        public static string SuccessfullyRemovedCategory(string categoryName)
        {
            return $"Successfully removed {categoryName}!";
        }

        // Products
        public static string SuccessfullyEditedProduct(string productName)
        {
            return $"{productName} has been saved!";
        }

        public static string SuccessfullyCreatedProduct(string productName)
        {
            return $"Successfuly added the product {productName}!";
        }

        public static string SuccessfullyRemovedProduct(int productId)
        {
            return $"Product with Id {productId} has been removed!";
        }

        // Users
        public static string SuccessfullyEditedUser(string userName)
        {
            return $"Successfuly edited {userName} permissions!";
        }
    }
}
