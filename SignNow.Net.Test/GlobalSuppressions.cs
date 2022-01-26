// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppression either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Naming", "CA1707:Identifiers should not contain underscores", Scope = "module")]

// Suppression for various Mock usage
[assembly: SuppressMessage("Microsoft.Usage", "CA1801:Review unused parameters.", Scope = "module")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]

// Suppression for string messages in tests
[assembly: SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1304:Specify CultureInfo")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1305:Specify IFormatProvider")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1307:Specify StringComparison for clarity")]
[assembly: SuppressMessage("Microsoft.Globalization", "CA1308:Normalize strings to uppercase")]

[assembly: SuppressMessage("Microsoft.Design", "CA1051:Do not declare visible instance fields")]
[assembly: SuppressMessage("Microsoft.Design", "CA1054:URI parameters should not be strings")]
