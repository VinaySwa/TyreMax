namespace TyreSaleService.Common
{
    /// <summary>
    /// Custom exception class for data access errors.
    /// </summary>
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception inner)
            : base(message, inner) { }
    }
}
