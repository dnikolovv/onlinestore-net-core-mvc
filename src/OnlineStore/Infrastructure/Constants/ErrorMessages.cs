namespace OnlineStore.Infrastructure.Constants
{
    public static class ErrorMessages
    {
        // User
        public const string INVALID_USERNAME_OR_PASSWORD = "Invalid username or password";

        public const string USERNAME_ALREADY_TAKEN = "That username is already taken.";

        public const string PASSWORDS_DONT_MATCH = "Passwords do not match.";

        // Categories
        public const string CATEGORY_NAME_ALREADY_TAKEN = "That category name is already taken.";

        public const string CATEGORY_DOESNT_EXIST = "The category you are trying to delete does not exist.";

        // Cart
        public const string CART_IS_EMPTY = "Your cart is empty";
    }
}
