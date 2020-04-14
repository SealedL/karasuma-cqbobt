using System;
using System.IO;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.Posts;

namespace cqbot
{
    internal static class EventHandler
    {
        public static async Task MessageProcess(HttpApiClient api,
            Sisters.WudiLib.Posts.Message message)
        {
            switch (message)
            {
                case GroupMessage groupMessage:
                {
                    var raw = groupMessage.Content.Text;
                    if (raw.StartsWith('/'))
                    {
                        var processed = CommandSplit(raw);
                        var command = processed[0];
                        var param = processed[1];
                        if (string.CompareOrdinal(command, "/echo") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, param);
                        }
                        else if (string.CompareOrdinal(command, "/help") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Help);
                        }
                        else if (string.CompareOrdinal(command, "/support") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Support);
                        }
                        else if (string.CompareOrdinal(command, "/comment") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Comment);
                            var comment = $"来自“{groupMessage.GroupId}:@{groupMessage.Sender.InGroupName}”的一条留言：\n";
                            comment += param;
                            comment += $"\n留言时间：{groupMessage.Time.ToLocalTime()}";
                            await api.SendPrivateMessageAsync(SharedContent.MasterId, comment);
                        }
                        else if (string.CompareOrdinal(command, "/image-test") == 0)
                        {
                            var bytes = await File.ReadAllBytesAsync("/home/cqbot/images/test.png");
                            var image = SendingMessage.ByteArrayImage(bytes);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, image);
                        }
                        else if (string.CompareOrdinal(command, "/wolfram") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Wait);
                            var url = ImageCapt.UrlHandle(param);
                            try
                            {
                                var bytes = ImageCapt.CaptCall(url);
                                if (bytes != null)
                                {
                                    var image = SendingMessage.ByteArrayImage(bytes);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, image);
                                }
                                else
                                {
                                    await api.SendGroupMessageAsync(groupMessage.GroupId,SharedContent.Error);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                await api.SendGroupMessageAsync(groupMessage.GroupId,SharedContent.Error);
                            }
                        }
                        else
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.SyntaxError);
                        }
                    }
                    break;
                }
                case PrivateMessage _:
                    break;
            }
        }

        private static string[] CommandSplit(string raw)
        {
            var splitIndex = raw.IndexOf(' ');
            var command = splitIndex > -1 ? raw.Substring(0, splitIndex) : raw;
            string param = null;
            if (splitIndex > -1)
            {
                param = raw.Substring(splitIndex + 1);
            }
            string[] result = {command, param};
            return result;
        }
    }
}