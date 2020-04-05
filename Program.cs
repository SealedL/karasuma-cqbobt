using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.WebSocket;
using Sisters.WudiLib.Posts;
using Sisters.WudiLib.Responses;

namespace cqbot
{
    class Program
    {
        static void Main(string[] args)
        {

            string help = "使用方法：\n以\'/\'为开头加上功能列表里的功能命令和后续参数\n\n功能列表：\n1. echo：复读命令后的字符串；\n2. support：显示该项目GitHub仓库链接，欢迎贡献。\n";
            string support = "本项目GitHub仓库链接：https://github.com/SealedL/karasuma-cqbot\n欢迎贡献";


            var httpApi = new HttpApiClient();
            httpApi.ApiAddress = "http://127.0.0.1:5700/";

            var webSocketEvent = new CqHttpWebSocketEvent("ws://127.0.0.1:6700/event");
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
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, help);
                            }

                            if (String.Compare(text, 0, "/echo ", 0, 5) == 0)
                            {
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, text.Substring(6));
                            }

                            if (String.Compare(text, 0, "/support ", 0, 7) == 0)
                            {
                                await httpApi.SendGroupMessageAsync(groupMessage.GroupId, support);
                            }
                        }
                    }
                }
                else if (e is PrivateMessage privateMessage)
                {
                    Console.WriteLine("QQ ID: " + privateMessage.UserId);
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
                    Console.WriteLine("Available: {0}, Listening {1}", webSocketEvent.IsAvailable, webSocketEvent.IsListening);
                }
            });

            Console.ReadLine();
        }
    }
}
