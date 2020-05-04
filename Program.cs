using System;
using System.Threading;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.WebSocket;

namespace cqbot
{
    internal static class Program
    {
        private static void Main()
        {

            var httpApi = new HttpApiClient {ApiAddress = SharedContent.HttpApiPath};

            var webSocketEvent = new CqHttpWebSocketEvent(SharedContent.WebSocketEventPath) {ApiClient = httpApi};



            // 订阅事件。
            webSocketEvent.MessageEvent += async (api, message) =>
            {
                await EventHandler.MessageProcess(api, message);
            };

            webSocketEvent.FriendRequestEvent += (api, e) => false;

            webSocketEvent.GroupInviteEvent += (api, e) => true;

            // 连接前等待 3 秒观察状态。
            Task.Delay(TimeSpan.FromSeconds(3)).Wait();

            // 连接（开始监听上报）。
            CreateConnect(webSocketEvent);

            Console.ReadLine();
        }

        private static void CreateConnect(CqHttpWebSocketEvent webSocketEvent)
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
                Task.Delay(TimeSpan.FromSeconds(5), cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
                cancellationTokenSource.Dispose();
                CreateConnect(webSocketEvent);
                Task.Delay(-1, cancellationTokenSource.Token).Wait(cancellationTokenSource.Token);
            }
        }
    }
}
