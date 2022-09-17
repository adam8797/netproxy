using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProxy
{
    public class ProxyParser
    {
        public const string PortSeparator = ":";
        public const string TargetSeparator = "~";
        public const string ProtocolSeparator = "/";

        public static Target ParseTarget(string s)
        {
            var halves = s.Split(PortSeparator);
            if (halves.Length > 2)
                throw new Exception("Bad target format");

            if (halves.Length == 1)
            {
                if (ushort.TryParse(halves[0], out var port))
                {
                    return new Target("127.0.0.1", port);
                }
                else
                {
                    return new Target(halves[0], 0);
                }
            }
            else
            {
                return new Target(halves[0], ushort.Parse(halves[1]));
            }
        }

        public static ProxyConfig ParseProxyConfig(string s)
        {
            var halves = s.Split(TargetSeparator);
            if (halves.Length != 2)
                throw new Exception("Unable to parse, must be in a known format");


            Target local = ParseTarget(halves[0]);
            Target remote;

            Protocol protocol;
            if (halves[1].Contains(ProtocolSeparator))
            {
                var remoteRaw = halves[1].Split(ProtocolSeparator);
                if (remoteRaw.Length != 2)
                    throw new Exception("Error while parsing");

                remote = ParseTarget(remoteRaw[0]);
                protocol = Enum.Parse<Protocol>(remoteRaw[1].ToUpper());
            }
            else
            {
                remote = ParseTarget(halves[1]);
                protocol = Protocol.TCP;
            }

            if (local.Port == 0)
            {
                local = new Target(local.IP, remote.Port);
            }

            if (remote.Port == 0)
            {
                remote = new Target(remote.IP, local.Port);
            }

            if (local.Port == 0 || remote.Port == 0)
                throw new Exception("Unable to parse port");

            return new ProxyConfig(protocol, local, remote);
        }
    }
}
