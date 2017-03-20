using System;

namespace OnlineStore.Infrastructure.Constants
{
    public static class SuccessMessages
    {
        // Categories
        public static string SuccessfullyCreatedCategory(string categoryName)
        {
            return $"Successfully created the category {categoryName}!";
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
            return $"Successfully added the product {productName}!";
        }

        public static string SuccessfullyRemovedProduct(int productId)
        {
            return $"Product with Id {productId} has been removed!";
        }

        // Users
        public static string SuccessfullyEditedUser(string userName)
        {
            return $"Successfully edited {userName} permissions!";
        }

        // Permission
        public static string SuccessfullyCreatedPermission()
        {
            return "Successfully created permission!";
        }

        public static string SuccessfullyEditedPermission()
        {
            return "Successfully edited permission!";
        }

        public static string SuccessfullyDeletedPermission(int id)
        {
            return $"Sucessfully deleted permission with Id {id}!";
        }

        // Roles
        public static string SuccessfullyEditedRole(string name)
        {
            return $"Sucessfully edited role {name}!";
        }

        public static string SuccessfullyCreatedRole(string name)
        {
            return $"Sucessfully created role {name}!";
        }

        public static string SuccessfullyDeletedRole(int roleId)
        {
            return $"Sucessfully deleted role {roleId}!";
        }
    }
}
