using System;
namespace JSIntegration
{
    /// <summary>
    /// Exception for JSIntegration, when something's wrong.
    /// </summary>
    public class JSIntegrationException : Exception
    {
        public string ErrorCode;
        public string ErrorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JSIntegration.JSIntegrationException"/> class.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="errorMessage">Error message.</param>
        public JSIntegrationException(string errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }
    }
}
