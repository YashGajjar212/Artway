namespace Artway.Application.Exceptions
{
    public static class ExceptionMessages
    {
        public static string NoRecordFound = "No record found";

        // All Exception messages regarding Customers
        public static string CustomerNotFoundwithId(int id) => $"No customer was found with Id: {id}";
        public const string CustomerNotFound = "Customer not found";
        public const string CustomerInsertException = "An exception occurred while adding new customer";
    }
}