using System.ComponentModel;
using AvaPro.Models.Server;
using AvaPro.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AvaPro.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    string blockText = "显示";
    string startServerText = "启动服务端";
    string sendMsgText = "发送消息";

    public string BlockText
    {
        get => blockText;
        set
        {
            blockText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BlockText)));
        }
    }

    public string StartServerText
    {
        get => startServerText;
        set
        {
            startServerText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartServerText)));
        }
    }

    public string SendMsgText
    {
        get => sendMsgText;
        set
        {
            sendMsgText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SendMsgText)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void StartServerButtonClicked()
    {
        var service = App.Current.Services.GetService<ISocketService>();
        BlockText = service.OpenService(new ServerMetaData { IPAddress = "127.0.0.1", Port = 40001, ListeningNum = 10 });
        Console.WriteLine("打印");
    }

    public void SendMsgButtonClicked()
    {
        var service = App.Current.Services.GetService<ISocketService>();
        BlockText = service.SendMsg("发消息了").Result;
    }
}