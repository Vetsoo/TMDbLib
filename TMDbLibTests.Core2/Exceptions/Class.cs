using System;

namespace TMDbLibTests.Core2.Exceptions
{
    public class ConfigurationErrorsException : Exception
    {
        public ConfigurationErrorsException(string message) : base(message)
        {

        }
    }
}
