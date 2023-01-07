using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AvaPro.Models.Server;

namespace AvaPro.Services;

public class SocketServer : ISocketService
{
    private Socket _socket;
    private Thread _threadClient = null;

    private Dictionary<string, Socket> _dic = new();

    public string OpenService(ServerMetaData serverMetaData)
    {
        // return Task.Run(() =>
        // {
            string result;
            //IP版本4，字节流方式传递信息，TCP协议
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(serverMetaData.IPAddress);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, serverMetaData.Port);
            try
            {
                _socket.Bind(ipEndPoint);

                //监听并指定最大监听数量
                _socket.Listen(serverMetaData.ListeningNum);
                _threadClient = new Thread(Listening);
                _threadClient.IsBackground = true;
                result = "开启成功";
                Console.WriteLine("开启成功");
            }
            catch (Exception)
            {
                result = "开启失败";
            }

            return result;
        // });
    }


    private void Listening()
    {
        while (true)
        {
            Socket socketClient = _socket.Accept();
            string strClient = socketClient.RemoteEndPoint.ToString();
            _dic.Add(strClient, socketClient);
            Thread thread = new Thread(ReceiveMsg);
            thread.IsBackground = true;
            thread.Start(socketClient);
        }
    }

    private void ReceiveMsg(object socketClient)
    {
        Socket socket = socketClient as Socket;
        while (true)
        {
            byte[] arrRcv = new byte[1024 * 1024 * 2];
            int Length = socket.Receive(arrRcv);
            if (Length == 0)
            {
                string str = socket.RemoteEndPoint.ToString();
                //从设备列表去除
                _dic.Remove(str);
                return;
            }
            else
            {
                string str = Encoding.Unicode.GetString(arrRcv, 0, Length);
            }
        }
    }

    public Task<string> SendMsg(string msg)
    {
        return Task.Run(() =>
        {
            byte[] arrSend = Encoding.Unicode.GetBytes(msg, 0, msg.Length);
            if (arrSend.Length != 0)
            {
                if (_dic.Count != 0)
                {
                    foreach (var item in _dic)
                    {
                        try
                        {
                            item.Value.Send(arrSend);
                        }
                        catch (Exception e)
                        {
                            return "发送失败";
                        }
                    }

                    return "发送成功";
                }
                else
                {
                    return "没有客户端连接";
                }
            }
            else
            {
                return "没有输入文本，发送失败";
            }
        });
    }
}