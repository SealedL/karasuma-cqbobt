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
                        if (string.CompareOrdinal(command, "/help") == 0)
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
                        else if (string.CompareOrdinal(command, "/encode") == 0)
                        {
                            var encoded = HttpUtility.UrlEncode(param);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, "对应字符（串）是" + encoded);
                        }
                        else if (string.CompareOrdinal(command, "/decode") == 0)
                        {
                            var decoded = HttpUtility.UrlDecode(param);
                            await api.SendGroupMessageAsync(groupMessage.GroupId, "对应字符（串）是" + decoded);
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