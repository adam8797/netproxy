using System;

namespace NetProxy;

public record class ProxyConfig(Protocol Protocol, Target Local, Target Remote);
public record class Target(string IP, ushort Port);

[Flags]
public enum Protocol
{
    TCP = 1,
    UDP = 2,
    ANY = TCP | UDP
}