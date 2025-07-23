using System;
using SignNow.Net.Exceptions;

namespace SignNow.Net.Internal.Helpers
{
    /// <summary>
    /// Helper class that prevents possible null reference exceptions.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensures that the specified argument is not null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="argumentName">The argument name.</param>
        /// <example>
        ///    Guard.ArgumentNotNull(input, nameof(input));
        /// </example>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (null == argument)
                throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Ensures that the specified object property is not null.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="argumentName">Name of the argument for validation.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void PropertyNotNull(object property, string argumentName, string message = default)
        {
            if (null == property)
                throw new ArgumentException(message, argumentName);
        }

        /// <summary>
        /// Ensures that the specified string is not null, whitespace or empty.
        /// </summary>
        /// <param name="argument">Input string for validation.</param>
        /// <param name="argumentName">Name of the argument for validation.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentException">If <paramref name="argument"/> is null, empty or whitespace.</exception>
        public static void ArgumentIsNotEmptyString(string argument, string argumentName, string message = default)
        {
            if (string.IsNullOrEmpty(argument) || string.IsNullOrWhiteSpace(argument))
                throw new ArgumentException(message ?? ExceptionMessages.StringNotNullOrEmptyOrWhitespace, argumentName);
        }
    }
}
