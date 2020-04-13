using System;
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
                        var splitIndex = raw.IndexOf(' ');
                        var command = splitIndex > -1 ? raw.Substring(0, splitIndex) : raw;
                        if (string.CompareOrdinal(command, "/echo") == 0)
                        {
                            var answer = raw.Substring(splitIndex + 1);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, answer);
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
                            comment += raw.Substring(splitIndex + 1);
                            comment += $"\n留言时间：{groupMessage.Time.ToLocalTime()}";
                            await api.SendPrivateMessageAsync(SharedContent.MasterId, comment);
                        }
                        else if (string.CompareOrdinal(command, "/auth") == 0)
                        {
                            if (groupMessage.Sender.UserId == SharedContent.MasterId)
                            {
                                var left = raw.Substring(splitIndex + 1);
                                var secondIndex = left.IndexOf(' ');
                                if (secondIndex > -1)
                                {
                                    var param = left.Substring(splitIndex + 1, secondIndex);
                                    var targetId = left.Substring(secondIndex + 1);
                                    if (string.CompareOrdinal(param, "--add") == 0 ||
                                        string.CompareOrdinal(param, "-a") == 0)
                                    {
                                        try
                                        {
                                            Auth.WriteAdminIds(long.Parse(targetId));
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.SyntaxError);
                                        }
                                    }
                                    else if (string.CompareOrdinal(param, "--delete") == 0 ||
                                             string.CompareOrdinal(param, "-d") == 0)
                                    {
                                        try
                                        {
                                            Auth.DeleteAdminIds(long.Parse(targetId));
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine(e);
                                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.SyntaxError);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.PermissionError);
                            }
                        }
                        else if (string.CompareOrdinal(command, "/image-test") == 0)
                        {
                            var image = SendingMessage.LocalImage("/home/cqbot/images/test.png");
                            await api.SendGroupMessageAsync(groupMessage.GroupId, image);
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
    }
}