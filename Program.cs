using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.WebSocket;

namespace cqbot
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpApi = new HttpApiClient();
            httpApi.ApiAddress = "http://127.0.0.1:5700/";

            var webSocketEvent = new CqHttpWebSocketEvent("ws://127.0.0.1:6700/event");
            webSocketEvent.ApiClient = httpApi;

            var userList = new List<long> { 921228653 };

            // 订阅事件。
            webSocketEvent.MessageEvent += async (api, e) =>
            {
                Console.WriteLine(e.Content.Text);
                Console.WriteLine(e.Endpoint.ToString());
                if (userList.Contains(e.UserId))
                {
                    await httpApi.SendPrivateMessageAsync(e.UserId, e.Content);
                }
            };
            webSocketEvent.FriendRequestEvent += (api, e) =>
            {
                return true;
            };
            webSocketEvent.GroupInviteEvent += (api, e) =>
            {
                return true;
            }; // 可以通过 return 的方式响应请求，与使用 HTTP 时没有差别。

            // 每秒打印 WebSocket 状态。
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    Console.WriteLine("Available: {0}, Listening {1}", webSocketEvent.IsAvailable, webSocketEvent.IsListening);
                }
            });

            // 连接前等待 3 秒观察状态。
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();

            // 连接（开始监听上报）。
            var cancellationTokenSource = new CancellationTokenSource();
            webSocketEvent.StartListen(cancellationTokenSource.Token); // 首次连接必须成功。

            Console.ReadLine();
        }
    }
}
