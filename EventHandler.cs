using System;
using System.Threading.Tasks;
using Sisters.WudiLib;
using Sisters.WudiLib.Posts;
using Sisters.WudiLib.WebSocket;

namespace cqbot
{
    static class EventHandler
    {
        public static async Task MessageProcess(HttpApiClient api,
            Sisters.WudiLib.Posts.Message message)
        {
            if (message is GroupMessage groupMessage)
            {
                string raw = groupMessage.Content.Text;
                if (raw.StartsWith('/'))
                {
                    string[] subs = raw.Split(' ');
                    if (String.Compare(subs[0], "/echo") == 0)
                    {
                        string answer = "";
                        for (int i = 1; i < subs.GetLength(0); i++)
                        {
                            answer += (subs[i] + " ");
                        }
                        answer += "\b";
                        await api.SendGroupMessageAsync(groupMessage.GroupId, answer);
                    }
                    else if (String.Compare(subs[0], "/help") == 0)
                    {
                        await api.SendGroupMessageAsync(groupMessage.GroupId, SharedText.Help);
                    }
                    else if (String.Compare(subs[0], "/support") == 0)
                    {
                        await api.SendGroupMessageAsync(groupMessage.GroupId, SharedText.Support);
                    }
                }
            }
            else if (message is PrivateMessage privateMessage)
            {

            }
        }
    }
}