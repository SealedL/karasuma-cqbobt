using System;
using System.Threading;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.WebSocket;
using Sisters.WudiLib.Posts;

namespace cqbot
{
    class Program
    {
        static void Main(string[] args)
        {
            const string HttpApiAdress = "http://127.0.0.1:5700/";
            const string WebSocketEventAdress = "ws://127.0.0.1:6700/event";

            const string Help = "用法：/[命令] [参数(可选)]\n" +
                "命令列表：\n" +
                "help       显示本帮助内容；\n\n" +
                "support    显示本项目GitHub仓库链接;\n\n" +
                "echo       复读命令后的内容。";
            const string Support = "本项目GitHub仓库链接：" +
                "https://github.com/SealedL/karasuma-cqbot\n" +
                "欢迎帮助我改进程序\n";

            var httpApi = new HttpApiClient();
            httpApi.ApiAddress = HttpApiAdress;

            var webSocketEvent = new CqHttpWebSocketEvent(WebSocketEventAdress);
            webSocketEvent.ApiClient = httpApi;

            // 订阅事件。
            webSocketEvent.MessageEvent += async (api, e) =>
            {
                if (e is GroupMessage groupMessage)
                {
                    Console.WriteLine("Group ID: " + groupMessage.GroupId);
                    if (e.Content.IsPlaintext)
                    {
                        string text = e.Content.Text;
                        if (text.StartsWith('/'))
                        {
                            if (String.Compare(text, 0, "/help ", 0, 5) == 0)
                            {
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, Help);
                            }

                            if (String.Compare(text, 0, "/echo ", 0, 5) == 0)
                            {
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, text.Substring(6));
                            }

                            if (String.Compare(text, 0, "/support ", 0, 7) == 0)
                            {
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, Support);
                            }
                        }
                    }
                }
                else if (e is PrivateMessage privateMessage)
                {
                }
            };

            webSocketEvent.FriendRequestEvent += (api, e) =>
            {
                return false;
            };

            webSocketEvent.GroupInviteEvent += (api, e) =>
            {
                return true;
            };

            // 连接前等待 3 秒观察状态。
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();

            // 连接（开始监听上报）。
            var cancellationTokenSource = new CancellationTokenSource();
            webSocketEvent.StartListen(cancellationTokenSource.Token); // 首次连接必须成功。

            // 每10秒打印 WebSocket 状态。
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(10000);
                    Console.WriteLine($"Available: {webSocketEvent.IsAvailable}," +
                        $"Listening {webSocketEvent.IsListening}");
                }
            });

            Console.ReadLine();
        }
    }
}
