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
                        var subs = raw.Split(' ');
                        if (string.CompareOrdinal(subs[0], "/echo") == 0)
                        {
                            var answer = "";
                            for (var i = 1; i < subs.GetLength(0); i++)
                            {
                                if (i != subs.GetLength(0) - 1)
                                {
                                    answer += (subs[i] + " ");
                                }
                                else
                                {
                                    answer += subs[i];
                                }
                            }
                            await api.SendGroupMessageAsync(groupMessage.GroupId, answer);
                        }
                        else if (string.CompareOrdinal(subs[0], "/help") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Help);
                        }
                        else if (string.CompareOrdinal(subs[0], "/support") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Support);
                        }
                        else if (string.CompareOrdinal(subs[0], "/comment") == 0)
                        {
                            await api.SendGroupMessageAsync(groupMessage.GroupId, SharedContent.Comment);
                            var comment = $"来自“{groupMessage.GroupId}:@{groupMessage.Sender.InGroupName}”的一条留言：\n";
                            for (var i = 1; i < subs.GetLength(0); i++)
                            {
                                if (i != subs.GetLength(0) - 1)
                                {
                                    comment += (subs[i] + " ");
                                }
                                else
                                {
                                    comment += subs[i];
                                }
                            }
                            comment += $"\n留言时间：{groupMessage.Time.ToLocalTime()}";
                            await api.SendPrivateMessageAsync(SharedContent.MasterId, comment);
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