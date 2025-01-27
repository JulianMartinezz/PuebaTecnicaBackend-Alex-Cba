using System.Globalization;

namespace HR_Medical_Records.Middleware.Exceptions
{
    /// <summary>
    /// Custom exception class for handling bad request errors from the client side.
    /// Inherits from the base <see cref="Exception"/> class.
    /// </summary>
    public class ExceptionBadRequestClient : Exception
    {
        public ExceptionBadRequestClient() : base() { }

        public ExceptionBadRequestClient(string message) : base(message) { }

        public ExceptionBadRequestClient(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
