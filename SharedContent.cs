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
                                   "help [无参数] #显示本帮助内容；\n\n" +
                                   "support [无参数] #显示本项目GitHub仓库链接;\n\n" +
                                   "echo [string] #复读[string]；\n\n" + 
                                   "comment [string] #给作者留言[string]；\n\n" + 
                                   "encode [string] #将[string]按URL链接的格式进行转码；\n\n" + 
                                   "wolfram [string] #利用Wolfram|Alpha搜索引擎对[string]进行计算。";
        public const string Support = "本项目GitHub仓库链接：\n" +
                                      "https://github.com/SealedL/karasuma-cqbot\n" +
                                      "欢迎帮助我改进程序";
        public const string Comment = "您已成功送出一条留言";
        public const string SyntaxError = "格式错误";
        public const string Wait = "数据传输中，请耐心等待";
        public const string Error = "发生错误，请向管理员报告";
        public const string Busy = "您有命令正在执行";
        
        //User ID related
        public static readonly long MasterId = ReadMasterId();

        private static long ReadMasterId()
        {
            var jsonString = File.ReadAllText("./bin/Debug/netcoreapp3.1/ids.json");
            var idType = JsonSerializer.Deserialize<IdType>(jsonString);
            return idType.MasterId;
        }
    }
}