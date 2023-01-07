using System.Threading.Tasks;
using AvaPro.Models.Server;

namespace AvaPro.Services;

public interface ISocketService
{
    public string OpenService(ServerMetaData serverMetaData);
    public Task<string> SendMsg(string msg);
}