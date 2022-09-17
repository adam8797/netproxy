[![NuGet Badge](https://buildstats.info/nuget/NetProxyCLI)](https://www.nuget.org/packages/NetProxyCLI/)
# NetProxy
Netproxy is a simple ipv4 UDP & TCP proxy based on .NET 5.0. The upstream repository uses a configuration file, and my use case was more temporary, therefore a CLI seemed more appropriate.

## Installing
```
dotnet tool install --global NetProxyCLI
``` 

## Why? 
When dealing with corporate firewalls and segregated networks, sometimes you need a quick and dirty reverse proxy in order to test software, or facilitate development. Setting up a full instance
of nginx isn't necessarily difficult, but way overkill for this situation. 

I found the upstream repository, refactored it to my liking, removed IPv6 support (I'm open to bringing it back, its just not in my use case), and published it under a new name


## Usage
This version of NetProxy uses a CLI that supports the following formats. That parser is quite naive, so please report any issues

```
Proxy Definition Formats:
  [IP]:[Port]~[IP]:[Port]/[Protocol]
  [IP]:[Port]~[IP]/[Protocol]
  [IP]~[IP]:[Port]/[Protocol]
  [Port]~[IP]:[Port]/[Protocol]
  [Port]~[IP]/[Protocol]
  [Port]~[Port]/[Protocol]
  [IP]:[Port]~[IP]:[Port]
  [IP]:[Port]~[IP]
  [IP]~[IP]:[Port]
  [Port]~[IP]:[Port]
  [Port]~[IP]
  [Port]~[Port]
```