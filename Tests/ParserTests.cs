using NetProxy;

namespace Tests
{
    public class ParserTests
    {

        [Fact]
        // {IP}:{Port}~{IP}:{Port}/{Protocol}
        public void TestFullParse()
        {
            var config = ProxyParser.ParseProxyConfig("1.2.3.4:80~5.6.7.8:81/udp");
            Assert.Equal("1.2.3.4", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(81, config.Remote.Port);
            Assert.Equal(Protocol.UDP, config.Protocol);
        }

        [Fact]
        // {IP}:{Port}~{IP}/{Protocol}
        public void TestParseInferRemotePort()
        {
            var config = ProxyParser.ParseProxyConfig("1.2.3.4:80~5.6.7.8/udp");
            Assert.Equal("1.2.3.4", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.UDP, config.Protocol);
        }

        [Fact]
        // {IP}~{IP}:{Port}/{Protocol}
        public void TestParseInferLocalPort()
        {
            var config = ProxyParser.ParseProxyConfig("1.2.3.4~5.6.7.8:81/udp");
            Assert.Equal("1.2.3.4", config.Local.IP);
            Assert.Equal(81, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(81, config.Remote.Port);
            Assert.Equal(Protocol.UDP, config.Protocol);
        }

        [Fact]
        // {Port}~{IP}:{Port}/{Protocol}
        public void TestParseBindAllLocal()
        {
            var config = ProxyParser.ParseProxyConfig("80~5.6.7.8:80/udp");
            Assert.Equal("127.0.0.1", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.UDP, config.Protocol);
        }

        [Fact]
        // {Port}~{IP}/{Protocol}
        public void TestParseBindAllLocalInferRemotePort()
        {
            var config = ProxyParser.ParseProxyConfig("80~5.6.7.8/udp");
            Assert.Equal("127.0.0.1", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.UDP, config.Protocol);
        }

        [Fact]
        // {IP}:{Port}~{IP}:{Port}
        public void TestParseFullNoProtocol()
        {
            var config = ProxyParser.ParseProxyConfig("1.2.3.4:80~5.6.7.8:81");
            Assert.Equal("1.2.3.4", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(81, config.Remote.Port);
            Assert.Equal(Protocol.TCP, config.Protocol);
        }

        [Theory]
        [InlineData("1.2.3.4:80~5.6.7.8:80/TCP")]
        [InlineData("1.2.3.4:80~5.6.7.8/TCP")]
        [InlineData("1.2.3.4~5.6.7.8:80/TCP")]
        [InlineData("1.2.3.4:80~5.6.7.8:80")]
        [InlineData("1.2.3.4:80~5.6.7.8")]
        [InlineData("1.2.3.4~5.6.7.8:80")]
        public void TestMultipleParse(string input)
        {
            var config = ProxyParser.ParseProxyConfig(input);
            Assert.Equal("1.2.3.4", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.TCP, config.Protocol);
        }

        [Theory]
        [InlineData("80~5.6.7.8:80/TCP")]
        [InlineData("80~5.6.7.8/TCP")]
        [InlineData("80~5.6.7.8:80")]
        [InlineData("80~5.6.7.8")]
        public void TestMultipleParseNullLocal(string input)
        {
            var config = ProxyParser.ParseProxyConfig(input);
            Assert.Equal("127.0.0.1", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("5.6.7.8", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.TCP, config.Protocol);
        }

        [Theory]
        [InlineData("CurrentHost:80~RemoteHost:80/TCP")]
        [InlineData("CurrentHost:80~RemoteHost/TCP")]
        [InlineData("CurrentHost~RemoteHost:80/TCP")]
        [InlineData("CurrentHost:80~RemoteHost:80")]
        [InlineData("CurrentHost:80~RemoteHost")]
        [InlineData("CurrentHost~RemoteHost:80")]
        public void TestMultipleParseWithHostnames(string input)
        {
            var config = ProxyParser.ParseProxyConfig(input);
            Assert.Equal("CurrentHost", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("RemoteHost", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.TCP, config.Protocol);
        }

        [Theory]
        [InlineData("80~RemoteHost:80/TCP")]
        [InlineData("80~RemoteHost/TCP")]
        [InlineData("80~RemoteHost:80")]
        [InlineData("80~RemoteHost")]
        public void TestMultipleParseWithHostnamesNullLocal(string input)
        {
            var config = ProxyParser.ParseProxyConfig(input);
            Assert.Equal("127.0.0.1", config.Local.IP);
            Assert.Equal(80, config.Local.Port);
            Assert.Equal("RemoteHost", config.Remote.IP);
            Assert.Equal(80, config.Remote.Port);
            Assert.Equal(Protocol.TCP, config.Protocol);
        }

    }
}