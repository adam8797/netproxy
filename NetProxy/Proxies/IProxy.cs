using System.Threading.Tasks;

namespace NetProxy.Proxies;

internal interface IProxy
{
    Task Start(Target local, Target remote);
}