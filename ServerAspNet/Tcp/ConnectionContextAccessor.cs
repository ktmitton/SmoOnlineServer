using Microsoft.AspNetCore.Connections;

namespace SuperMarioOdysseyOnline.Server.Tcp;

public interface ITcpConnectionContextAccessor
{
    ConnectionContext? ConnectionContext { get; set; }
}

internal class DefaultTcpConnectionContextAccessor : ITcpConnectionContextAccessor
{
    public ConnectionContext? ConnectionContext { get; set; }
}
