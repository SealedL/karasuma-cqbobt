using System;
using System.Collections.Generic;
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
            var userList = new List<long>();
            
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
                            var userId = groupMessage.Sender.UserId;
                            var isLocked = await ImageCapt.IsUserListed(userList, userId);
                            if (!isLocked)
                            {
                                try
                                {
                                    await ImageCapt.AddUserToList(userList, userId);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Wait);
                                    var url = ImageCapt.UrlHandle(param);
                                    await ImageCapt.CaptCall(url);
                                    var bytes = await File.ReadAllBytesAsync("/home/cqbot/images/answer.png");
                                    var image = SendingMessage.ByteArrayImage(bytes);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, image);
                                    await ImageCapt.RemoveUserFromList(userList, userId);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Error);
                                }
                            }
                            {
                                await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Busy);
                            }
                        }
                        else if (string.CompareOrdinal(command, "/encode") == 0)
                        {
                            var encoded = ImageCapt.UrlHandle(param);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, encoded);
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