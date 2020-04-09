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
                        if (splitIndex != -1)
                        {
                            var command = raw.Substring(0, splitIndex);
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
                                    try
                                    {
                                        var newAdminId = long.Parse(raw.Substring(splitIndex + 1));
                                        Auth.WriteAdminIds(newAdminId);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                        await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.SyntaxError);
                                    }
                                }
                                else
                                {
                                    await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.PermissionError);
                                }
                            }
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