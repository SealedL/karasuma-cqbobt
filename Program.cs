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

            var httpApi = new HttpApiClient();
            httpApi.ApiAddress = SharedContent.HttpApiPath;

            var webSocketEvent = new CqHttpWebSocketEvent(SharedContent.WebSocketEventPath);
            webSocketEvent.ApiClient = httpApi;



            // 订阅事件。
            webSocketEvent.MessageEvent += async (api, message) =>
            {
                await EventHandler.MessageProcess(api, message);
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
            createConnect(webSocketEvent);

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

        static void createConnect(CqHttpWebSocketEvent webSocketEvent)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                webSocketEvent.StartListen(cancellationTokenSource.Token); // 首次连接必须成功。
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(2));
                Task.Delay(TimeSpan.FromSeconds(5)).Wait();
                cancellationTokenSource.Dispose();
                createConnect(webSocketEvent);
                Task.Delay(-1).Wait();
            }
        }
    }
}
