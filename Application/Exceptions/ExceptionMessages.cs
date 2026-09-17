namespace Artway.Application.Exceptions
{
    public static class ExceptionMessages
    {
        public static string NoRecordFound = "No record found";

        // All Exception messages regarding Accounts
        public static string AccountNotFoundwithId(int id) => $"No account was found with Id: {id}";
        public const string AccountNotFound = "Account not found";
        public const string AccountInsertException = "An exception occurred while adding new account";
    }
}