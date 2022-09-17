#nullable enable
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NetProxy.Proxies;

namespace NetProxy
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  proxy.exe [Proxy Definitions]");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("  proxy.exe 8080~PrivateHost");
                Console.WriteLine("  proxy.exe 8080~PrivateHost:80/TCP");
                Console.WriteLine();
                Console.WriteLine("Proxy Definition Formats{ProxyParser.PortSeparator}");
                Console.WriteLine($"  [IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.ProtocolSeparator}[Protocol]");
                Console.WriteLine($"  [IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.ProtocolSeparator}[Protocol]");
                Console.WriteLine($"  [IP]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.ProtocolSeparator}[Protocol]");
                Console.WriteLine($"  [Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.ProtocolSeparator}[Protocol]");
                Console.WriteLine($"  [Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.ProtocolSeparator}[Protocol]");
                Console.WriteLine($"  [IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]");
                Console.WriteLine($"  [IP]{ProxyParser.PortSeparator}[Port]{ProxyParser.TargetSeparator}[IP]");
                Console.WriteLine($"  [IP]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]");
                Console.WriteLine($"  [Port]{ProxyParser.TargetSeparator}[IP]{ProxyParser.PortSeparator}[Port]");
                Console.WriteLine($"  [Port]{ProxyParser.TargetSeparator}[IP]");

            }
            try
            {
                var tasks = args.SelectMany(c =>
                {
                    var proxyConfig = ProxyParser.ParseProxyConfig(c);
                    return ProxyFromConfig(proxyConfig);
                });
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred : {ex}");
            }
        }

        private static IEnumerable<Task> ProxyFromConfig(ProxyConfig config)
        {
            bool protocolHandled = false;

            if (config.Protocol.HasFlag(Protocol.UDP))
            {
                protocolHandled = true;
                Task task;
                try
                {
                    var proxy = new UdpProxy();
                    task = proxy.Start(config.Local, config.Remote);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start : {ex.Message}");
                    throw;
                }

                yield return task;
            }

            if (config.Protocol.HasFlag(Protocol.TCP))
            {
                protocolHandled = true;
                Task task;
                try
                {
                    var proxy = new TcpProxy();
                    task = proxy.Start(config.Local, config.Remote);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to start : {ex.Message}");
                    throw;
                }

                yield return task;
            }

            if (!protocolHandled)
            {
                throw new InvalidOperationException($"protocol not supported");
            }
        }
    }
}