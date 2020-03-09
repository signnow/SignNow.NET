using System.Runtime.CompilerServices;

#if DEBUG
[assembly: InternalsVisibleTo("SignNow.Net.Test")]
[assembly: InternalsVisibleTo("Bogus")]
// Requires for Moq ProxyBuilder
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
#endif
