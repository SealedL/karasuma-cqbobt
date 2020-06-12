using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
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
                            var senderName = groupMessage.Sender.InGroupName ?? groupMessage.Sender.Nickname;
                            var comment = $"来自“{groupMessage.GroupId}:@{senderName}”的一条留言：\n";
                            comment += param;
                            comment += $"\n留言时间：{groupMessage.Time.ToLocalTime()}";
                            await api.SendPrivateMessageAsync(SharedContent.MasterId, comment);
                        }
                        else if (string.CompareOrdinal(command, "/wolfram") == 0)
                        {
                            var userId = groupMessage.Sender.UserId;
                            if (!Queue.IsUserListed(userId))
                            {
                                try
                                {
                                    Queue.AddUserToList(userId);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Wait);
                                    var url = ImageCapt.UrlHandle(param);
                                    var time = groupMessage.Time.LocalDateTime;
                                    await ImageCapt.CaptCall(url, userId, time);
                                    var bytes = await File.ReadAllBytesAsync($"/home/cqbot/images/answer-{time.Minute}-{time.Hour}-{time.Day}-{time.Month}-{time.Year}-{userId}.png");
                                    var image = SendingMessage.ByteArrayImage(bytes);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, image);
                                    Queue.RemoveUserFromList(userId);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Error);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, e.Message);
                                    Queue.RemoveUserFromList(userId);
                                    ImageCapt.KillChromeProcess();
                                }
                            }
                            else
                            {
                                await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Busy);
                            }
                        }
                        else if (string.CompareOrdinal(command, "/encode") == 0)
                        {
                            var encoded = HttpUtility.UrlEncode(param);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, encoded);
                        }
                        else if (string.CompareOrdinal(command, "/decode") == 0)
                        {
                            var decoded = HttpUtility.UrlDecode(param);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, decoded);
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