using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace cqbot
{
    internal static class SharedContent
    {
        //API related strings
        public const string HttpApiPath = "http://127.0.0.1:5700/";
        public const string WebSocketEventPath = "ws://127.0.0.1:6700/event";
        
        //Commands related strings
        public const string Help = "用法：/[命令] [参数(可选)]\n" +
                                   "命令列表：\n" +
                                   "help       显示本帮助内容；\n\n" +
                                   "support    显示本项目GitHub仓库链接;\n\n" +
                                   "echo       复读命令后的内容；\n\n" + 
                                   "comment    给作者留言。\n\n";
        public const string Support = "本项目GitHub仓库链接：" +
                                      "https://github.com/SealedL/karasuma-cqbot\n" +
                                      "欢迎帮助我改进程序。";
        public const string Comment = "您已成功送出一条留言。";
        
        //User ID related
        private const string JsonFilePath = "./bin/Debug/netcoreapp3.1/ids.json";
        private static readonly string JsonString = File.ReadAllText(JsonFilePath);
        private static readonly IdType Ids = JsonSerializer.Deserialize<IdType>(JsonString);
        public static readonly long MasterId = Ids.MasterId;
        public static readonly List<long> AdminIds = Ids.AdminIds;
    }
}