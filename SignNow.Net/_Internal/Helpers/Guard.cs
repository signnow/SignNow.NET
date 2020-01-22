using System;

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
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
